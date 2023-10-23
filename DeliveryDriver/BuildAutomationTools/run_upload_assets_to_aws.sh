#!/bin/bash

cd BuildAutomationTools

chmod a+x ./run_python_setup_macos.sh
/bin/bash ./run_python_setup_macos.sh

python3 -m poetry run upload_to_aws -b thundr-staging -p DeliveryDriver\Assets\StreamingAssets -a -a delivery_driver\asset-bundles

cd ..
echo "Script completed."
