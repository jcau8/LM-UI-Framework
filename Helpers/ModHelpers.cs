using LMUI.Core;
using static LMUI.CoreShell;

namespace LMUI.Helpers;

/// <summary>
/// Contains useful helper functions.
/// </summary>
internal static class ModHelpers
{
    /// <summary>
    /// Converts public MenuScreen enum to internal InternalMenuScreen enum.
    /// </summary>
    /// <returns>
    /// InternalMenuScreen enum.
    /// </returns>
    internal static ObjectManager.InternalMenuScreen ToInternal(MenuScreen targetMenu)
    {
        return targetMenu switch
        {
            MenuScreen.MainMenu => ObjectManager.InternalMenuScreen.MainMenu,
            MenuScreen.PauseMenu => ObjectManager.InternalMenuScreen.PauseMenu,
            MenuScreen.SettingsMenu => ObjectManager.InternalMenuScreen.SettingsMenu,
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Converts InternalMenuScreen enum to public MenuScreen enum.
    /// </summary>
    /// <returns>
    /// Public MenuScreen enum.
    /// </returns>
    internal static MenuScreen ToPublic(ObjectManager.InternalMenuScreen targetMenu)
    {
        return targetMenu switch
        {
            ObjectManager.InternalMenuScreen.MainMenu => MenuScreen.MainMenu,
            ObjectManager.InternalMenuScreen.PauseMenu => MenuScreen.PauseMenu,
            ObjectManager.InternalMenuScreen.SettingsMenu => MenuScreen.SettingsMenu,
            _ => throw new NotImplementedException()
        };
    }
}
