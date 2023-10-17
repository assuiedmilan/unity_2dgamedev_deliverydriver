import os
from pathlib import Path
from typing import Optional, Callable

from build_automation_tools.logging_configuration.logging import get_logger


LOGGER = get_logger(__name__)


def resolve_path_relative_to_git_root(path_to_resolve: Optional[str]) -> Optional[str]:
    """From a given path, resolve it relative to the git root obtained from the current directory

    Note:
        Path will be returned as it if it is absolute

    Args:
        path_to_resolve(str)

    Returns:
        The resolved path, as a str
    """

    if path_to_resolve is None:
        return None

    if not os.path.isabs(path_to_resolve):
        path_to_resolve = os.path.join(search_git_root(os.getcwd()), path_to_resolve)

    return os.path.normpath(path_to_resolve)


def search_git_root(from_dir: str) -> Path:
    """From given directory, look in the parents directory if a git repository root, or a git submodule is found

    The first valid repository will be returned.

    Args:
        from_dir (str): The starting dir

    Raises:
        FileNotFoundError if git root is not found.

    Returns:
        Git root path
    """

    check_is_directory(from_dir)

    path = Path(from_dir)
    paths = [path, *path.parents]

    for directory, git_candidate in zip(paths, [os.path.join(x, '.git') for x in paths]):

        result = [True for n in os.listdir(directory) if n == '.git'] + [os.path.isdir(git_candidate)]

        if any(result):
            return directory

    error_message = f"Found no git repository under {paths}"
    LOGGER.error(error_message)
    raise FileNotFoundError(error_message)


def check_is_directory(*args: str) -> None:
    """Check that every receive argument is a valid directory.

    Args:
        *args (str): Every path to check, separated by a comma

    Raises:
        FileNotFoundError if one of the provided directories does not exist
    """

    def __check_using(value: str, function: Callable):
        if not function(value):
            error_message = f"Requested folder or file {value} does not exist."
            LOGGER.error(error_message)
            raise FileNotFoundError(error_message)

    list(map(__check_using, args, [os.path.isdir] * len(args)))
