using UnityEngine;
using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Layout
    {
        /// <summary>
        /// Duplicates the layout GameObject from one of the game's menus, removes all children and instantiates it under the UI wrapper.
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject Create(MenuScreen targetMenu, string newLayoutName)
        {
            Init();
            Instance.RefreshReferences();

            return Instance.DuplicateAndClearLayoutFromMenu(targetMenu, newLayoutName);
        }
        /// <summary>
        /// Duplicates the layout GameObject from one of the game's menus, removes all children and instantiates it under the UI wrapper.
        /// </summary>
        /// <returns>
        /// A menu layout GameObject with children removed.
        /// </returns>
        public GameObject Create(GameObject targetLayout, string newLayoutName)
        {
            Init();
            Instance.RefreshReferences();

            return Instance.DuplicateAndClearLayoutFromMenu(targetLayout, newLayoutName);
        }
    }
}
