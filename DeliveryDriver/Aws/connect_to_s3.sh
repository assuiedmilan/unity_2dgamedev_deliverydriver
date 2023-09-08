#!/bin/bash

# Check if the AWS CLI is already installed
if ! command -v aws &> /dev/null; then
    echo "AWS CLI is not installed. Installing it now..."

    # Check if we are on a Debian/Ubuntu-based system
    if [ -x "$(command -v apt-get)" ]; then
        sudo apt-get update
        sudo apt-get install -y awscli
    # Check if we are on a Red Hat/CentOS-based system
    elif [ -x "$(command -v yum)" ]; then
        sudo yum install -y awscli
    else
        echo "Unsupported package manager. Please install the AWS CLI manually and run this script again."
        exit 1
    fi
fi

# Use the AWS CLI to list S3 buckets
aws s3 ls

#Set upload bucket
aws s3 ls s3://thunder-testing

#Upload to destination folder
folder_name=$(aws s3 cp .DeliveryDriver/Assets/StreamingAssets/ s3://thundr-testing/asset-bundles/$BUNDLE_TARGET/001 --recursive 2>&1 | grep -o '$BUNDLE_TARGET/[0-9]*' | sort -u)
