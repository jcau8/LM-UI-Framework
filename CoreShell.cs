using LMUI.Core;
using UnityEngine;
using static LMUI.Helpers.ModHelpers;

namespace LMUI
{
    // Serves as a wrapper for LMUI and the lowest level of user accessible code.
    public class CoreShell
    {
        private ObjectReferenceManager _objectRefManager;
        private ObjectManager _objectManager;
        private ObjectManager.ButtonManager _buttonManager;
        private ObjectManager.MenuManager _menuManager;
        private ObjectManager.LayoutManager _layoutManager;
        private FileHandler.AssetBundleHandler _assetBundleHandler;
        private FileHandler.AssetBundlesFolder _assetBundlesFolder;
        private static CoreShell _instance;

        CoreShell(
            ObjectReferenceManager objectReferenceManager,
            ObjectManager objectManager,
            ObjectManager.ButtonManager buttonManager,
            ObjectManager.MenuManager menuManager,
            ObjectManager.LayoutManager layoutManager,
            FileHandler.AssetBundleHandler assetBundleHandler,
            FileHandler.AssetBundlesFolder assetBundlesFolder
            )
        {
            _objectRefManager = objectReferenceManager;
            _objectManager = objectManager;
            _buttonManager = buttonManager;
            _menuManager = menuManager;
            _layoutManager = layoutManager;
            _assetBundleHandler = assetBundleHandler;
            _assetBundlesFolder = assetBundlesFolder;
        }

        public static CoreShell Instance => _instance;

        public static void Init()
        {
            if (_instance == null)
            {
                Logger logger = new();
                ObjectReferenceManager objectRefManager = new(logger);
                ObjectManager objectManager = new(logger, objectRefManager);
                ObjectManager.ButtonManager buttonManager = new(logger, objectRefManager);
                ObjectManager.MenuManager menuManager = new(logger, objectRefManager);
                ObjectManager.LayoutManager layoutManager = new(logger, objectRefManager, objectManager);
                FileHandler.AssetBundleHandler assetBundleHandler = new(logger);
                FileHandler.AssetBundlesFolder assetBundlesFolder = new(logger);

                _instance = new
                    (
                    objectRefManager,
                    objectManager,
                    buttonManager,
                    menuManager,
                    layoutManager,
                    assetBundleHandler,
                    assetBundlesFolder
                    );
            }
        }

        public static void CheckForActiveInstance()
        {
            if (_instance == null)
            {
                throw new Exception("CoreShell not initialised. Call Init() first.");
            }
        }

        // Game menu properties
        public GameObject LMDUIWrapper => _objectRefManager.LMDUIWrapper;

        public GameObject MainMenuScreen => _objectRefManager.MainMenuScreen;
        public GameObject PauseScreen => _objectRefManager.PauseScreen;
        public GameObject SettingsScreen => _objectRefManager.SettingsScreen;

        public GameObject MainMenuScreenLayout => _objectRefManager.MainMenuScreenLayout;
        public GameObject PauseScreenLayout => _objectRefManager.PauseScreenLayout;
        public GameObject SettingsScreenLayout => _objectRefManager.SettingsScreenLayout;

        public GameObject MainMenuOptionsButton => _objectRefManager.MainMenuOptionsButton;
        public GameObject MainMenuOptionsButtonElements => _objectRefManager.MainMenuOptionsButtonElements;

        public GameObject SettingsEnumLayout => _objectRefManager.SettingsEnumLayout;
        public GameObject SettingsEnumLanguage => _objectRefManager.SettingsEnumLanguage;

        /// <summary>
        /// Holds all possible menu screen types.
        /// </summary>
        public enum MenuScreen
        {
            MainMenu,
            PauseMenu,
            SettingsMenu
        }

        /// <summary>
        /// Find the given GameObject reference.
        /// </summary>
        /// <returns>
        /// The GameObject found at that reference.
        /// </returns>
        public GameObject FindGameObjectReference(string referencePath)
        {
            CheckForActiveInstance();
            return _objectRefManager.FindGameObjectReference(referencePath);
        }

        /// <summary>
        /// Refreshes menu game object references. For use on scene load.
        /// </summary>
        public void RefreshReferences()
        {
            CheckForActiveInstance();
            _objectRefManager.RefreshReferences();
        }

        /// <summary>
        /// Take a Prefab out of an AssetBundle.
        /// </summary>
        /// <returns>
        /// The prefab as a GameObject.
        /// </returns>
        public GameObject LoadPrefabFromBundle(AssetBundle loadedAssetBundle, string prefabPath)
        {
            CheckForActiveInstance();
            return _objectManager.LoadPrefabFromBundle(loadedAssetBundle, prefabPath);
        }

        /// <summary>
        /// Instantiate a prefab (which isn't a button) into a game menu.
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiatePrefabIntoGameMenu(GameObject prefab, MenuScreen targetMenu)
        {
            CheckForActiveInstance();
            return _objectManager.InstantiatePrefabIntoGameMenu(prefab, ToInternal(targetMenu));
        }

        /// <summary>
        /// Instantiates a prefab (which isn't a button) into a custom menu.
        /// </summary>
        /// <returns>
        /// The instatiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiatePrefabIntoCustomMenu(GameObject prefab, GameObject menu)
        {
            return _objectManager.InstantiatePrefabIntoCustomMenu(prefab, menu);
        }

        /// <summary>
        /// Deletes all children of a given GameObject.
        /// </summary>
        /// <returns>
        /// Given GameObject without children.
        /// </returns>
        public GameObject DeleteAllChildren(GameObject parentObject)
        {
            CheckForActiveInstance();
            return _objectManager.DeleteAllChildren(parentObject);
        }

        /// <summary>
        /// Duplicates the layout GameObject from a menu and removes all children.
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject DuplicateAndClearLayoutFromMenu(MenuScreen targetMenu, string newLayoutName)
        {
            CheckForActiveInstance();
            return _layoutManager.DuplicateAndClearLayoutFromMenu(ToInternal(targetMenu), newLayoutName);
        }
        /// <summary>
        /// Duplicates the layout GameObject from a menu and removes all children.
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject DuplicateAndClearLayoutFromMenu(GameObject targetLayout, string newLayoutName)
        {
            CheckForActiveInstance();
            return _layoutManager.DuplicateAndClearLayoutFromMenu(targetLayout, newLayoutName);
        }

        /// <summary>
        /// Steals the game's language setting image and applies it to the given GameObject
        /// </summary>
        /// <returns>
        /// The given GameObject with the language setting's image applied
        /// </returns>
        public GameObject StealGameSettingImage(GameObject baseObject)
        {
            CheckForActiveInstance();
            return _objectManager.StealGameSettingImage(baseObject);
        }

        /// <summary>
        /// Takes the image and settings from a default game button and apply them to the given GameObject.
        /// </summary>
        /// <returns>
        /// Returns the given GameObject with the game buttons' image and settings applied.
        /// </returns>
        public GameObject StealGameButtonImage(GameObject buttonObject)
        {
            CheckForActiveInstance();
            return _buttonManager.StealGameButtonImage(buttonObject);
        }

        /// <summary>
        /// Takes the TMP formatting from a default game button and applies them to the given GameObject.
        /// </summary>
        /// <returns>
        /// The given GameObject with the game button's text formatting.
        /// </returns>
        public GameObject StealGameButtonTextFormatting(GameObject buttonObject)
        {
            CheckForActiveInstance();
            return _buttonManager.StealGameButtonTextFormatting(buttonObject);
        }

        /// <summary>
        /// Adds an OnHoverHandler script that emulates the game button's on hover appearance.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the OnHoverHandler script added.
        /// </returns>
        public GameObject ApplyHoverHandler(GameObject buttonObject)
        {
            CheckForActiveInstance();
            return _buttonManager.ApplyHoverHandler(buttonObject);
        }

        /// <summary>
        /// Applies the game's hover effect, button image and text formatting to a button GameObject.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the game's hover effect, button image and texxt formatting.
        /// </returns>
        public GameObject ApplyGameMenuStyle(GameObject buttonObject)
        {
            CheckForActiveInstance();
            return _buttonManager.ApplyGameMenuStyle(buttonObject);
        }

        /// <summary>
        /// Instantiate a prefab (which will be used as a button) into a game menu.
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiateButtonPrefabIntoGameMenu(GameObject buttonPrefab, MenuScreen targetGameMenu,  bool useLayout)
        {
            CheckForActiveInstance();
            return _buttonManager.InstantiateButtonPrefabIntoGameMenu(buttonPrefab, ToInternal(targetGameMenu), useLayout);
        }

        /// <summary>
        /// Instantiate a prefab (which will be used as a button) into a custom menu.
        /// </summary>
        /// <returns>
        /// The instatiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiateButtonPrefabIntoCustomMenu(GameObject buttonPrefab, GameObject menu)
        {
            CheckForActiveInstance();
            return _buttonManager.InstantiateButtonPrefabIntoCustomMenu(buttonPrefab, menu);
        }

        /// <summary>
        /// Sets the text of an existing button.
        /// </summary>
        public void SetButtonText(GameObject buttonObject, string text)
        {
            CheckForActiveInstance();
            _buttonManager.SetButtonText(buttonObject, text);
        }

        /// <summary>
        /// Adds an on click listener to the given button GameObject.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the on click listener added.
        /// </returns>
        public GameObject AddOnClickListener(GameObject buttonObject, System.Action onClickAction)
        {
            CheckForActiveInstance();
            return _buttonManager.AddOnClickListener(buttonObject, onClickAction);
        }

        /// <summary>
        /// Checks if a given game menu has been exists.
        /// </summary>
        /// <returns>
        /// A boolean of whether or not the given menu exists.
        /// </returns>
        public bool GameMenuExists(MenuScreen targetMenu)
        {
            CheckForActiveInstance();
            return _menuManager.GameMenuExists(ToInternal(targetMenu));
        }

        /// <summary>
        /// Enables the given game menu.
        /// </summary>
        public void EnableGameMenu(MenuScreen targetMenu)
        {
            CheckForActiveInstance();
            _menuManager.EnableGameMenu(ToInternal(targetMenu));
        }

        /// <summary>
        /// Disables the given game menu.
        /// </summary>
        public void DisableGameMenu(MenuScreen targetMenu)
        {
            CheckForActiveInstance();
            _menuManager.DisableGameMenu(ToInternal(targetMenu));
        }

        /// <summary>
        /// Checks whether any of the game menu screens are active.
        /// </summary>
        /// <returns>
        /// The active MenuScreen if there is one. If there are no active menu screens then it returns default.
        /// </returns>
        public MenuScreen GetActiveGameMenu()
        {
            CheckForActiveInstance();
            return ToPublic(_menuManager.GetActiveGameMenu());
        }

        /// <summary>
        /// Creates the default asset bundle folder if it doesn't already exist.
        /// </summary>
        public void CreateDefaultAssetBundleFolder()
        {
            CheckForActiveInstance();
            _assetBundlesFolder.CreateDefault();
        }

        /// <summary>
        /// Creates a folder at the specified path.
        /// </summary>
        public void CreateCustomAssetBundleFolder(string assetBundleFolderPath)
        {
            CheckForActiveInstance();
            _assetBundlesFolder.CreateCustom(assetBundleFolderPath);
        }

        /// <summary>
        /// Loads an asset bundle.
        /// </summary>
        /// <returns>
        /// The loaded asset bundle.
        /// </returns>
        public AssetBundle LoadAssetBundle(string assetBundleName, string assetBundleFolderPath)
        {
            CheckForActiveInstance();
            return _assetBundleHandler.LoadAssetBundle(assetBundleName, assetBundleFolderPath);
        }

        /// <summary>
        /// Collects all garbage then unloads the given bundle and all it's loaded objects. For use OnDeinitializeMelon.
        /// </summary>
        public void Unload(AssetBundle loadedAssetBundle)
        {
            CheckForActiveInstance();
            // FIXME
            _assetBundleHandler.Unload(loadedAssetBundle);
        }
    }
}