using static LMUI.CoreShell;

namespace LMUI.API
{
    public class AssetBundlesFolder
    {
        /// <summary>
        /// Creates the default asset bundle folder if it doesn't already exist.
        /// </summary>
        public void CreateDefault()
        {
            Init();
            Instance.CreateDefault();
        }

        /// <summary>
        /// Creates a folder at the specified path.
        /// </summary>
        public void CreateCustom(string assetBundleFolderPath)
        {
            Init();
            Instance.CreateCustom(assetBundleFolderPath);
        }
    }
}
