"""Upload entry point"""

import argparse
import os
from pathlib import Path

import boto3

from aws_upload.logging_configuration.logging import get_logger

LOGGER = get_logger(__name__)

AWS_ACCESS_KEY_ID = os.getenv("AWS_ACCESS_KEY_ID")
AWS_SECRET_ACCESS_KEY = os.getenv("AWS_SECRET_ACCESS_KEY")
AWS_REGION = os.getenv("AWS_DEFAULT_REGION")
S3 = boto3.client("s3", region_name=AWS_REGION, aws_access_key_id=AWS_ACCESS_KEY_ID, aws_secret_access_key=AWS_SECRET_ACCESS_KEY)


def upload_files(arguments: argparse.Namespace) -> None:
    """Upload the files from the given path to the given bucket"""

    LOGGER.info("Uploading files located at %s to bucket %s under %s", arguments.path_to_assets, arguments.bucket, arguments.amazon_key_root)
    
    for root, _, files in os.walk(arguments.path_to_assets):
        for file in files:

            local_file_path = os.path.join(root, file)
            upload_path = extract_upload_path(Path(local_file_path), os.path.basename(arguments.path_to_assets), arguments.amazon_key_root)
            s3_key = replace_separators_to_unix_style(upload_path)

            try:
                S3.upload_file(local_file_path, arguments.bucket, s3_key)
                LOGGER.info("Uploaded file: %s to %s}", local_file_path, replace_separators_to_unix_style(os.path.join(arguments.bucket, s3_key)))
            except TypeError as type_error:
                LOGGER.error(type_error)
                LOGGER.error("Error uploading file: %s", local_file_path)

    LOGGER.info("Upload completed.")


def extract_upload_path(local_file: Path, split_folder: str, s3_root_key: str) -> str:
    """Extract the upload path from the local file path

    Args:
        local_file (Path): The local file path
        split_folder (str): The folder to split the path on
        s3_root_key (str): First key folder located under the bucket

    Returns:
        The upload path
    """

    return os.path.join(s3_root_key, *local_file.parts[local_file.parts.index(split_folder)+1:])


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
    arguments.path_to_assets = os.path.normpath(os.path.join(os.getcwd(), arguments.path_to_assets))
    return arguments


def main():
    """Entry point. Must be without arguments"""
    args = parse_arguments()
    upload_files(args)


if __name__ == "__main__":
    main()
