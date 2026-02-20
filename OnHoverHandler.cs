using UnityEngine;
using UnityEngine.EventSystems;

namespace LMUI
{
    internal class OnHoverHandler : MonoBehaviour
    {
        private bool _pointerOver = false;
        private EventTrigger _trigger;
        private EventTrigger.Entry _pointerEnter;

        internal void PointerEntered(GameObject btnObj)
        {
            // See if the button object already has the event trigger component
            // If not then add it, if it does the get it
            if (btnObj.GetComponent<EventTrigger>() == null)
            {
                _trigger = btnObj.AddComponent<EventTrigger>();
            }
            else
            {
                _trigger = btnObj.GetComponent<EventTrigger>();
            }
            // Create a new entry of type PointerEnter
            _pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };

            _pointerEnter.callback.AddListener(OnPointerEntered);
        }

        internal void OnPointerEntered()
        {
            _pointerOver = true;
        }
    }
}
