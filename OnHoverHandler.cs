using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LMUI
{
    internal class OnHoverHandler : MonoBehaviour
    {
        private bool _pointerOver = false;
        private EventTrigger _trigger;
        private EventTrigger.Entry _pointerEnter;
        private EventTrigger.Entry _pointerExit;
        Color baseColour;

        // Using a property so it can be accessed from all functions
        // Doing this due to an annoying IL2CPP and Unity thing
        internal GameObject BtnObj { get; set; }

        private void SetTrigger()
        {
            // See if the button object already has the event trigger component
            // If not then add it, if it does the get it
            _trigger = BtnObj.GetComponent<EventTrigger>() ?? BtnObj.AddComponent<EventTrigger>();
        }

        private TextMeshProUGUI GetTMP()
        {
            return BtnObj.GetComponentInChildren<TextMeshProUGUI>();
        }

        internal void PointerEnterListener()
        {
            SetTrigger();
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
            _pointerOver = true;
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
            _pointerOver = false;
            // Get the TMP object and save the original colour
            TextMeshProUGUI _TMP = GetTMP();
            _TMP.color = baseColour;
        }
    }
}
