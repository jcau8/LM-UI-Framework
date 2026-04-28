using MelonLoader;
using UnityEngine;
using static LMUI.CoreShell;
using LMUI.API;

[assembly: MelonInfo(typeof(LMUI.Core.FunctionTest), "LMD UI Lib", "1.0.0", "DevdudeX and jcau8", null)]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace LMUI.Core
{
    public class FunctionTest : MelonMod
    {
        internal Logger _logger = new();
        Button _button = new();
        LMUI.API.AssetBundle _assetBundle = new();
        Menu.GameMenu _gameMenu = new();
        Layout _layout = new();
        UnityEngine.AssetBundle _loadedAssetBundle = null;
        GameObject _testButtonPrefab;
        GameObject _buttonObject;
        bool _btnInstantiated;
        bool _sceneLoaded;
        // 0: Don't run anything - Always use this for release builds!
        // 1: Test functions at wrapper level
        // 2: Test functions at API level
        internal int _level = 0;


        public override void OnInitializeMelon()
        {
            if (_level == 1)
            {
                // Using TONS of log entries so I know exactly where something fails
                Init();
                _logger.LogInfo("Successfully initialised scripts");

                _logger.LogInfo("Creating AssetBundles folder");
                Instance.CreateDefault();
                _logger.LogInfo("AssetBundles folder created in game root dir");

                _logger.LogInfo("Loading testbundle");
                _loadedAssetBundle = Instance.LoadAssetBundle("testbundle", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Lonely Mountains - Downhill\\AssetBundles\\");
                _logger.LogInfo("testbundle loaded");

                _logger.LogInfo("Loading testbutton prefab");
                _testButtonPrefab = Instance.LoadPrefabFromBundle(_loadedAssetBundle, "assets/prefabs/testbutton.prefab");
                _logger.LogInfo("Loaded testbutton prefab");

                _btnInstantiated = false;
                _sceneLoaded = false;
            }
            else if (_level == 2)
            {
                Init();
                _logger.LogInfo("Successfully initialised scripts");

                _logger.LogInfo("Loading testbundle");
                _loadedAssetBundle = _assetBundle.Load("testbundle", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Lonely Mountains - Downhill\\AssetBundles\\");
                _logger.LogInfo("testbundle loaded");

                _logger.LogInfo("Loading testbutton prefab");
                _testButtonPrefab = Instance.LoadPrefabFromBundle(_loadedAssetBundle, "assets/prefabs/testbutton.prefab");
                _logger.LogInfo("Loaded testbutton prefab");

                _btnInstantiated = false;
                _sceneLoaded = false;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_level == 1)
            {
                _logger.LogInfo("Refreshing references");
                Instance.RefreshReferences();
                _logger.LogInfo("References refreshed");

                if (!_sceneLoaded)
                {
                    if (!_btnInstantiated)
                    {
                        _logger.LogInfo("Instantiating button prefab into main menu");
                        _buttonObject = Instance.InstantiateButtonPrefabIntoGameMenu(_testButtonPrefab, MenuScreen.MainMenu, true);
                        if (_buttonObject != null)
                        {
                            _logger.LogInfo("testbutton prefab instantiated into main menu");
                            _btnInstantiated = true;
                        }
                    }

                    _logger.LogInfo("Applying game menu style to test button");
                    Instance.ApplyGameMenuStyle(_buttonObject);
                    _logger.LogInfo("Applied game menu style");

                    _logger.LogInfo("Adding on click listener");
                    Instance.AddOnClickListener(_buttonObject, OnTestButtonClick);
                    _logger.LogInfo("Added on click listener");

                    _logger.LogInfo("Scene loaded");
                    _sceneLoaded = true;
                }
            }
            else if (_level == 2)
            {
                if (!_sceneLoaded)
                {
                    if (!_btnInstantiated)
                    {
                        _logger.LogInfo("Creating testbutton");
                        _buttonObject = _button.Create(_testButtonPrefab, MenuScreen.MainMenu, OnTestButtonClick, true, true);
                        if (_buttonObject != null)
                        {
                            _logger.LogInfo("testbutton created");
                            _btnInstantiated = true;
                        }
                    }
                    _sceneLoaded = true;

                }
            }
        }

        public void OnTestButtonClick()
        {
            _logger.LogInfo("Button clicked");
            _gameMenu.Disable(MenuScreen.MainMenu);
            GameObject custLayout = _layout.Create(MenuScreen.MainMenu, "CustomLayout");

            for (int i=0; i < 4; i++)
            {
                _button.Create(_testButtonPrefab, custLayout, $"Button {i + 1}", LogButtonClick, true);
            }
        }

        public void LogButtonClick()
        {
            _logger.LogInfo($"Button clicked");
        }

        public override void OnDeinitializeMelon()
        {
            if (_level == 1)
            {
                _logger.LogInfo("Unloading testbundle");
                Instance.Unload(_loadedAssetBundle);
                _logger.LogInfo("testbundle unloaded");
            }
            else if (_level == 2)
            {
                // void
            }
        }
    }
}