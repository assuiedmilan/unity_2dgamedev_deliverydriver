poetry install
poetry run pyinstaller --onefile .\src\build_automation_tools\entry_points\upload_to_aws.py --exclude-module _bootlocale
poetry run pyinstaller --onefile .\src\build_automation_tools\entry_points\zip_folder.py --exclude-module _bootlocale

echo "Moving executable to repository root directory"
echo F | xcopy dist\upload_to_aws.exe upload_to_aws_win.exe /Y
echo F | xcopy dist\zip_folder.exe zip_folder_win.exe /Y

rmdir  build /q /s
rmdir  dist /q /s
del upload_to_aws.spec /q /f
del zip_folder.spec /q /f
