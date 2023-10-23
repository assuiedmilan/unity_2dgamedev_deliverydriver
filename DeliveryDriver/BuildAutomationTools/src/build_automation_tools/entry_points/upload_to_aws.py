"""Upload entry point"""
import argparse
import os
from pathlib import Path

import boto3

from build_automation_tools.system.paths_resolutions import resolve_path_relative_to_git_root
from build_automation_tools.logging_configuration.logging import get_logger


LOGGER = get_logger(__name__)


AWS_ACCESS_KEY_ID = os.getenv("AWS_ACCESS_KEY_ID")
AWS_SECRET_ACCESS_KEY = os.getenv("AWS_SECRET_ACCESS_KEY")
AWS_REGION = os.getenv("AWS_DEFAULT_REGION")
S3 = boto3.client("s3", region_name=AWS_REGION, aws_access_key_id=AWS_ACCESS_KEY_ID, aws_secret_access_key=AWS_SECRET_ACCESS_KEY)


def upload_files(arguments: argparse.Namespace) -> None:
    """Upload the files from the given path to the given bucket"""

    LOGGER.info("Uploading files located at %s to bucket %s under %s", arguments.path_to_assets, arguments.bucket, arguments.amazon_key_root)

    if Path(arguments.path_to_assets).is_file():
        __upload_single_file(arguments)
    elif Path(arguments.path_to_assets).is_dir():
        for root, _, files in os.walk(arguments.path_to_assets):
            for file in files:
                __upload_from_folder(os.path.join(root, file), arguments)
    else:
        LOGGER.error("The given path is not a file or a directory: %s", arguments.path_to_assets)

    LOGGER.info("Upload completed.")


def __upload_single_file(arguments: argparse.Namespace):
    upload_path = os.path.join(arguments.amazon_key_root, os.path.basename(arguments.path_to_assets))
    __upload_file(arguments.path_to_assets, upload_path, arguments)


def __upload_from_folder(file: str, arguments: argparse.Namespace):
    upload_path = extract_upload_path(arguments.amazon_key_root, file, os.path.basename(arguments.path_to_assets))
    __upload_file(file, upload_path, arguments)


def __upload_file(file: str, upload_path: str, arguments: argparse.Namespace) -> None:

    upload_path = replace_separators_to_unix_style(upload_path)

    try:
        S3.upload_file(file, arguments.bucket, upload_path)
        LOGGER.info("Uploaded file: %s to %s}", file, replace_separators_to_unix_style(os.path.join(arguments.bucket, upload_path)))
    except TypeError as type_error:
        LOGGER.error(type_error)
        LOGGER.error("Error uploading file: %s", file)


def extract_upload_path(s3_root_key: str, local_file: str, split_at_folder: str) -> str:
    """Extract the upload path from the local file path

    Args:
        s3_root_key (str): First key folder located under the bucket
        local_file (str): The local file path
        split_at_folder (str): The folder to split the path on

    Returns:
        The upload path
    """

    local_file = Path(local_file)
    return os.path.join(s3_root_key, *local_file.parts[local_file.parts.index(split_at_folder)+1:])


def replace_separators_to_unix_style(path: str) -> str:
    """Replace the separators in the path to unix style

    Args:
        path (str): The path to convert

    Returns:
        The converted path
    """

    return path.replace("\\", "/")


def parse_arguments():
    """
    Use this command to specific bundled assets to be uploaded to AWS

    Example:
        If the bundled asset are located under : ~/my_repository/project/bundled_assets/Windows/001 and want to be uploaded to the bucket: my_bucket
        Then: 'upload_to_aws -b my_bucket -p project/bundled_assets' will upload all the files under ~/my_repository/project/bundled_assets to the bucket my_bucket/Windows/001
    """

    parser = argparse.ArgumentParser(description=parse_arguments.__doc__, formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument('-b', '--bucket', type=str, help='Target bucket')
    parser.add_argument('-p', '--path_to_assets', type=str, help='Relative to the current working directory. Last folder will be considered as the asset root, and the child folders structure will be used as the s3 key for the upload.')
    parser.add_argument('-a', '--amazon_key_root', type=str, help='First key folder located under the bucket, sub-folders will be derived from the child folders structure located under the path_to_asset argument.')

    arguments = parser.parse_args()
    arguments.path_to_assets = resolve_path_relative_to_git_root(arguments.path_to_assets)
    return arguments


def main():
    """Entry point. Must be without arguments"""
    args = parse_arguments()
    upload_files(args)


if __name__ == "__main__":
    main()
