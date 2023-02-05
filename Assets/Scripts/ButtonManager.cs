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

    private string _buttonId;
    public void ActivateButton(string buttonId)
    {
        _buttonId = buttonId;
        _buttonEventsDictionary[_buttonId].IsActive = !_buttonEventsDictionary[_buttonId].IsActive;
        Debug.Log($"IS ACTIVE? {_buttonEventsDictionary[_buttonId].IsActive}");

        if (_buttonEventsDictionary[_buttonId].IsActive)
        {
            Invoke("InvokeActivate", 0.7f);
        }
        else
        {
            _buttonEventsDictionary[_buttonId].ButtonInteractable.OnInteract(_buttonEventsDictionary[_buttonId].IsActive);
        }
    }

    private void InvokeActivate()
    {
        _buttonEventsDictionary[_buttonId].ButtonInteractable.OnInteract(_buttonEventsDictionary[_buttonId].IsActive);
    }
}
