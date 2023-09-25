#!/bin/bash

brew uninstall --ignore-dependencies python3 || true
brew install python@3.11
brew link --force python@3.11
python3.11 --version
python3.11 -m pip install pip --upgrade 
python3.11 -m pip install poetry 
sudo ln -sf /usr/local/bin/python3.11 /usr/local/bin/python
sudo ln -sf /usr/local/bin/python3.11 /usr/local/bin/python3
sudo ln -sf /usr/local/bin/pip3.11 /usr/local/bin/pip
sudo ln -sf /usr/local/bin/pip3.11 /usr/local/bin/pip3
python3 -m pip install poetry
echo "CURRENT DIRECTORY CONTENT"
ls
echo "TRYING TO INSTALL"

cd DeliveryDriver/BundledAssetsUploader && python3 -m poetry lock
python3 -m poetry install
python3 -m poetry run upload_to_aws -b thundr-staging -p AssetBundles -a asset-bundles

echo "Script completed."
