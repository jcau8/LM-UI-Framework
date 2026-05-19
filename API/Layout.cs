using UnityEngine;
using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Layout
    {
        /// <summary>
        /// Duplicates the layout GameObject from one of the game's menus, removes all children and instantiates it under the UI wrapper.
        /// </summary>
        /// <param name="targetMenu">The menu to duplicate the layout from.</param>
        /// <param name="newLayoutName">What the new layout should be called.</param>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject CreateDuplicate(MenuScreen targetMenu, string newLayoutName)
        {
            Init();
            Instance.RefreshReferences();

            return Instance.DuplicateAndClearLayoutFromMenu(targetMenu, newLayoutName);
        }
        /// <summary>
        /// Duplicates the layout GameObject from one of the game's menus, removes all children and instantiates it under the UI wrapper.
        /// </summary>
        /// <param name="targetLayout">The layout to create a duplicate of.</param>
        /// <param name="newLayoutName">What the new layout should be called.</param>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject CreateDuplicate(GameObject targetLayout, string newLayoutName)
        {
            Init();
            Instance.RefreshReferences();

            return Instance.DuplicateAndClearLayoutFromMenu(targetLayout, newLayoutName);
        }
    }
}
