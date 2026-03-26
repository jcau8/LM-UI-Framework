using UnityEngine;

namespace LMUI.Core;

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

    private const string MAIN_MENU_OPTIONS_BUTTON_PATH = MAIN_MENU_SCREEN_LAYOUT_PATH + "ListButton_Options";

    // Set constant for the hover text colour
    // Currently not used
    internal static Color32 LMD_HOVER_TEXT_COLOUR = new(145, 190, 15, 255);

    // Set properties
    internal GameObject MainMenuScreen { get; private set; }
    internal GameObject PauseScreen { get; private set; }
    internal GameObject SettingsScreen { get; private set; }

    internal GameObject MainMenuScreenLayout { get; private set; }
    internal GameObject PauseScreenLayout { get; private set; }
    internal GameObject SettingsScreenLayout { get; private set; }

    internal GameObject MainMenuOptionsButton { get; private set; }

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
        MainMenuScreen = FindGameObjectReference(MAIN_MENU_SCREEN_PATH);
        PauseScreen = FindGameObjectReference(PAUSE_SCREEN_PATH);
        SettingsScreen = FindGameObjectReference(SETTINGS_SCREEN_PATH);

        MainMenuScreenLayout = FindGameObjectReference(MAIN_MENU_SCREEN_LAYOUT_PATH);
        PauseScreenLayout = FindGameObjectReference(PAUSE_SCREEN_LAYOUT_PATH);
        SettingsScreenLayout = FindGameObjectReference(SETTINGS_SCREEN_LAYOUT_PATH);

        MainMenuOptionsButton = FindGameObjectReference(MAIN_MENU_OPTIONS_BUTTON_PATH);
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
