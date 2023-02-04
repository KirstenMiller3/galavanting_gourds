using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject _hazardWarning;
    [SerializeField] private Button[] _buttons;

    private void Start()
    {
        ShowHazardWarning(false);
    }

    private void Update()
    {
        SetNoPlayerControls(!GameController.Instance.PauseControls);
    }

    public void SetNoPlayerControls(bool playerControlsActive)
    {
        for(int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = playerControlsActive;
        }
    }

    public void ShowHazardWarning(bool hazardWarning)
    {
        _hazardWarning.SetActive(hazardWarning);
    }
}
