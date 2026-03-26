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
        private FileHandler.AssetBundleHandler _assetBundleHandler;
        private FileHandler.AssetBundlesFolder _assetBundlesFolder;
        private static CoreShell _instance;

        CoreShell(
            ObjectReferenceManager objectReferenceManager,
            ObjectManager objectManager,
            ObjectManager.ButtonManager buttonManager,
            FileHandler.AssetBundleHandler assetBundleHandler,
            FileHandler.AssetBundlesFolder assetBundlesFolder
            )
        {
            _objectRefManager = objectReferenceManager;
            _objectManager = objectManager;
            _buttonManager = buttonManager;
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
                FileHandler.AssetBundleHandler assetBundleHandler = new(logger);
                FileHandler.AssetBundlesFolder assetBundlesFolder = new(logger);

                _instance = new
                    (
                    objectRefManager,
                    objectManager,
                    buttonManager,
                    assetBundleHandler,
                    assetBundlesFolder
                    );
            }
        }

        public void CheckForActiveInstance()
        {
            if (_instance == null)
            {
                throw new Exception("CoreShell not initialised. Call Init() first.");
            }
        }

        /// <summary>
        /// Allows the user to decide what they want to do when the button is clicked.
        /// </summary>
        public System.Action OnClickAction
        {
            get => _buttonManager.InternalOnClickAction;
            set
            {
                if (value != null)
                {
                    _buttonManager.InternalOnClickAction = value;
                }
            }
        }

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
            return _objectRefManager.FindGameObjectReference(referencePath);
        }

        /// <summary>
        /// Refreshes menu game object references. For use on scene load.
        /// </summary>
        public void RefreshReferences()
        {
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
            return _objectManager.LoadPrefabFromBundle(loadedAssetBundle, prefabPath);
        }

        /// <summary>
        /// Checks whether the given menu screen is active or not.
        /// </summary>
        /// <returns>
        /// A boolean representing the state of the given menu.
        /// </returns>
        public bool TargetMenuActive(MenuScreen targetMenu)
        {
            return _objectManager.TargetMenuActive(ToInternal(targetMenu));
        }

        /// <summary>
        /// Instantiate a prefab (which isn't a button) into a game menu.
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiatePrefabIntoMenuScreen(GameObject prefab, MenuScreen targetMenu)
        {
            return _objectManager.InstantiatePrefabIntoMenuScreen(prefab, ToInternal(targetMenu));
        }

        /// <summary>
        /// Deletes all children of a given GameObject.
        /// </summary>
        /// <returns>
        /// Given GameObject without children.
        /// </returns>
        public GameObject DeleteAllChildren(GameObject parentObject)
        {
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
            return _objectManager.DuplicateAndClearLayoutFromMenu(ToInternal(targetMenu), newLayoutName);
        }

        /// <summary>
        /// Takes the image and settings from a default game button and apply them to the given GameObject.
        /// </summary>
        /// <returns>
        /// Returns the given GameObject with the game buttons' image and settings applied.
        /// </returns>
        public GameObject StealGameButtonImage(GameObject buttonObject)
        {
            return _buttonManager.StealGameButtonImage(buttonObject);
        }

        /// <summary>
        /// Adds an OnHoverHandler script that emulates the game button's on hover appearance.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the OnHoverHandler script added.
        /// </returns>
        public GameObject ApplyHoverHandler(GameObject buttonObject)
        {
            return _buttonManager.ApplyHoverHandler(buttonObject);
        }

        /// <summary>
        /// Applies the game's hover effect and button image to a button GameObject.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the game's hover effect and button image.
        /// </returns>
        public GameObject ApplyGameMenuStyle(GameObject buttonObject)
        {
            return _buttonManager.ApplyGameMenuStyle(buttonObject);
        }

        /// <summary>
        /// Instantiate a prefab (which will be used as a button) into a game menu.
        /// </summary>
        /// <returns>
        /// The instantiated prefab as a GameObject.
        /// </returns>
        public GameObject InstantiateButtonPrefabIntoGameMenu(GameObject buttonPrefab, MenuScreen targetMenu,  bool useLayout)
        {
            return _buttonManager.InstantiateButtonPrefabIntoGameMenu(buttonPrefab, ToInternal(targetMenu), useLayout);
        }

        /// <summary>
        /// Adds an on click listener to the given button GameObject.
        /// </summary>
        /// <returns>
        /// The given button GameObject with the on click listener added.
        /// </returns>
        public GameObject AddOnClickListener(GameObject buttonObject)
        {
            return _buttonManager.AddOnClickListener(buttonObject);
        }

        /// <summary>
        /// Creates the default asset bundle folder if it doesn't already exist.
        /// </summary>
        public void CreateDefault()
        {
            _assetBundlesFolder.CreateDefault();
        }

        /// <summary>
        /// Creates a folder at the specified path.
        /// </summary>
        public void CreateCustom(string assetBundleFolderPath)
        {
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
            return _assetBundleHandler.LoadAssetBundle(assetBundleName, assetBundleFolderPath);
        }

        /// <summary>
        /// Collects all garbage then unloads the given bundle and all it's loaded objects. For use OnDeinitializeMelon.
        /// </summary>
        public void Unload(AssetBundle loadedAssetBundle)
        {
            // FIXME
            _assetBundleHandler.Unload(loadedAssetBundle);
        }
    }
}