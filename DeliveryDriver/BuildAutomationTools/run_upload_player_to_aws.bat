set archive_target=build_to_aws
set aws_bucket=thunder-staging
set bucket_root_folder=udemy
set project_name=delivery_driver
set release_folder=delivery_driver
set release_archive=delivery_driver_win.zip
set build_folder=build
set patchmedata_file=PatchMetadata.json
set release_branch=main

cd BuildAutomationTools

call zip_folder_win.exe -s %build_folder% -d "%archive_target%/%project_name%-%BUILD_PLATFORM%-%UCB_BUILD_NUMBER%"
call upload_to_aws_win -b %aws_bucket% -p %archive_target% -a %bucket_root_folder%/%BUILD_PLATFORM%/%SCM_BRANCH%

if "%SCM_BRANCH%"=="%release_branch%" (
    rmdir /S /Q ..\%archive_target%
    call zip_folder_win.exe -s %build_folder% -d %archive_target%/%release_archive%"
    call upload_to_aws_win -b %aws_bucket% -p %archive_target%/%release_archive% -a "%bucket_root_folder%/%BUILD_PLATFORM%/%release_folder%"
    call upload_to_aws_win -b %aws_bucket% -p %build_folder%/%patchmedata_file% -a "%bucket_root_folder%/%BUILD_PLATFORM%/%release_folder%"
)

cd..
echo "Script completed."
