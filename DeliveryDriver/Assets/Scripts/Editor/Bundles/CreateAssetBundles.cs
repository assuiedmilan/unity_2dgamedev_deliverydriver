using UnityEditor;

public static class CreateAssetBundles
{
    [MenuItem("Assets/Asset Bundles/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        const string assetBundleDirectory = "AssetsBundles/Windows/001";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}