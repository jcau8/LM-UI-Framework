using MelonLoader.Utils;
using UnityEngine;

namespace LMUI
{
    internal class FileHandler
    {
        readonly Logger _logger;
        internal FileHandler(Logger logger)
        {
            _logger = logger;
        }

        private static readonly string _defaultAssetBundleFolderPath = Path.Combine(MelonEnvironment.GameRootDirectory, "AssetBundles");

        internal class AssetBundlesFolder
        {
            readonly Logger _logger;
            internal AssetBundlesFolder(Logger logger)
            {
                _logger = logger;
            }

            internal void CreateDefault()
            {
                try
                {
                    // Check if the folder already exists
                    if (!Directory.Exists(_defaultAssetBundleFolderPath))
                    {
                        Directory.CreateDirectory(_defaultAssetBundleFolderPath);
                        _logger.LogInfo("Sucessfully created AssetBundles folder in your game root directory.");
                    }
                    else
                    {
                        _logger.LogInfo("The AssetBundles folder already exists in your game root directory.");
                    }
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to create AssetBundles folder.");
                    throw;
                }
            }

            internal void CreateCustom(string assetBundleFolderPath)
            {
                try
                {
                    // Check if the folder already exists
                    if (!Directory.Exists(assetBundleFolderPath))
                    {
                        Directory.CreateDirectory(assetBundleFolderPath);
                        _logger.LogInfo($"Sucessfully created: {assetBundleFolderPath}");
                    }
                    else
                    {
                        _logger.LogInfo($"{assetBundleFolderPath} folder already exists.");
                    }
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to create folder at:\n{assetBundleFolderPath}");
                    throw;
                }
            }
        }

        public class AssetBundleHandler
        {
            // Not sure if I'm supposed to add this again or not... \/
            // Well it fixes the error so I guess it'll be fine
            readonly Logger _logger;
            internal AssetBundleHandler(Logger logger)
            {
                _logger = logger;
            }

            private string _assetBundleName = null;
            private string _assetBundleFolderPath = _defaultAssetBundleFolderPath;
            private string _assetBundlePath;
            private AssetBundle _loadedAssetBundle;

            public string FolderPath
            {
                get
                {
                    return _assetBundleFolderPath;
                }
                set
                {
                    if (value.GetType() == typeof(string))
                    {
                        _assetBundleFolderPath = value;
                    }
                }
            }

            // Name of the asset bundle to load
            public string BundleName
            {
                get
                {
                    return _assetBundleName;
                }
                set
                {
                    if (value.GetType() == typeof(string) && value != null && !value.EndsWith(".assetbundle"))
                    {
                        _assetBundleName = value;
                    }
                }
            }

            public AssetBundle LoadAssetBundle(string assetBundleName, string assetBundleFolderPath)
            {
                try
                {
                    // Make sure the directory exists
                    if (Directory.Exists(assetBundleFolderPath))
                    {
                        _assetBundlePath = Path.Combine(assetBundleFolderPath + assetBundleName);
                        // Make sure file exists
                        if (File.Exists(_assetBundlePath))
                        {
                            _loadedAssetBundle = AssetBundle.LoadFromFile(_assetBundlePath);
                        }
                        else
                        {
                            // The file doesn't exist so exit before any functions are run on it
                            _logger.LogError($"The file at path: {_assetBundlePath} doesn't exist");
                            _logger.LogInfo("Returning...");
                            return null;
                        }
                    }
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to load asset bundle from path:\n{_assetBundlePath}\nCheck that the file exists.");
                    throw;
                }

                if (_loadedAssetBundle != null)
                {
                    // Loaded successfully
                    _logger.LogInfo($"Successfully loaded asset bundle from path:\n{_assetBundlePath}");
                    return _loadedAssetBundle;
                }
                else if (_assetBundlePath == null)
                {
                    _logger.LogError($"Failed to load asset bundle from path:\n{_assetBundlePath}\nThe bundle is null");
                }

                // An error has occured so return without data
                return null;
            }

            /// <summary>
            /// Collects all garbage then unloads the given bundle and all its loaded objects. For use OnDeinitializeMelon
            /// </summary>
            internal void Unload(AssetBundle loadedAssetBundle)
            {
                if (loadedAssetBundle != null)
                {
                    System.GC.Collect();
                    loadedAssetBundle.Unload(true);
                }
                else
                {
                    _logger.LogInfo($"{loadedAssetBundle.name} is null");
                }
            }
        }
    }
}
