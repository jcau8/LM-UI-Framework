using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(LMUI.FunctionTest), "LMD UI Framework", "1.0.0", "DevdudeX", null)]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace LMUI
{
    public class FunctionTest : MelonMod
    {
        private static Logger _logger;
        private static ObjectReferenceManager _objectRefManager;
        private static ObjectManager _objectManager;
        private static ObjectManager.LMUIButton _LMUIButton;
        private static FileHandler _fileHandler;
        private static FileHandler.AssetBundleHandler _assetBundleHandler;
        private static FileHandler.AssetBundlesFolder _assetBundlesFolder;

        AssetBundle _loadedAssetBundle = null;
        GameObject _testButtonPrefab;
        GameObject _buttonObject;
        bool _btnInstantiated;
        bool _sceneLoaded;


        public override void OnInitializeMelon()
        {
            // Using TONS of log entries so I know exactly where something fails
            _logger = new Logger(LoggerInstance);
            _logger.LogInfo("Initializing...");

            _objectRefManager = new ObjectReferenceManager(_logger);
            _objectManager = new ObjectManager(_logger, _objectRefManager);
            _LMUIButton = new ObjectManager.LMUIButton(_logger, _objectRefManager);
            _fileHandler = new FileHandler(_logger);
            _assetBundleHandler = new FileHandler.AssetBundleHandler(_logger);
            _assetBundlesFolder = new FileHandler.AssetBundlesFolder(_logger);
            _logger.LogInfo("Successfully initialised scripts");

            _logger.LogInfo("Creating AssetBundles folder");
            _assetBundlesFolder.CreateDefault();
            _logger.LogInfo("AssetBundles folder created in game root dir");

            _logger.LogInfo("Loading testbundle");
            _loadedAssetBundle = _assetBundleHandler.LoadAssetBundle("testbundle", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Lonely Mountains - Downhill\\AssetBundles\\");
            _logger.LogInfo("testbundle loaded");

            _logger.LogInfo("Loading testbutton prefab");
            _testButtonPrefab = _objectManager.LoadPrefabFromBundle(_loadedAssetBundle, "assets/prefabs/testbutton.prefab");
            _logger.LogInfo("Loaded testbutton prefab");

            _logger.LogInfo("Registering the OnHoverHandler");
            _LMUIButton.RegisterOnHoverHandler();
            _logger.LogInfo("OnHoverHandler registered");

            _btnInstantiated = false;
            _sceneLoaded = false;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _logger.LogInfo("Refreshing references");
            _objectRefManager.RefreshReferences();
            _logger.LogInfo("References refreshed");

            if (!_sceneLoaded)
            {
                if (!_btnInstantiated)
                {
                    _logger.LogInfo("Instantiating button prefab into main menu");
                    _buttonObject = _LMUIButton.InstantiateButtonPrefabIntoGameMenu(_testButtonPrefab, ObjectManager.MenuScreen.MainMenu, true);
                    if (_buttonObject != null)
                    {
                        _logger.LogInfo("testbutton prefab instantiated into main menu");
                        _btnInstantiated = true;
                    }
                }

                _logger.LogInfo("Applying game menu style to test button");
                _LMUIButton.ApplyGameMenuStyle(_buttonObject);
                _logger.LogInfo("Applied game menu style");

                _logger.LogInfo("Setting on click action");
                _LMUIButton.OnClickAction = OnTestButtonClick;
                _logger.LogInfo("On clik action set");

                _logger.LogInfo("Adding on click listener");
                _LMUIButton.AddOnClickListener(_buttonObject);
                _logger.LogInfo("Added on click listener");

                _logger.LogInfo("Scene loaded");
                _sceneLoaded = true;
            }

        }

        public static void OnTestButtonClick()
        {
            _logger.LogInfo("Button clicked");
        }

        public override void OnUpdate()
        {
            //void
        }

        public override void OnDeinitializeMelon()
        {
            _logger.LogInfo("Unloading testbundle");
            _assetBundleHandler.Unload(_loadedAssetBundle);
            _logger.LogInfo("testbundle unloaded");
        }
    }
}