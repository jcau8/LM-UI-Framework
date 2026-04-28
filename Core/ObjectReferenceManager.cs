using UnityEngine;

namespace LMUI.Core;

public class ObjectReferenceManager
{
    readonly Logger _logger;
    internal ObjectReferenceManager(Logger logger)
    {
        _logger = logger;
    }

    // Set constant for the game's UI wrapper path
    private const string LMDUI_WRAPPER_PATH = "UI(Clone)/Canvas3D/Wrapper/";

    // Set constants for menu paths
    private const string MAIN_MENU_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/";
    private const string PAUSE_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/PauseScreen(Clone)/";
    private const string SETTINGS_SCREEN_PATH = "UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)/";

    // Set constants for menu layout paths
    private const string MAIN_MENU_SCREEN_LAYOUT_PATH = MAIN_MENU_SCREEN_PATH + "MainMenuScreen/Layout_ButtonList/";
    private const string PAUSE_SCREEN_LAYOUT_PATH = PAUSE_SCREEN_PATH + "PauseScreen_Group/Layout/";
    private const string SETTINGS_SCREEN_LAYOUT_PATH = SETTINGS_SCREEN_PATH + "SettingsMenu/Layout_Categories/";

    private const string MAIN_MENU_OPTIONS_BUTTON_PATH = MAIN_MENU_SCREEN_LAYOUT_PATH + "ListButton_Options";
    private const string MAIN_MENU_OPTIONS_BUTTON_ELEMENTS_PATH = MAIN_MENU_OPTIONS_BUTTON_PATH + "/Elements";

    private const string SETTINGS_ENUM_LAYOUT_PATH = SETTINGS_SCREEN_PATH + "SettingsMenu/Settings/Layout_Settings(Clone)/";
    private const string SETTINGS_ENUM_LANGUAGE_PATH = SETTINGS_SCREEN_PATH + "SettingsMenu/Settings/Layout_Settings(Clone)/SettingsEnum_Language";

    // Set constant for the hover text colour
    // Currently not used
    internal static Color32 LMD_HOVER_TEXT_COLOUR = new(145, 190, 15, 255);

    // Set properties
    internal GameObject LMDUIWrapper {  get; private set; }

    internal GameObject MainMenuScreen { get; private set; }
    internal GameObject PauseScreen { get; private set; }
    internal GameObject SettingsScreen { get; private set; }

    internal GameObject MainMenuScreenLayout { get; private set; }
    internal GameObject PauseScreenLayout { get; private set; }
    internal GameObject SettingsScreenLayout { get; private set; }

    internal GameObject MainMenuOptionsButton { get; private set; }
    internal GameObject MainMenuOptionsButtonElements { get; private set; }

    internal GameObject SettingsEnumLayout { get; private set; }
    internal GameObject SettingsEnumLanguage { get; private set; }

    /// <summary>
    /// Find the given GameObject reference
    /// </summary>
    /// <returns>
    /// The GameObject found at that reference
    /// </returns>
    internal GameObject FindGameObjectReference(string referencePath)
    {
        try
        {
            return GameObject.Find(referencePath);
        }
        catch (Exception)
        {
            _logger.LogError($"Failed to find a GameObject at {referencePath}");
            throw;
        }
    }

    /// <summary>
    /// Assigns GameObjects to the properites
    /// </summary>
    private void AssignGameObjects()
    {
        LMDUIWrapper = FindGameObjectReference(LMDUI_WRAPPER_PATH);

        MainMenuScreen = FindGameObjectReference(MAIN_MENU_SCREEN_PATH);
        PauseScreen = FindGameObjectReference(PAUSE_SCREEN_PATH);
        SettingsScreen = FindGameObjectReference(SETTINGS_SCREEN_PATH);

        MainMenuScreenLayout = FindGameObjectReference(MAIN_MENU_SCREEN_LAYOUT_PATH);
        PauseScreenLayout = FindGameObjectReference(PAUSE_SCREEN_LAYOUT_PATH);
        SettingsScreenLayout = FindGameObjectReference(SETTINGS_SCREEN_LAYOUT_PATH);

        MainMenuOptionsButton = FindGameObjectReference(MAIN_MENU_OPTIONS_BUTTON_PATH);
        MainMenuOptionsButtonElements = FindGameObjectReference(MAIN_MENU_OPTIONS_BUTTON_ELEMENTS_PATH);

        SettingsEnumLayout = FindGameObjectReference(SETTINGS_ENUM_LAYOUT_PATH);
        SettingsEnumLanguage = FindGameObjectReference(SETTINGS_ENUM_LANGUAGE_PATH);
    }

    // Clear function for use on scene load
    /// <summary>
    /// Assigns/reassigns GameObjects to the menu screen properties. For use on scene load.
    /// </summary>
    public void RefreshReferences()
    {
        try
        {
            AssignGameObjects();
        }
        catch (Exception)
        {
            _logger.LogError("Failed to assign game objects, the game objects may not exist in this scene.");
            throw;
        }
    }
}
