import argparse
import os
import boto3

# Define your AWS credentials (ensure AWS CLI is configured or use environment variables)
aws_access_key_id = os.getenv("AWS_ACCESS_KEY_ID")
aws_secret_access_key = os.getenv("AWS_SECRET_ACCESS_KEY")
aws_region = os.getenv("AWS_DEFAULT_REGION")

# Create an S3 client
s3 = boto3.client("s3", region_name=aws_region, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key)


def upload_files(folder_to_upload, bucket_to_upload_to, folder_number, s3_prefix=""):
    for root, dirs, files in os.walk(folder_to_upload):
        for file in files:
            local_file = os.path.join(root, file)
            s3_key = os.path.join("asset-bundles", "Windows", folder_number, file).replace("\\", "/")
            try:
                s3.upload_file(local_file, bucket_to_upload_to, s3_key)
                print(f"Uploaded file: {local_file} to {os.path.join(bucket_to_upload_to, s3_key)}")
            except TypeError as e:
                print(e)
                print("Error uploading file: {}".format(local_file))

    print("Upload completed.")


def parse_folder_number():
    parser = argparse.ArgumentParser(description='Parse the folder number.')
    parser.add_argument('-n', '--folder_number', type=str, help='Target folder number')
    parser.add_argument('-b', '--bucket', type=str, help='Target bucket')
    parser.add_argument('-p', '--path_to_assets', type=str, help='Relative to the current working directory')

    arguments = parser.parse_args()
    print(f"Current working directory: {os.getcwd()}")
    print(f"Received assets path: {arguments.path_to_assets}")
    arguments.path_to_assets = os.path.normpath(os.path.join(os.getcwd(), arguments.path_to_assets))
    print(f"Absolute assets path: {arguments.path_to_assets}")
    return arguments


if __name__ == "__main__":
    args = parse_folder_number()
    print(f"Trying to upload bundled assets located at {args.path_to_assets} to bucket {args.bucket} in folder {args.folder_number}")
    upload_files(args.path_to_assets, args.bucket, args.folder_number)
