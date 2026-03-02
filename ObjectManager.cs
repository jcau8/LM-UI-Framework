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

        // Take a prefab out of a bundle
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

        // Instantiate a prefab (which isn't a button) into a game menu
        internal GameObject LoadPrefabIntoGameMenu(GameObject prefab, string menuName)
        {
            // There is probably a better way to do this \/
            if (!string.IsNullOrEmpty(menuName) && menuName.Length <= 8)
            {
                if (menuName.ToLower() == "main")
                {
                    return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreen.transform);
                }
                else if (menuName.ToLower() == "pause")
                {
                    return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreen.transform);
                }
                else if (menuName.ToLower() == "settings")
                {
                    return GameObject.Instantiate(prefab, _objectReferenceManager.SettingsScreen.transform);
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

        internal class CustomButton
        {
            readonly Logger _logger;
            internal CustomButton(Logger logger)
            {
                _logger = logger;
            }

            private readonly ObjectReferenceManager _objectReferenceManager;
            private GameObject _buttonObject;
            private Action _onClickAction;

            public GameObject ButtonObject
            {
                get => _buttonObject;
                set => _buttonObject = value;
            }

            // Take the image and settings from a default game button and apply them to the specified btnObj
            internal void StealGameButtonImage(GameObject btnObj)
            {
                try
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
                catch (Exception)
                {
                    _logger.LogError($"Failed to steal the game button's image and apply it to the object: {btnObj.name}");
                    throw;
                }
            }

            internal static void ApplyHoverHandler(GameObject btnObj)
            {
                btnObj.AddComponent<OnHoverHandler>();
            }

            internal void ApplyGameMenuStyle(GameObject btnObj)
            {
                StealGameButtonImage(btnObj);
                ApplyHoverHandler(btnObj);
            }

            // Instantiate a button prefab into a game menu
            internal GameObject LoadButtonPrefabIntoGameMenu(GameObject btnPrefab, string menuName, bool useMenuStyle)
            {
                GameObject _btnObj;
                // There is probably a better way to do this \/
                if (!string.IsNullOrEmpty(menuName) && menuName.Length <= 8)
                {
                    if (menuName.ToLower() == "main")
                    {
                        if (useMenuStyle)
                        {
                            _btnObj = GameObject.Instantiate(btnPrefab, _objectReferenceManager.MainMenuScreenLayout.transform);
                            ApplyGameMenuStyle(_btnObj);
                            return _btnObj;
                        }
                        return GameObject.Instantiate(btnPrefab, _objectReferenceManager.MainMenuScreen.transform);
                    }
                    else if (menuName.ToLower() == "pause")
                    {
                        if (useMenuStyle)
                        {
                            _btnObj = GameObject.Instantiate(btnPrefab, _objectReferenceManager.PauseScreenLayout.transform);
                            ApplyGameMenuStyle(_btnObj);
                            return _btnObj;
                        }
                        return GameObject.Instantiate(btnPrefab, _objectReferenceManager.PauseScreen.transform);
                    }
                    else if (menuName.ToLower() == "settings")
                    {
                        if (useMenuStyle)
                        {
                            _btnObj = GameObject.Instantiate(btnPrefab, _objectReferenceManager.SettingsScreenLayout.transform);
                            ApplyGameMenuStyle(_btnObj);
                            return _btnObj;
                        }
                        return GameObject.Instantiate(btnPrefab, _objectReferenceManager.SettingsScreen.transform);
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

            // Allows the user to decide what they want to do when the button is clicked
            internal void SetOnClickAction(Action action)
            {
                if (action != null)
                {
                    _onClickAction = action;
                }
                else
                {
                    _logger.LogError($"Failed to assign {action} to _onClickAction because {action} is null");
                }
            }

            internal void OnBtnClick()
            {
                try
                {
                    _onClickAction?.Invoke();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to run user-defined on click action");
                    throw;
                }
            }

            internal void AddOnClickListener(GameObject btnObj)
            {
                btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)OnBtnClick);
            }
        }
    }
}
