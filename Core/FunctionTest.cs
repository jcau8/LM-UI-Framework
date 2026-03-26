using MelonLoader;
using UnityEngine;
using static LMUI.CoreShell;

[assembly: MelonInfo(typeof(LMUI.Core.FunctionTest), "LMD UI Lib", "1.0.0", "DevdudeX", null)]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace LMUI.Core
{
    public class FunctionTest : MelonMod
    {
        internal Logger _logger = new();
        AssetBundle _loadedAssetBundle = null;
        GameObject _testButtonPrefab;
        GameObject _buttonObject;
        bool _btnInstantiated;
        bool _sceneLoaded;


        public override void OnInitializeMelon()
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

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _logger.LogInfo("Refreshing references");
            Instance.RefreshReferences();
            _logger.LogInfo("References refreshed");

            if (!_sceneLoaded)
            {
                if (!_btnInstantiated)
                {
                    _logger.LogInfo("Instantiating button prefab into main menu");
                    _buttonObject = Instance.InstantiateButtonPrefabIntoGameMenu(_testButtonPrefab, CoreShell.MenuScreen.MainMenu, true);
                    if (_buttonObject != null)
                    {
                        _logger.LogInfo("testbutton prefab instantiated into main menu");
                        _btnInstantiated = true;
                    }
                }

                _logger.LogInfo("Applying game menu style to test button");
                Instance.ApplyGameMenuStyle(_buttonObject);
                _logger.LogInfo("Applied game menu style");

                _logger.LogInfo("Setting on click action");
                Instance.OnClickAction = OnTestButtonClick;
                _logger.LogInfo("On clik action set");

                _logger.LogInfo("Adding on click listener");
                Instance.AddOnClickListener(_buttonObject);
                _logger.LogInfo("Added on click listener");

                _logger.LogInfo("Scene loaded");
                _sceneLoaded = true;
            }

        }

        public void OnTestButtonClick()
        {
            _logger.LogInfo("Button clicked");
        }

        public override void OnDeinitializeMelon()
        {
            _logger.LogInfo("Unloading testbundle");
            Instance.Unload(_loadedAssetBundle);
            _logger.LogInfo("testbundle unloaded");
        }
    }
}