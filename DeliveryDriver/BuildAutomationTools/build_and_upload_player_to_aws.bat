REM D:\UnityDownloader\2022.3.0f1\unity.exe --batchmode --quit --silent --projectPath . --executeMethod Unity.DeliveryDriver.Editor.Build.BuildSetup.RunBatchModeBuild

cd DeliveryDriver\build
echo Listing files at: %CD%
dir

echo Moving to automation tools folder from: %CD%
cd ..\BuildAutomationTools 

echo Creating upload folder from: %CD%
mkdir ..\build_upload

echo Calling zip from: %CD%
call zip_folder_win.exe -s ../build/delivery_driver -n ../build_upload/delivery_driver -env BRANCH %SCM_BRANCH%

echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p ../build_upload -a delivery_driver/builds

cd..
cd..