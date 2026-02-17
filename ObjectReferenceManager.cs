using Il2CppMegagon.Downhill.UI;
using Il2CppMegagon.Downhill.UI.Animations;
using Il2CppMegagon.Downhill.UI.Screens;
using Il2CppMegagon.Downhill.UI.Screens.Helper;
using Il2CppTMPro;
using LMUI.Core;
using MelonLoader;
using UnityEngine;

namespace LMUI;

public class ObjectReferenceManager
{
    readonly Logger _logger;
	internal ObjectReferenceManager(Logger logger)
	{
		_logger = logger;
	}

	// Set constants for menu paths
	private const string MAIN_MENU_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/";
	private const string PAUSE_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/PauseScreen(Clone)/PauseScreen_Group/";
	private const string SETTINGS_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)/SettingsMenu/";

	// Set constants for menu layout paths
	private const string MAIN_MENU_SCREEN_LAYOUT_PATH = MAIN_MENU_SCREEN_PATH + "Layout_ButtonList/";
	private const string PAUSE_SCREEN_LAYOUT_PATH = PAUSE_SCREEN_PATH + "Layout/";
	private const string SETTINGS_SCREEN_LAYOUT_PATH = SETTINGS_SCREEN_PATH + "Layout_Categories/";

	// Member fields to use as backing stores
	private GameObject _mainMenuScreen;
	private GameObject _settingsScreen;
	private GameObject _pauseScreen;

	private GameObject _mainMenuScreenLayout;
	private GameObject _pauseScreenLayout;
	private GameObject _settingsScreenLayout;

	// Find the GameObject reference
	public GameObject FindGameObjectReference(string referencePath)
	{
		return GameObject.Find(referencePath);
	}

	private void AssignGameObjects()
	{
		_mainMenuScreen = FindGameObjectReference(MAIN_MENU_SCREEN_PATH);
		_pauseScreen = FindGameObjectReference(PAUSE_SCREEN_PATH);
		_settingsScreen = FindGameObjectReference(SETTINGS_SCREEN_PATH);

		_mainMenuScreenLayout = FindGameObjectReference(MAIN_MENU_SCREEN_LAYOUT_PATH);
		_pauseScreenLayout = FindGameObjectReference(PAUSE_SCREEN_LAYOUT_PATH);
		_settingsScreenLayout = FindGameObjectReference(SETTINGS_SCREEN_LAYOUT_PATH);

	}

	// Clear function for use on scene load
	public void RefreshReferences()
	{
		AssignGameObjects();
	}

	// Set properties
	public GameObject MainMenuScreen
    {
        get => _mainMenuScreen;
        private set => _mainMenuScreen = value;
    }
	public GameObject PauseScreen
	{
		get => _pauseScreen;
		private set => _pauseScreen = value;
	}
    public GameObject SettingsScreen
    {
        get => _settingsScreen;
        private set => _settingsScreen = value;
    }

	public GameObject MainMenuScreenLayout
	{
		get => _mainMenuScreenLayout;
		private set => _mainMenuScreenLayout = value;
	}
	public GameObject PauseScreenLayout
	{
		get => _pauseScreenLayout;
		private set => _pauseScreenLayout = value;
	}
	public GameObject SettingsScreenLayout
	{
		get => _settingsScreenLayout;
		private set => _settingsScreenLayout = value;
	}
}
