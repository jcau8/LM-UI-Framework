//using MelonLoader;
//using UnityEngine;

//[assembly: MelonInfo(typeof(LMUI.ModCore), "LMD UI Framework", "1.0.0", "DevdudeX", null)]
//[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

//namespace LMUI
//{
//	public class ModCore : MelonMod
//	{
//		public const string MOD_VERSION = "1.0.0";
//		private static Logger _logger;
//		private static ObjectReferenceManager _objectRefManager;
//		private static ObjectManager _objectManager;
//		private static FileHandler _fileHandler;
//		private static FileHandler.AssetBundleHandler _assetBundleHandler;
//		private static FileHandler.AssetBundlesFolder _assetBundlesFolder;

//		bool _mainMenuWasLoaded = false;
//		bool _menuHasBeenSetUp = false;
//		float _loadTimer = 0f;

//		public override void OnInitializeMelon()
//		{
//			_logger = new Logger(LoggerInstance);
//			_logger.LogInfo("Initializing...");

//			_objectRefManager = new ObjectReferenceManager(_logger);
//			_objectManager = new ObjectManager(_logger);
//			_fileHandler = new FileHandler(_logger);
//			_assetBundleHandler = new FileHandler.AssetBundleHandler(_logger);
//			_assetBundlesFolder = new FileHandler.AssetBundlesFolder(_logger);
//			_logger.LogInfo("Successfully initialised scripts");
//		}

//		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
//		{
//			string[] whitelistedLoadScenes = ["gameplay"];
//			if (Array.IndexOf(whitelistedLoadScenes, sceneName) != -1)
//			{
//				LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
//				LoggerInstance.Msg("Refreshing references...");
//				_objectRefManager.RefreshReferences();
//				_mainMenuWasLoaded = true;
//			}
//		}

//		public override void OnUpdate()
//		{
//		}
//	}
//}