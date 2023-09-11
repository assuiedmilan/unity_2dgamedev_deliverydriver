poetry install
poetry run pyinstaller --onefile .\src\aws_upload\entry_points\upload_to_aws.py --exclude-module _bootlocale
echo Y | xcopy dist\upload_to_aws.exe ..\..
rmdir  build /q /s
rmdir  dist /q /s
pause