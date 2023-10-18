REM D:\UnityDownloader\2022.3.0f1\unity.exe --batchmode --quit --silent --projectPath . --executeMethod Unity.DeliveryDriver.Editor.Build.BuildSetup.RunBatchModeBuild

cd DeliveryDriver\build
echo Listing files at: %CD%
dir

echo Moving to automation tools folder from: %CD%
cd ..\BuildAutomationTools 

echo Creating upload folder from: %CD%
mkdir ..\build_upload

echo Calling zip from: %CD%
REM Zip must have the same name as the one expected by UCB to ensure it will be archived and appear as artifact. If artifacts are no longer uploaded, verify the UCB project name and find an old log to get/change the syntax
REM Zipping twice as we want to upload a meaningful zipfile on AWS, which will contain the branch name
call zip_folder_win.exe -s DeliveryDriver/build -d build_archive/deliverydriver
call zip_folder_win.exe -s DeliveryDriver/build -d build_upload/deliverydriver -env SCM_BRANCH

echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p build_upload -a delivery_driver/builds

cd..
cd..