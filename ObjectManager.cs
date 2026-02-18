using UnityEngine;

namespace LMUI
{
    internal class ObjectManager
    {
        readonly Logger _logger;
        internal ObjectManager(Logger logger)
        {
            _logger = logger;
        }

        readonly ObjectReferenceManager _objectReferenceManager;

        private GameObject _loadedPrefab;
        // Take the prefab out of the bundle
        internal GameObject LoadPrefabFromBundle(AssetBundle loadedBundle, string prefabName)
        {
            try
            {
                // Load the prefab from the bundle into a variable
                _loadedPrefab = loadedBundle.LoadAsset<GameObject>(prefabName);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to load prefab: {prefabName}, from bundle: {loadedBundle.name}");
                throw;
            }

            if (_loadedPrefab != null)
            {
                _logger.LogInfo($"Successfully loaded prefab: {prefabName}, from bundle: {loadedBundle.name}");
                return _loadedPrefab;
            }
            else if (_loadedPrefab == null)
            {
                _logger.LogError($"Failed to load prefab: {prefabName}, from bundle: {loadedBundle.name}. The prefab is null");
            }

            // An error occured, return without data
            return null;
        }

        // Takes the image and settings from a default game button and applies them to the specified btnObj
        internal void StealGameButtonImage(GameObject btnObj)
        {
            GameObject mainMenuOptionsBtn = _objectReferenceManager.MainMenuOptionsButton;

            UnityEngine.UI.Image sourceImage = mainMenuOptionsBtn.GetComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Image btnImage = btnObj.GetComponent<UnityEngine.UI.Image>();

            btnImage.sprite = sourceImage.sprite;
            btnImage.color = sourceImage.color;
            btnImage.material = sourceImage.material;
            btnImage.type = sourceImage.type;
            btnImage.preserveAspect = sourceImage.preserveAspect;
        }

        // Instantiate the prefab into a game menu
        internal GameObject LoadPrefabIntoMenu(GameObject prefab, string menuName, bool useMenuStyle)
        {
            // There has GOT to be a better way to do this \/
            if (!string.IsNullOrEmpty(menuName) && menuName.Length <= 8)
            {
                if (menuName.ToLower() == "main")
                {
                    if (useMenuStyle)
                    {
                        StealGameButtonImage(prefab);
                        return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreenLayout.transform); ;
                    }
                    else
                    {
                        return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreen.transform);
                    }
                }
                else if (menuName.ToLower() == "pause")
                {
                    if (useMenuStyle)
                    {
                        StealGameButtonImage(prefab);
                        return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreenLayout.transform);
                    }
                    else
                    {
                        return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreen.transform);
                    }
                }
                else if (menuName.ToLower() == "settings")
                {
                    if (useMenuStyle)
                    {
                        StealGameButtonImage(prefab);
                        return GameObject.Instantiate(prefab, _objectReferenceManager.SettingsScreenLayout.transform);
                    }
                    else
                    {
                        return GameObject.Instantiate(prefab, _objectReferenceManager.SettingsScreen.transform);
                    }
                }
                else
                {
                    _logger.LogError($"{menuName} isn't a valid menu name. The valid names are: \"main\", \"pause\" and \"settings\"");
                    return null;
                }
            }
            else
            {
                _logger.LogError($"{menuName} isn't a valid menu name. The valid names are: \"main\", \"pause\" and \"settings\"");
                return null;
            }
        }
    }
}
