using MelonLoader.Utils;
using UnityEngine;

namespace LMUI.Core
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

            /// <summary>
            /// Creates the default asset bundle folder if it doesn't already exist.
            /// </summary>
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

            /// <summary>
            /// Creates a folder at the specified path.
            /// </summary>
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

        internal class AssetBundleHandler
        {
            readonly Logger _logger;
            internal AssetBundleHandler(Logger logger)
            {
                _logger = logger;
            }

            private string _assetBundlePath;
            private AssetBundle _loadedAssetBundle;

            /// <summary>
            /// Loads an asset bundle.
            /// </summary>
            /// <returns>
            /// The loaded asset bundle.
            /// </returns>
            internal AssetBundle LoadAssetBundle(string assetBundleName, string assetBundleFolderPath)
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
                // FIXME
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
