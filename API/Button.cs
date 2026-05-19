using UnityEngine;
using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Button
    {
        internal GameObject _buttonObject;

        /// <summary>
        /// Instantiates a given prefab into a given game menu.
        /// </summary>
        /// <param name="buttonPrefab">The prefab to instantiate.</param>
        /// <param name="targetGameMenu">The menu to instantiate the prefab into.</param>
        /// <param name="onClickAction">The action to run when the button is clicked.</param>
        /// <param name="optionalText">(Optional) Change the text of the button once it's instantiated.</param>
        /// <param name="useLayout">(Optional) Whether or not to use the layout from the given menu.</param>
        /// <param name="useGameMenuStyle">(Optional) Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(
            GameObject buttonPrefab,
            MenuScreen targetGameMenu,
            Action onClickAction,
            string optionalText = null,
            bool useLayout = false,
            bool useGameMenuStyle = false
            )
        {
            Init();
            Instance.RefreshReferences();

            _buttonObject = Instance.InstantiateButtonPrefabIntoGameMenu(buttonPrefab, targetGameMenu, useLayout);
            if (useGameMenuStyle)
            {
                _buttonObject = Instance.ApplyGameMenuStyle(_buttonObject);
            }

            if (optionalText != null)
            {
                Instance.SetButtonText(_buttonObject, optionalText);
            }

            Instance.AddOnClickListener(_buttonObject, onClickAction);
            return _buttonObject;
        }

        /// <summary>
        /// Instantiates a given prefab into a given custom menu.
        /// </summary>
        /// <param name="buttonPrefab">The prefab to instantiate.</param>
        /// <param name="customMenu">The menu GameObject to instantiate under. This can be a layout as well.</param>
        /// <param name="onClickAction">The action to run when the button is clicked.</param>
        /// <param name="optionalText">(Optional) Change the text of the button once it's instantiated.</param>
        /// <param name="useGameMenuStyle">(Optional) Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(
            GameObject buttonPrefab,
            GameObject customMenu,
            Action onClickAction,
            string optionalText = null,
            bool useGameMenuStyle = false
            )
        {
            Init();
            Instance.RefreshReferences();

            _buttonObject = Instance.InstantiateButtonPrefabIntoCustomMenu(buttonPrefab, customMenu);
            if (useGameMenuStyle)
            {
                _buttonObject = Instance.ApplyGameMenuStyle(_buttonObject);
            }
            
            if (optionalText != null)
            {
                Instance.SetButtonText(_buttonObject, optionalText);
            }

            Instance.AddOnClickListener(_buttonObject, onClickAction);
            return _buttonObject;
        }
    }
}
