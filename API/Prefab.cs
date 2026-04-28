using UnityEngine;
using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Prefab
    {
        /// <summary>
        /// Loads a prefab from a given asset bundle.
        /// </summary>
        /// <param name="loadedAssetBundle">The asset bundle to load the prefab from.</param>
        /// <param name="prefabPath">The path to the prefab you want to load.</param>
        /// <returns>The prefab as a GameObject.</returns>
        public GameObject Load(UnityEngine.AssetBundle loadedAssetBundle, string prefabPath)
        {
            Init();
            Instance.RefreshReferences();
            return Instance.LoadPrefabFromBundle(loadedAssetBundle, prefabPath);
        }
    }
}
