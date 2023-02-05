using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject _hazardWarning;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _poisonedObj;
    [SerializeField] private TextMeshProUGUI _poisonedText;

    private void Start()
    {
        //ShowHazardWarning(false);
        SetPlayButton(false);
        UpdatePoisoned(0);
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

    public void SetPlayButton(bool isActive)
    {
        _playButton.interactable = isActive;
    }

    public void ShowHazardWarning(bool hazardWarning)
    {
       // _hazardWarning.SetActive(hazardWarning);
    }

    public void UpdatePoisoned(int amount)
    {
        _poisonedObj.gameObject.SetActive(amount > 0);
        if (amount == 0)
        {
            return;
        }

        _poisonedObj.transform.DORestart();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_poisonedObj.transform.DOShakeScale(0.5f, 0.5f, 10));

        _poisonedText.text = $"Poisoned: {amount}";
    }
}
