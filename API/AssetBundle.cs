using static LMUI.CoreShell;

namespace LMUI.API
{
    // Need to come up with a name that doesn't clash with UnityEngine.AssetBundle
    public class AssetBundle
    {
        /// <summary>
        /// Loads an asset bundle. Use on initialise melon.
        /// </summary>
        /// <param name="assetBundleName">The name of the asset bundle to load.</param>
        /// <param name="assetBundleFolderPath">The path of the folder that the asset bundle is in.</param>
        /// <returns>The loaded asset bundle as type: UnityEngine.AssetBundle</returns>
        public UnityEngine.AssetBundle Load(string assetBundleName, string assetBundleFolderPath)
        {
            Init();
            return Instance.LoadAssetBundle(assetBundleName, assetBundleFolderPath);
        }
    }
}
