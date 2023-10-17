"""This module is the entry point to zip a folder"""
import argparse
import os
import zipfile
from pathlib import Path
from typing import List

from build_automation_tools.logging_configuration.logging import get_logger
from build_automation_tools.system.paths_resolutions import resolve_path_relative_to_git_root, check_is_directory

LOGGER = get_logger(__name__)


def parse_zip_folder_args() -> argparse.Namespace:
    """
    Use this command to archive a folder using the zip format
    """

    parser = argparse.ArgumentParser(description=parse_zip_folder_args.__doc__, formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument('-s', "--source_folder", type=str)
    parser.add_argument('-n', '--name', default="", type=str)
    parser.add_argument('-env', '--environment_variables', default="", type=str, nargs="+", help="List of environment variables that will be added to the archive name, in the form of a space separated string of name/value pairs. Example: -env VAR1 VAR1 will add \"_VALUE_OF_VAR1_VALUE_OF_VAR2 \" to the archive name")

    args = parser.parse_args()
    args.source_folder = resolve_path_relative_to_git_root(args.source_folder)
    args.name = resolve_path_relative_to_git_root(args.name)

    args.name = add_env_variable_values_to_string(args.name, args.environment_variables)

    return args


ZIPFILE_EXT = ".zip"


def zip_folder(folder_path: str, output_filename: str, **kwargs):
    """
    Zip a folder with customizable options using the zipfile library.

    Args:
        folder_path (str): Path to the folder to be zipped.
        output_filename (str): Name of the output zip file.
        **kwargs: Additional options supported by zipfile.ZipFile, such as compression, file format, etc.

    Returns:
        str: Path to the created zip file.

    Raises:
        FileNotFoundError: If the specified folder_path does not exist.
    """

    check_is_directory(folder_path)

    output_filename = output_filename + ZIPFILE_EXT if not output_filename.endswith(ZIPFILE_EXT) else output_filename
    folder_path = Path(folder_path)
    zip_file_path = os.path.join(os.getcwd(), output_filename)

    with zipfile.ZipFile(zip_file_path, 'w', **kwargs) as zipf:
        for file_path in folder_path.rglob("*"):
            zipf.write(
                file_path,
                arcname=file_path.relative_to(folder_path)
            )

    return zip_file_path


def add_env_variable_values_to_string(value: str, variables: List[str]) -> str:
    """Add environment variables values to the provided string, separated by an underscore, if the environment variable is set

    Args:
        value: The string to which the environment variables will be added
        variables: The dictionary of environment variables to add

    Returns:
        The string with the environment variables added
    """
    LOGGER.info("Adding environment variables %s to string: %s", variables, value)
    new_value = value + "".join([f"_{os.getenv(env_var)}" for env_var in variables if os.getenv(env_var)])

    if not value:
        new_value = new_value[1:]

    LOGGER.info("Resulting string: %s", new_value)
    return new_value


def main():
    """Entry point. Must be without arguments"""
    args = parse_zip_folder_args()
    zip_file_path = zip_folder(args.source_folder, args.name)

    if zip_file_path:
        LOGGER.info("Zip file created at %s}", zip_file_path)
    else:
        LOGGER.warning("Zip file creation failed")


if __name__ == "__main__":
    main()  # pragma: no cover
