using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Menu
    {
        public class GameMenu
        {
            /// <summary>
            /// Disables the given game menu.
            /// </summary>
            /// <param name="targetMenu">The menu to disable.</param>
            public void Disable(MenuScreen targetMenu)
            {
                Init();
                Instance.RefreshReferences();

                Instance.DisableGameMenu(targetMenu);
            }

            /// <summary>
            /// Enables the given game menu.
            /// </summary>
            /// <param name="targetMenu">The menu to enable.</param>
            public void Enable(MenuScreen targetMenu)
            {
                Init();
                Instance.RefreshReferences();

                Instance.EnableGameMenu(targetMenu);
            }

            /// <summary>
            /// Checks if a given game menu has been exists.
            /// </summary>
            /// 
            /// <returns>
            /// A boolean of whether or not the given menu exists.
            /// </returns>
            public bool Exists(MenuScreen targetMenu)
            {
                Init();
                Instance.RefreshReferences();

                return Instance.GameMenuExists(targetMenu);
            }

            /// <summary>
            /// Checks whether any of the game menu screens are active.
            /// </summary>
            /// <returns>
            /// The active MenuScreen if there is one. If there are no active menu screens then it returns default.
            /// </returns>
            public MenuScreen GetActiveMenu()
            {
                Init();
                Instance.RefreshReferences();

                return Instance.GetActiveGameMenu();
            }
        }
    }
}
