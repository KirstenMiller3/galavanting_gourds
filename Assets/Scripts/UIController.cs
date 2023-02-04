using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject _hazardWarning;

    private void Start()
    {
        ShowHazardWarning(false);
    }

    public void ShowHazardWarning(bool hazardWarning)
    {
        _hazardWarning.SetActive(hazardWarning);
    }
}
