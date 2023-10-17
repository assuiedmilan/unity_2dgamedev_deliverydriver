"""Logging module"""

import logging
import os
from pathlib import Path

import coloredlogs

LOG_FOLDER = os.path.join(os.path.dirname(Path(__file__).parent.resolve()), "Logs")
Path(LOG_FOLDER).mkdir(parents=True, exist_ok=True)

ROOT_PACKAGE = 'aws_upload'
DEBUG_LOG = os.path.join(LOG_FOLDER, f"{ROOT_PACKAGE}_debug.log")
ERROR_LOG = os.path.join(LOG_FOLDER, f"{ROOT_PACKAGE}_error.log")
CRITICAL_LOG = os.path.join(LOG_FOLDER, f"{ROOT_PACKAGE}_critical.log")

FIELD_COLOR_STYLE = {'asctime': {'color': 'green'},
                     'levelname': {'bold': True, 'color': 'black'},
                     'filename': {'color': 'cyan'},
                     'funcName': {'color': 'blue'}}

LEVEL_COLOR_STYLE = {'critical': {'bold': True, 'color': 'red'},
                     'debug': {'color': 'green'},
                     'error': {'color': 'red'},
                     'info': {'color': 'yellow'},
                     'warning': {'color': 'orange'}}

LOG_FORMAT = "%(asctime)s [%(levelname)s] - [%(filename)s > %(funcName)s() > %(lineno)s] - %(message)s"
LOG_FORMATTER = logging.Formatter(LOG_FORMAT)


def get_logger(name: str) -> logging.Logger:
    """Generate a logger with given name

    Args:
        name(str): Name given to the logger

    Returns:
        The logger
    """

    the_log = logging.getLogger(name)
    the_log.setLevel(logging.DEBUG)

    __install_colors(the_log)

    __add_log_handler(the_log, DEBUG_LOG, logging.DEBUG)
    __add_log_handler(the_log, ERROR_LOG, logging.ERROR)
    __add_log_handler(the_log, CRITICAL_LOG, logging.CRITICAL)

    return the_log


def __install_colors(logger: logging.Logger):
    coloredlogs.install(level=logging.DEBUG,
                        logger=logger,
                        fmt=LOG_FORMAT,
                        datefmt='%H:%M:%S',
                        field_styles=FIELD_COLOR_STYLE,
                        level_styles=LEVEL_COLOR_STYLE)


def __add_log_handler(logger: logging.Logger, log_name: str, level: int):
    logger_handler = logging.FileHandler(log_name)
    logger_handler.setLevel(level)
    logger_handler.setFormatter(LOG_FORMATTER)

    logger.addHandler(logger_handler)
