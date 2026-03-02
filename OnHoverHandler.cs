using Il2CppTMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LMUI
{
    internal class OnHoverHandler : MonoBehaviour
    {
        private EventTrigger _trigger;
        private EventTrigger.Entry _pointerEnter;
        private EventTrigger.Entry _pointerExit;
        Color baseColour;

        // Using an external vairable so it can be accessed from all functions
        // Doing this due to an annoying IL2CPP and Unity thing
        // Probably a better way to do this though
        private GameObject _btnObj;

        private void SetBtnObj()
        {
            _btnObj = gameObject;
        }

        private void SetTrigger()
        {
            // See if the button object already has the event trigger component
            // If not then add it, if it does the get it
            _trigger = _btnObj.GetComponent<EventTrigger>() ?? _btnObj.AddComponent<EventTrigger>();
        }

        private TextMeshProUGUI GetTMP()
        {
            return _btnObj.GetComponentInChildren<TextMeshProUGUI>();
        }

        internal void PointerEnterListener()
        {
            SetTrigger();
            SetBtnObj();
            // Create a new entry of type PointerEnter
            _pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            // This one line took so long to make it work, I only got it working thanks to (mainly) @the_lenny and @atmudia
            _pointerEnter.callback.AddListener(new System.Action<BaseEventData>(OnPointerEntered));
            // Add the listener to the trigger
            _trigger.triggers.Add(_pointerEnter);
        }

        internal void OnPointerEntered(BaseEventData data)
        {
            // Get the TMP object and save the original colour
            TextMeshProUGUI _TMP = GetTMP();
            baseColour = _TMP.color;
            // Set the text colour to the green that all LMD buttons use (haven't implemented LMSR support yet)
            // Need to somehow get this from ObjectReferenceManager.cs for LMSR support but idk how to rn
            _TMP.color = new Color32(145, 190, 15, 255);
        }

        internal void PointerExitListener()
        {
            SetTrigger();
            SetBtnObj();
            // Create a new entry of type PointerEnter
            _pointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            // This one line took so long to make it work, I only got it working thanks to (mainly) @the_lenny and @atmudia
            _pointerExit.callback.AddListener(new System.Action<BaseEventData>(OnPointerEntered));
            // Add the listener to the trigger
            _trigger.triggers.Add(_pointerExit);
        }

        internal void OnPointerExited(BaseEventData data)
        {
            // Get the TMP object and save the original colour
            TextMeshProUGUI _TMP = GetTMP();
            _TMP.color = baseColour;
        }
    }
}
