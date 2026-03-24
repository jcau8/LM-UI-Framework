using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace LMUI
{
    internal class ObjectManager
    {
        readonly Logger _logger;
        readonly ObjectReferenceManager _objectReferenceManager;
        internal ObjectManager(Logger logger, ObjectReferenceManager objectReferenceManager)
        {
            _logger = logger;
            _objectReferenceManager = objectReferenceManager;
        }

        private GameObject _loadedPrefab;

        /// <summary>
        /// Holds all possible menu screen types.
        /// </summary>
        internal enum MenuScreen
        {
            MainMenu,
            PauseMenu,
            SettingsMenu
        }

        /// <summary>
        /// Take a Prefab out of an AssetBundle.
        /// </summary>
        /// <returns>
        /// The prefab as a GameObject
        /// </returns>
        internal GameObject LoadPrefabFromBundle(AssetBundle loadedAssetBundle, string prefabPath)
        {
            try
            {
                // Load the prefab from the bundle into a variable
                _loadedPrefab = loadedAssetBundle.LoadAsset<GameObject>(prefabPath);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to load prefab: {prefabPath}, from bundle: {loadedAssetBundle.name}");
                throw;
            }

            if (_loadedPrefab != null)
            {
                _logger.LogInfo($"Successfully loaded prefab: {prefabPath}, from bundle: {loadedAssetBundle.name}");
                return _loadedPrefab;
            }
            else if (_loadedPrefab == null)
            {
                _logger.LogError($"Failed to load prefab: {prefabPath}, from bundle: {loadedAssetBundle.name}. The prefab is null");
            }

            // An error occured, return without data
            _logger.LogError("Error occured, returning null");
            return null;
        }

        /// <summary>
        /// Checks whether the given menu screen is active or not.
        /// </summary>
        /// <returns>
        /// A boolean representing the state of the given menu.
        /// </returns>
        internal bool TargetMenuActive(MenuScreen targetMenu)
        {
            // Using the layouts instead of the actual screens due to the order of which they are added.
            // Once the layout is added we know that the actual menu exists.
            switch (targetMenu)
            {
                case MenuScreen.MainMenu:
                    return _objectReferenceManager.MainMenuScreenLayout != null;

                case MenuScreen.PauseMenu:
                    return _objectReferenceManager.PauseScreenLayout != null;

                case MenuScreen.SettingsMenu:
                    return _objectReferenceManager.SettingsScreenLayout != null;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Instantiate a prefab (which isn't a button) into a game menu
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject
        /// </returns>
        internal GameObject InstantiatePrefabIntoMenuScreen(GameObject prefab, MenuScreen targetMenu)
        {
            // There is probably a better way to handle when the menu isn't active
            try
            {
                if (TargetMenuActive(targetMenu))
                {
                    switch (targetMenu)
                    {
                        case MenuScreen.MainMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreen.transform);

                        case MenuScreen.PauseMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreen.transform);

                        case MenuScreen.SettingsMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.SettingsScreen.transform);

                        default:
                            _logger.LogError($"{targetMenu} isn't a valid menu.");
                            return null;
                    }
                }
                _logger.LogInfo($"{targetMenu} isn't active. Returning null...");
                return null;
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to instantiate prefab: {prefab.name} into menu: {targetMenu}");
                throw;
            }
        }

        /// <summary>
        /// Deletes all children of a given GameObject
        /// </summary>
        /// <returns>
        /// Given GameObject without children
        /// </returns>
        internal GameObject DeleteAllChildren(GameObject parentObject)
        {
            try
            {
                List<GameObject> _children = [];
                GameObject _parentObject = parentObject;

                foreach (Transform child in _parentObject.transform)
                {
                    _children.Add(child.gameObject);
                }
                _children.ForEach(child => GameObject.Destroy(child));

                return _parentObject;
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to delete children from {parentObject.name}");
                throw;
            }
        }

        /// <summary>
        /// Duplicates the layout GameObject from a menu and removes all children
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed
        /// </returns>
        internal GameObject DuplicateAndClearLayoutFromMenu(MenuScreen targetMenu, string newLayoutName)
        {
            try
            {
                GameObject _newLayout;
                switch (targetMenu)
                {
                    case MenuScreen.MainMenu:
                        _newLayout = GameObject.Instantiate(_objectReferenceManager.MainMenuScreenLayout, _objectReferenceManager.MainMenuScreenLayout.transform);
                        _newLayout.name = newLayoutName;
                        _newLayout = DeleteAllChildren(_newLayout);
                        return _newLayout;

                    case MenuScreen.PauseMenu:
                        _newLayout = GameObject.Instantiate(_objectReferenceManager.PauseScreenLayout, _objectReferenceManager.PauseScreenLayout.transform);
                        _newLayout.name = newLayoutName;
                        _newLayout = DeleteAllChildren(_newLayout);
                        return _newLayout;

                    case MenuScreen.SettingsMenu:
                        _newLayout = GameObject.Instantiate(_objectReferenceManager.SettingsScreenLayout, _objectReferenceManager.SettingsScreenLayout.transform);
                        _newLayout.name = newLayoutName;
                        _newLayout = DeleteAllChildren(_newLayout);
                        return _newLayout;

                    default:
                        _logger.LogError($"{targetMenu} isn't a valid menu.");
                        return null;
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to duplicate and clear the layout from {targetMenu}");
                throw;
            }
        }

        /// <summary>
        /// Class containing functions for instantiating and manipulating button GameObjects
        /// </summary>
        internal class LMUIButton
        {
            readonly Logger _logger;
            readonly ObjectReferenceManager _objectReferenceManager;
            internal LMUIButton(Logger logger, ObjectReferenceManager objectReferenceManager)
            {
                _logger = logger;
                _objectReferenceManager = objectReferenceManager;
            }

            private GameObject _buttonObject;
            private System.Action _onClickAction;

            internal GameObject ButtonObject
            {
                get => _buttonObject;
                set => _buttonObject = value;
            }


            /// <summary>
            /// Allows the user to decide what they want to do when the button is clicked
            /// </summary>
            internal System.Action OnClickAction
            {
                get => _onClickAction;
                set
                {
                    if (value != null)
                    {
                        _onClickAction = value;
                    }
                }
            }

            /// <summary>
            /// Takes the image and settings from a default game button and apply them to the given GameObject
            /// </summary>
            /// <returns>
            /// Returns the given GameObject with the game buttons' image and settings applied
            /// </returns>
            internal GameObject StealGameButtonImage(GameObject btnObj)
            {
                try
                {
                    GameObject _btnObj = btnObj;
                    GameObject mainMenuOptionsBtn = _objectReferenceManager.MainMenuOptionsButton;

                    UnityEngine.UI.Image _sourceImage = mainMenuOptionsBtn.GetComponent<UnityEngine.UI.Image>();
                    UnityEngine.UI.Image _btnImage = _btnObj.GetComponent<UnityEngine.UI.Image>();

                    _btnImage.sprite = _sourceImage.sprite;
                    _btnImage.color = _sourceImage.color;
                    _btnImage.material = _sourceImage.material;
                    _btnImage.type = _sourceImage.type;
                    _btnImage.preserveAspect = _sourceImage.preserveAspect;

                    return _btnObj;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to steal the game buttons' image and apply it to: {btnObj.name}");
                    throw;
                }
            }

            /// <summary>
            /// Register the OnHoverHandler as a type in Il2Cpp. USE ON INITIALISE MELON!
            /// </summary>
            internal void RegisterOnHoverHandler()
            {
                try
                {
                    ClassInjector.RegisterTypeInIl2Cpp<OnHoverHandler>();
                }
                catch (Exception)
                {
                    _logger.LogInfo("Failed to register OnHoverHandler as a type in il2cpp");
                }
            }

            /// <summary>
            /// Adds an OnHoverHandler script that emulates the game button's on hover appearance
            /// </summary>
            /// <returns>
            /// The given button GameObject with the OnHoverHandler script added
            /// </returns>
            internal GameObject ApplyHoverHandler(GameObject btnObj)
            {
                try
                {
                    GameObject _btnObj = btnObj;
                    OnHoverHandler _hoverHandler = _btnObj.AddComponent<OnHoverHandler>();
                    _hoverHandler.Init();
                    return _btnObj;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to apply OnHoverHandler script to {btnObj.name}");
                    throw;
                }
            }

            /// <summary>
            /// Applies the game's hover effect and button image to a button GameObject
            /// </summary>
            /// <returns>
            /// The given button GameObject with the game's hover effect and button image.
            /// </returns>
            internal GameObject ApplyGameMenuStyle(GameObject btnObj)
            {
                try
                {
                    GameObject _btnObj = btnObj;
                    StealGameButtonImage(_btnObj);
                    ApplyHoverHandler(_btnObj);
                    return _btnObj;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to apply the game menu style to: {btnObj.name}");
                    throw;
                }
            }

            /// <summary>
            /// Instantiate a prefab (which will be used as a button) into a game menu
            /// </summary>
            /// <returns>
            /// The instatiated prefab as a GameObject
            /// </returns>
            internal GameObject InstantiateButtonPrefabIntoGameMenu(GameObject btnPrefab, MenuScreen targetMenu, bool useLayout)
            {
                try
                {
                    switch (targetMenu)
                    {
                        case MenuScreen.MainMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(btnPrefab, _objectReferenceManager.MainMenuScreenLayout.transform);
                            }
                            return GameObject.Instantiate(btnPrefab, _objectReferenceManager.MainMenuScreen.transform);

                        case MenuScreen.PauseMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(btnPrefab, _objectReferenceManager.PauseScreenLayout.transform);
                            }
                            return GameObject.Instantiate(btnPrefab, _objectReferenceManager.PauseScreen.transform);

                        case MenuScreen.SettingsMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(btnPrefab, _objectReferenceManager.SettingsScreenLayout.transform);
                            }
                            return GameObject.Instantiate(btnPrefab, _objectReferenceManager.SettingsScreen.transform);

                        default:
                            _logger.LogError($"{targetMenu} isn't a valid menu.");
                            return null;
                    }
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to instantiate {btnPrefab.name} into {targetMenu}");
                    throw;
                }
            }

            /// <summary>
            /// Invokes the user defined on click action
            /// </summary>
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

            /// <summary>
            /// Adds an on click listener to the given button GameObject
            /// </summary>
            /// <returns>
            /// The given button GameObject with the on click listener added
            /// </returns>
            internal GameObject AddOnClickListener(GameObject btnObj)
            {
                try
                {
                    GameObject _btnObj = btnObj;
                    _btnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)OnBtnClick);
                    return _btnObj;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to add an on click listener to {btnObj.name}");
                    throw;
                }
            }
        }
    }
}
