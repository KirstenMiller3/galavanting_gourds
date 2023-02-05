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
    [SerializeField] private TextMeshProUGUI _poisonedText;

    private void Start()
    {
        ShowHazardWarning(false);
        SetPlayButton(false);
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
        _hazardWarning.SetActive(hazardWarning);
    }

    public void UpdatePoisoned(int amount)
    {
        _poisonedText.gameObject.SetActive(amount > 0);
        if(amount == 0)
        {
            return;
        }

        _poisonedText.color = Color.white;
        _poisonedText.rectTransform.DORestart();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_poisonedText.rectTransform.DOShakeScale(0.5f, 0.5f, 10)).Insert(0f, _poisonedText.DOColor(Color.green, 1f));


        //_poisonedText.rectTransform.DOShakeScale(0.5f, 0.5f, 10);

        _poisonedText.text = $"Poisoned: {amount}";
    }
}
