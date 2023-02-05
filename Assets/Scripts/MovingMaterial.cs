using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMaterial : MonoBehaviour
{
    [SerializeField] private Material _material;

    private void Start()
    {
        _material.mainTextureOffset = Vector2.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.SetLoops(-1).Append(_material.DOOffset(Vector2.up * 10f, 50f).SetEase(Ease.Linear));
    }
}
