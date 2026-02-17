using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(LMUI.ModCore), "LMD UI Framework", "1.0.0", "DevdudeX", null)]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace LMUI
{
	public class ModCore : MelonMod
	{
		public const string MOD_VERSION = "1.0.0";
		private static Logger _logger;
		private static ObjectReferenceManager _objectRefManager;

		bool _mainMenuWasLoaded = false;
		bool _menuHasBeenSetUp = false;
		float _loadTimer = 0f;

		public override void OnInitializeMelon()
		{
			_logger = new Logger(LoggerInstance);
			_logger.LogInfo("Initializing.");

			_objectRefManager = new ObjectReferenceManager(_logger);
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			string[] whitelistedLoadScenes = ["gameplay"];
			if (Array.IndexOf(whitelistedLoadScenes, sceneName) != -1)
			{
				LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
				LoggerInstance.Msg("");
				_objectRefManager.AssignGameObjects();
				_mainMenuWasLoaded = true;
			}
		}

		public override void OnUpdate()
		{
		}
	}
}