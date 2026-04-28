using UnityEngine;
using static LMUI.CoreShell;

namespace LMUI.API
{
    public class Button
    {
        internal GameObject _buttonObject;

        // Am I doing overloads correctly? Also is this the simplest way to do them?

        /// <summary>
        /// Instantiates a given prefab into a given game menu.
        /// </summary>
        /// <param name="buttonPrefab">The prefab to instantiate.</param>
        /// <param name="targetGameMenu">The menu to instantiate the prefab into.</param>
        /// <param name="onClickAction">The action to run when the button is clicked.</param>
        /// <param name="useLayout">Whether or not to use the layout from the given menu.</param>
        /// <param name="useGameMenuStyle">Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(GameObject buttonPrefab, MenuScreen targetGameMenu, Action onClickAction, bool useLayout, bool useGameMenuStyle)
        {
            Init();
            Instance.RefreshReferences();

            _buttonObject = Instance.InstantiateButtonPrefabIntoGameMenu(buttonPrefab, targetGameMenu, useLayout);
            if (useGameMenuStyle)
            {
                _buttonObject = Instance.ApplyGameMenuStyle(_buttonObject);
            }

            Instance.AddOnClickListener(_buttonObject, onClickAction);
            return _buttonObject;
        }

        /// <summary>
        /// Instantiates a given prefab into a given game menu.
        /// </summary>
        /// <param name="buttonPrefab">The prefab to instantiate.</param>
        /// <param name="targetGameMenu">The menu to instantiate the prefab into.</param>
        /// <param name="optionalText">Optional. Change the text of the button once it's instantiated. This is only for if you are resuing the same prefab.</param>
        /// <param name="onClickAction">The action to run when the button is clicked.</param>
        /// <param name="useLayout">Whether or not to use the layout from the given menu.</param>
        /// <param name="useGameMenuStyle">Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(GameObject buttonPrefab, MenuScreen targetGameMenu, string optionalText, Action onClickAction, bool useLayout, bool useGameMenuStyle)
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
        /// <param name="useGameMenuStyle">Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(GameObject buttonPrefab, GameObject customMenu, Action onClickAction, bool useGameMenuStyle)
        {
            Init();
            Instance.RefreshReferences();

            _buttonObject = Instance.InstantiateButtonPrefabIntoCustomMenu(buttonPrefab, customMenu);
            if (useGameMenuStyle)
            {
                _buttonObject = Instance.ApplyGameMenuStyle(_buttonObject);
            }

            Instance.AddOnClickListener(_buttonObject, onClickAction);
            return _buttonObject;
        }

        /// <summary>
        /// Instantiates a given prefab into a given custom menu.
        /// </summary>
        /// <param name="buttonPrefab">The prefab to instantiate.</param>
        /// <param name="customMenu">The menu GameObject to instantiate under. This can be a layout as well.</param>
        /// <param name="optionalText">Optional. Change the text of the button once it's instantiated. This only for if you reuse the same prefab.</param>
        /// <param name="onClickAction">The action to run when the button is clicked.</param>
        /// <param name="useGameMenuStyle">Whether or not to use the style of the game's buttons.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Create(GameObject buttonPrefab, GameObject customMenu, string optionalText, Action onClickAction, bool useGameMenuStyle)
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
