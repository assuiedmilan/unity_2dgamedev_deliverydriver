using System.IO;
using UnityEngine;

public class BundledObjectLoader : MonoBehaviour
{
    public string assetName = "BundledSpriteObject";
    public string bundleName = "testbundle";
    
    void Start()
    {
        var localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        
        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle !");
            return;
        }
        
        var asset = localAssetBundle.LoadAsset<GameObject>(assetName);
        asset.transform.position = new Vector3(4, -60, 0);
        Instantiate(asset);
        localAssetBundle.Unload(false);
    }
}
