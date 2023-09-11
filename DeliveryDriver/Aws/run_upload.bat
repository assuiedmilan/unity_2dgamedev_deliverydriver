REM Batch file will be executed from the root of UCB working directory (BUILDPATH\p\) which means the executable must be at the root at the repository, while the script must be in the folder the project is located. This means this batch file will not work locally.

call upload_to_aws -n 004 -b thundr-staging -p .build\last\asset-bundles-content-only-windows-desktop-64-bit\extra_data\asset_bundles\