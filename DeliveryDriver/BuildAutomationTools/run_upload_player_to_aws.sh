#!/bin/bash

archive_target="build_to_aws"
aws_bucket="thunder-staging"
bucket_root_folder="udemy"
project_name="delivery_driver"
release_folder="delivery_driver"
release_archive="delivery_driver_mac.zip"
build_folder="build"
patchmedata_file="PatchMetadata.json"
release_branch="main"

cd BuildAutomationTools

chmod a+x ./run_python_setup_macos.sh
/bin/bash ./run_python_setup_macos.sh

python3 -m poetry run zip_folder -s $build_folder -d "$archive_target/$project_name-$BUILD_PLATFORM-$UCB_BUILD_NUMBER"
python3 -m poetry run upload_to_aws -b $aws_bucket -p $archive_target -a "$bucket_root_folder/$BUILD_PLATFORM/$SCM_BRANCH"

if [ "$SCM_BRANCH" == "$release_branch" ]; then
    rm -rf "../$archive_target"
    python3 -m poetry run zip_folder -s $build_folder -d "$archive_target/$release_archive"
    python3 -m poetry run upload_to_aws -b "$aws_bucket" -p "$archive_target/$release_archive" -a "$bucket_root_folder/$BUILD_PLATFORM/$release_folder"
    python3 -m poetry run upload_to_aws -b "$aws_bucket" -p "$build_folder/$patchmedata_file" -a "$bucket_root_folder/$BUILD_PLATFORM/$release_folder"
fi

cd ..
echo "Script completed."
