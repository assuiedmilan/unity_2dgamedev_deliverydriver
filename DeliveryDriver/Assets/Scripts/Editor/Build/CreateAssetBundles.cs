using System;
using UnityEditor;
using UnityEngine;

namespace Unity.DeliveryDriver.Editor.Build
{
    public static class CreateAssetBundles
    {
        [MenuItem("DeliveryDriver/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            var assetBundleDirectory = Application.streamingAssetsPath;
            if (!System.IO.Directory.Exists(assetBundleDirectory))
            {
                System.IO.Directory.CreateDirectory(assetBundleDirectory);
            }
        
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
    }
}