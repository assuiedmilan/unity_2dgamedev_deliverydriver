echo Moving to automation tools folder from: %CD%
cd DeliveryDriver\BuildAutomationTools 

echo Calling upload from: %CD%
call upload_to_aws_win -b thundr-staging -p ..\AssetBundles -a delivery_driver\asset-bundles

cd..
cd..