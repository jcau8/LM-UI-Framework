using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace LMUI.Core
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
        internal enum InternalMenuScreen
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
        internal bool TargetMenuActive(InternalMenuScreen targetMenu)
        {
            // Using the layouts instead of the actual screens due to the order of which they are added.
            // Once the layout is added we know that the actual menu exists.
            switch (targetMenu)
            {
                case InternalMenuScreen.MainMenu:
                    return _objectReferenceManager.MainMenuScreenLayout != null;

                case InternalMenuScreen.PauseMenu:
                    return _objectReferenceManager.PauseScreenLayout != null;

                case InternalMenuScreen.SettingsMenu:
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
        internal GameObject InstantiatePrefabIntoMenuScreen(GameObject prefab, InternalMenuScreen targetMenu)
        {
            // There is probably a better way to handle when the menu isn't active
            try
            {
                if (TargetMenuActive(targetMenu))
                {
                    switch (targetMenu)
                    {
                        case InternalMenuScreen.MainMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreen.transform);

                        case InternalMenuScreen.PauseMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreen.transform);

                        case InternalMenuScreen.SettingsMenu:
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
                List<GameObject> children = [];
                GameObject parent = parentObject;

                foreach (Transform child in parent.transform)
                {
                    children.Add(child.gameObject);
                }
                children.ForEach(child => GameObject.Destroy(child));

                return parent;
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
        internal GameObject DuplicateAndClearLayoutFromMenu(InternalMenuScreen targetMenu, string newLayoutName)
        {
            try
            {
                GameObject newLayout;
                switch (targetMenu)
                {
                    case InternalMenuScreen.MainMenu:
                        newLayout = GameObject.Instantiate(_objectReferenceManager.MainMenuScreenLayout, _objectReferenceManager.MainMenuScreenLayout.transform);
                        newLayout.name = newLayoutName;
                        newLayout = DeleteAllChildren(newLayout);
                        return newLayout;

                    case InternalMenuScreen.PauseMenu:
                        newLayout = GameObject.Instantiate(_objectReferenceManager.PauseScreenLayout, _objectReferenceManager.PauseScreenLayout.transform);
                        newLayout.name = newLayoutName;
                        newLayout = DeleteAllChildren(newLayout);
                        return newLayout;

                    case InternalMenuScreen.SettingsMenu:
                        newLayout = GameObject.Instantiate(_objectReferenceManager.SettingsScreenLayout, _objectReferenceManager.SettingsScreenLayout.transform);
                        newLayout.name = newLayoutName;
                        newLayout = DeleteAllChildren(newLayout);
                        return newLayout;

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
        internal class ButtonManager
        {
            readonly Logger _logger;
            readonly ObjectReferenceManager _objectReferenceManager;
            internal ButtonManager(Logger logger, ObjectReferenceManager objectReferenceManager)
            {
                _logger = logger;
                _objectReferenceManager = objectReferenceManager;
            }

            private System.Action _onClickAction;
            /// <summary>
            /// Allows the user to decide what they want to do when the button is clicked
            /// </summary>
            internal System.Action InternalOnClickAction
            {
                get => _onClickAction;
                set => _onClickAction = value;
            }

            /// <summary>
            /// Takes the image and settings from a default game button and apply them to the given GameObject
            /// </summary>
            /// <returns>
            /// Returns the given GameObject with the game buttons' image and settings applied
            /// </returns>
            internal GameObject StealGameButtonImage(GameObject buttonObject)
            {
                try
                {
                    GameObject baseButtonObject = buttonObject;
                    GameObject mainMenuOptionsBtn = _objectReferenceManager.MainMenuOptionsButton;

                    UnityEngine.UI.Image sourceImage = mainMenuOptionsBtn.GetComponent<UnityEngine.UI.Image>();
                    UnityEngine.UI.Image btnImage = baseButtonObject.GetComponent<UnityEngine.UI.Image>();

                    btnImage.sprite = sourceImage.sprite;
                    btnImage.color = sourceImage.color;
                    btnImage.material = sourceImage.material;
                    btnImage.type = sourceImage.type;
                    btnImage.preserveAspect = sourceImage.preserveAspect;

                    return baseButtonObject;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to steal the game buttons' image and apply it to: {buttonObject.name}");
                    throw;
                }
            }

            // I plan to make this work for all custom components
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
            internal GameObject ApplyHoverHandler(GameObject buttonObject)
            {
                try
                {
                    RegisterOnHoverHandler();
                    GameObject baseButtonObject = buttonObject;
                    OnHoverHandler hoverHandler = baseButtonObject.AddComponent<OnHoverHandler>();
                    hoverHandler.Init();
                    return baseButtonObject;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to apply OnHoverHandler script to {buttonObject.name}");
                    throw;
                }
            }

            /// <summary>
            /// Applies the game's hover effect and button image to a button GameObject
            /// </summary>
            /// <returns>
            /// The given button GameObject with the game's hover effect and button image.
            /// </returns>
            internal GameObject ApplyGameMenuStyle(GameObject buttonObject)
            {
                try
                {
                    GameObject styledButtonObject = buttonObject;
                    StealGameButtonImage(styledButtonObject);
                    ApplyHoverHandler(styledButtonObject);
                    return styledButtonObject;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to apply the game menu style to: {buttonObject.name}");
                    throw;
                }
            }

            /// <summary>
            /// Instantiate a prefab (which will be used as a button) into a game menu
            /// </summary>
            /// <returns>
            /// The instatiated prefab as a GameObject
            /// </returns>
            internal GameObject InstantiateButtonPrefabIntoGameMenu(GameObject buttonPrefab, InternalMenuScreen targetMenu, bool useLayout)
            {
                try
                {
                    switch (targetMenu)
                    {
                        case InternalMenuScreen.MainMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.MainMenuScreenLayout.transform);
                            }
                            return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.MainMenuScreen.transform);

                        case InternalMenuScreen.PauseMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.PauseScreenLayout.transform);
                            }
                            return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.PauseScreen.transform);

                        case InternalMenuScreen.SettingsMenu:
                            if (useLayout)
                            {
                                return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.SettingsScreenLayout.transform);
                            }
                            return GameObject.Instantiate(buttonPrefab, _objectReferenceManager.SettingsScreen.transform);

                        default:
                            _logger.LogError($"{targetMenu} isn't a valid menu.");
                            return null;
                    }
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to instantiate {buttonPrefab.name} into {targetMenu}");
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
            internal GameObject AddOnClickListener(GameObject buttonObject)
            {
                try
                {
                    GameObject buttonObjectWithListener = buttonObject;
                    buttonObjectWithListener.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)OnBtnClick);
                    return buttonObjectWithListener;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to add an on click listener to {buttonObject.name}");
                    throw;
                }
            }
        }
    }
}
