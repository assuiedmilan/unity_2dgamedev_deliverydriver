using System.IO;
using UnityEngine;

public class BundledObjectLoader : MonoBehaviour
{
    public string assetName = "BundledSpriteObject";
    public string bundleName = "testbundle";
    
    void Start()
    {
        var localAssetBundlePath = Application.streamingAssetsPath;
        var localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(localAssetBundlePath, bundleName));
        
        Debug.Log($"Trying to load assets at path {localAssetBundlePath}");
        
        if (localAssetBundle == null)
        {
            Debug.LogError($"Failed to load AssetBundle {Path.Combine(localAssetBundlePath, bundleName)}!");
            return;
        }
        
        var asset = localAssetBundle.LoadAsset<GameObject>(assetName);
        asset.transform.position = new Vector3(4, -60, 0);
        Instantiate(asset);
        localAssetBundle.Unload(false);
    }
}
