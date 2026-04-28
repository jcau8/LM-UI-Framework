using Il2CppInterop.Runtime.Injection;
using Il2CppTMPro;
using UnityEngine;
using static LMUI.CoreShell;
using static MelonLoader.MelonLogger;

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
        /// Instantiate a prefab (which isn't a button) into a game menu.
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject.
        /// </returns>
        internal GameObject InstantiatePrefabIntoGameMenu(GameObject prefab, InternalMenuScreen targetGameMenu)
        {
            // There is probably a better way to handle when the menu isn't active
            try
            {
                if (TargetMenuActive(targetGameMenu))
                {
                    switch (targetGameMenu)
                    {
                        case InternalMenuScreen.MainMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.MainMenuScreen.transform);

                        case InternalMenuScreen.PauseMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.PauseScreen.transform);

                        case InternalMenuScreen.SettingsMenu:
                            return GameObject.Instantiate(prefab, _objectReferenceManager.SettingsScreen.transform);

                        default:
                            _logger.LogError($"{targetGameMenu} isn't a valid menu.");
                            return null;
                    }
                }
                _logger.LogInfo($"{targetGameMenu} isn't active. Returning null...");
                return null;
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to instantiate prefab: {prefab.name} into menu: {targetGameMenu}");
                throw;
            }
        }

        /// <summary>
        /// Instantiates a prefab (which isn't a button) into a custom menu.
        /// </summary>
        /// <returns>
        /// The instatiated prefab as a GameObject.
        /// </returns>
        internal GameObject InstantiatePrefabIntoCustomMenu(GameObject prefab, GameObject menu)
        {
            try
            {
                return GameObject.Instantiate(prefab, menu.transform);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to instantiate {prefab.name} into {menu.name}");
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
                GameObject parent = parentObject;

                for (int i = parent.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = parent.transform.GetChild(i);
                    GameObject.Destroy(child.gameObject);
                }

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
                        newLayout = GameObject.Instantiate(_objectReferenceManager.MainMenuScreenLayout, _objectReferenceManager.LMDUIWrapper.transform);
                        newLayout.name = newLayoutName;
                        newLayout = DeleteAllChildren(newLayout);
                        return newLayout;

                    case InternalMenuScreen.PauseMenu:
                        newLayout = GameObject.Instantiate(_objectReferenceManager.PauseScreenLayout, _objectReferenceManager.LMDUIWrapper.transform);
                        newLayout.name = newLayoutName;
                        newLayout = DeleteAllChildren(newLayout);
                        return newLayout;

                    case InternalMenuScreen.SettingsMenu:
                        newLayout = GameObject.Instantiate(_objectReferenceManager.SettingsScreenLayout, _objectReferenceManager.LMDUIWrapper.transform);
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
        /// Duplicates the layout GameObject from a menu and removes all children
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed
        /// </returns>
        internal GameObject DuplicateAndClearLayoutFromMenu(GameObject targetLayout, string newLayoutName)
        {
            try
            {
                GameObject newLayout;
                newLayout = GameObject.Instantiate(targetLayout, _objectReferenceManager.LMDUIWrapper.transform);
                newLayout.name = newLayoutName;
                newLayout = DeleteAllChildren(newLayout);
                return newLayout;
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to duplicate and clear the layout from {targetLayout.name}");
                throw;
            }
        }

        /// <summary>
        /// Steals the game's language setting image and applies it to the given GameObject
        /// </summary>
        /// <returns>
        /// The given GameObject with the language setting's image applied
        /// </returns>
        internal GameObject StealGameSettingImage(GameObject baseObject)
        {
            try
            {
                GameObject targetObject = baseObject;
                GameObject languageSetting = _objectReferenceManager.SettingsEnumLanguage;

                UnityEngine.UI.Image sourceImage = languageSetting.GetComponent<UnityEngine.UI.Image>();
                UnityEngine.UI.Image objectImage = targetObject.GetComponent<UnityEngine.UI.Image>();

                objectImage.sprite = sourceImage.sprite;
                objectImage.color = sourceImage.color;
                objectImage.material = sourceImage.material;
                objectImage.type = sourceImage.type;
                objectImage.preserveAspect = sourceImage.preserveAspect;

                return targetObject;
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to steal the language setting's image and apply it to: {baseObject.name}");
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

            /// <summary>
            /// Takes the image and settings from a default game button and apply them to the given GameObject
            /// </summary>
            /// <returns>
            /// Returns the given GameObject with the game button's image and settings applied
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
                    _logger.LogError($"Failed to steal the game button's image and apply it to: {buttonObject.name}");
                    throw;
                }
            }

            /// <summary>
            /// Takes the TMP formatting from a default game button and applies them to the given GameObject.
            /// </summary>
            /// <returns>
            /// The given GameObject with the game button's text formatting.
            /// </returns>
            internal GameObject StealGameButtonTextFormatting(GameObject buttonObject)
            {
                try
                {
                    GameObject baseButtonObject = buttonObject;
                    GameObject mainMenuOptionsBtn = _objectReferenceManager.MainMenuOptionsButtonElements;

                    TextMeshProUGUI sourceTMP = mainMenuOptionsBtn.GetComponent<TextMeshProUGUI>();
                    if (sourceTMP == null)
                    {
                        _logger.LogInfo("Failed to get the TMP component from the main menu options button. Returning null...");
                        return null;
                    }
                    TextMeshProUGUI buttonTMP = baseButtonObject.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonTMP == null)
                    {
                        _logger.LogInfo("Failed to get the TMP component from the given button. Returning null...");
                        return null;
                    }

                    buttonTMP.color = sourceTMP.color;
                    buttonTMP.fontMaterials = sourceTMP.fontMaterials;
                    buttonTMP.font = sourceTMP.font;
                    buttonTMP.fontSize = sourceTMP.fontSize;
                    buttonTMP.fontWeight = sourceTMP.fontWeight;
                    buttonTMP.fontStyle = sourceTMP.fontStyle;

                    return baseButtonObject;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to steal the game button's text formatting and apply it to: {buttonObject.name}");
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
            /// Applies the game's hover effect, button image and text formatting to a button GameObject
            /// </summary>
            /// <returns>
            /// The given button GameObject with the game's hover effect, button image and text formatting.
            /// </returns>
            internal GameObject ApplyGameMenuStyle(GameObject buttonObject)
            {
                try
                {
                    GameObject styledButtonObject = buttonObject;
                    StealGameButtonImage(styledButtonObject);
                    StealGameButtonTextFormatting(styledButtonObject);
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
            internal GameObject InstantiateButtonPrefabIntoGameMenu(GameObject buttonPrefab, InternalMenuScreen targetGameMenu, bool useLayout)
            {
                try
                {
                    switch (targetGameMenu)
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
                            _logger.LogError($"{targetGameMenu} isn't a valid menu.");
                            return null;
                    }
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to instantiate {buttonPrefab.name} into {targetGameMenu}");
                    throw;
                }
            }

            /// <summary>
            /// Instantiate a prefab (which will be used as a button) into a custom menu.
            /// </summary>
            /// <returns>
            /// The instatiated prefab as a GameObject.
            /// </returns>
            internal GameObject InstantiateButtonPrefabIntoCustomMenu(GameObject buttonPrefab, GameObject menu)
            {
                try
                {
                    return GameObject.Instantiate(buttonPrefab, menu.transform);
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to instantiate {buttonPrefab.name} into {menu.name}");
                    throw;
                }
            }

            /// <summary>
            /// Sets the text of an existing button.
            /// </summary>
            internal void SetButtonText(GameObject buttonObject, string text)
            {
                try
                {
                    // VERY IMPORTANT!
                    // Has to be a GameObject with a child TMP component, cannot be a prefab which has a different structure.
                    buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to set the text of {buttonObject.name} to: {text} \nDoes {buttonObject.name} have a child TMP component?");
                    throw;
                }
            }

            /// <summary>
            /// Adds an on click listener to the given button GameObject
            /// </summary>
            /// <returns>
            /// The given button GameObject with the on click listener added
            /// </returns>
            internal GameObject AddOnClickListener(GameObject buttonObject, System.Action onClickAction)
            {
                try
                {
                    GameObject buttonObjectWithListener = buttonObject;
                    buttonObjectWithListener.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)onClickAction);
                    return buttonObjectWithListener;
                }
                catch (Exception)
                {
                    _logger.LogError($"Failed to add an on click listener to {buttonObject.name}");
                    throw;
                }
            }
        }

        internal class MenuManager
        {
            readonly Logger _logger;
            readonly ObjectReferenceManager _objectRefManager;

            internal MenuManager(Logger logger, ObjectReferenceManager objectReferenceManager)
            {
                _logger = logger;
                _objectRefManager = objectReferenceManager;
            }

            /// <summary>
            /// Checks if a given game menu has been exists.
            /// </summary>
            /// <returns>
            /// A boolean of whether or not the given menu exists.
            /// </returns>
            internal bool GameMenuExists(InternalMenuScreen targetMenu)
            {
                switch (targetMenu)
                {
                    case InternalMenuScreen.MainMenu:
                        return _objectRefManager.MainMenuScreen != null;

                    case InternalMenuScreen.PauseMenu:
                        return _objectRefManager.PauseScreen != null;

                    case InternalMenuScreen.SettingsMenu:
                        return _objectRefManager.SettingsScreen != null;

                    default:
                        _logger.LogInfo($"{targetMenu} is not a valid game menu.");
                        return false;
                }
            }

            /// <summary>
            /// Enables the given game menu.
            /// </summary>
            internal void EnableGameMenu(InternalMenuScreen targetMenu)
            {
                switch (targetMenu)
                {
                    case InternalMenuScreen.MainMenu:
                        _objectRefManager.MainMenuScreen.SetActive(true);
                        return;

                    case InternalMenuScreen.PauseMenu:
                        _objectRefManager.PauseScreen.SetActive(true);
                        return;

                    case InternalMenuScreen.SettingsMenu:
                        _objectRefManager.SettingsScreen.SetActive(true);
                        return;

                    default:
                        _logger.LogInfo($"{targetMenu} is not a valid game menu.");
                        return;
                }
            }

            /// <summary>
            /// Disables the given game menu.
            /// </summary>
            internal void DisableGameMenu(InternalMenuScreen targetMenu)
            {
                switch (targetMenu)
                {
                    case InternalMenuScreen.MainMenu:
                        _objectRefManager.MainMenuScreen.SetActive(false);
                        return;

                    case InternalMenuScreen.PauseMenu:
                        _objectRefManager.PauseScreen.SetActive(false);
                        return;

                    case InternalMenuScreen.SettingsMenu:
                        _objectRefManager.SettingsScreen.SetActive(false);
                        return;

                    default:
                        _logger.LogInfo($"{targetMenu} is not a valid game menu.");
                        return;
                }
            }

            /// <summary>
            /// Checks whether any of the game menu screens are active.
            /// </summary>
            /// <returns>
            /// The active MenuScreen if there is one. If there are no active menu screens then it returns default.
            /// </returns>
            internal InternalMenuScreen GetActiveGameMenu()
            {
                try
                {
                    if (_objectRefManager.MainMenuScreen.activeInHierarchy)
                    {
                        return InternalMenuScreen.MainMenu;
                    }
                    else if (_objectRefManager.PauseScreen.activeInHierarchy)
                    {
                        return InternalMenuScreen.PauseMenu;
                    }
                    else if (_objectRefManager.SettingsScreen.activeInHierarchy)
                    {
                        return InternalMenuScreen.SettingsMenu;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to find an active game menu. Do the menus even exist yet?");
                    throw;
                }
            }
        }
    }
}