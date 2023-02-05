using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActivateButtonBounce : MonoBehaviour
{
    private Button _button;
    private RectTransform _rect;

    private bool _changed;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _changed = _button.interactable;
        _button.onClick.AddListener(OnClick);
    }

    void Update()
    {
        if(_changed != _button.interactable)
        {
            _changed = _button.interactable;
            _rect.DOShakeScale(0.5f, 0.2f);
        }
    }

    private void OnClick()
    {
        _rect.DOShakeScale(0.5f, 0.2f);
    }

}
