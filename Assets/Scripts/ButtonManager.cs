using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractableObjectEvent : UnityEvent<ButtonInteractable> { }

[System.Serializable]
public class ButtonEvent
{
    public ButtonInteractable ButtonInteractable;
    public bool IsActive;
}

public class ButtonManager : Singleton<ButtonManager>
{
    [SerializeField] private ButtonEvent[] _buttonEvents;

    private Dictionary<string, ButtonEvent> _buttonEventsDictionary = new Dictionary<string, ButtonEvent>();

    protected override void Awake()
    {
        base.Awake();
        foreach(var bEvent in _buttonEvents) {
            _buttonEventsDictionary.Add(bEvent.ButtonInteractable.Id, bEvent);
        }
    }

    public void ActivateButton(string buttonId)
    {
        _buttonEventsDictionary[buttonId].IsActive = !_buttonEventsDictionary[buttonId].IsActive;
        Debug.Log($"IS ACTIVE? {_buttonEventsDictionary[buttonId].IsActive}");
        _buttonEventsDictionary[buttonId].ButtonInteractable.OnInteract(_buttonEventsDictionary[buttonId].IsActive);
    }
}
