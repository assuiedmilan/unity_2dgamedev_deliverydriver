cd DeliveryDriver\Assets\StreamingAssets
echo Listing files at: %CD%
dir

echo Moving to automation tools folder from: %CD%
cd ..\..\BuildAutomationTools 

cd BuildAutomationTools 
echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p DeliveryDriver\Assets\StreamingAssets -a delivery_driver\asset-bundles

cd..
cd..