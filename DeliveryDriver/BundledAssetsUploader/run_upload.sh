#!/bin/bash

chmod +x upload_to_aws
./upload_to_aws -b thundr-staging -p AssetBundles -a asset-bundles

echo "Script completed."
