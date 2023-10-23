echo Moving to automation tools folder from: %CD%
cd BuildAutomationTools

echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p DeliveryDriver\Assets\StreamingAssets -a delivery_driver\asset-bundles

cd..
echo "Script completed."