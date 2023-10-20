REM D:\UnityDownloader\2022.3.0f1\unity.exe --batchmode --quit --silent --projectPath . --executeMethod Unity.DeliveryDriver.Editor.Build.BuildSetup.RunBatchModeBuild

echo Initial directory: %CD%

cd DeliveryDriver
echo Listing files at: %CD%
dir

echo Moving to automation tools folder from: %CD%
cd BuildAutomationTools 

echo Creating upload folder from: %CD%
set archive_target=..\build_to_aws
mkdir %archive_target%

echo Calling zip from: %CD%
call zip_folder_win.exe -s DeliveryDriver/build -d %archive_target%/deliverydriver -env SCM_BRANCH

echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p %archive_target% -a _milan_tests/delivery_driver/builds

cd..
cd..