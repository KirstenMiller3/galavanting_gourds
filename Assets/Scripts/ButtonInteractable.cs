using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private Animator _animator;

    public string Id => _id;

    public void OnInteract(bool interact)
    {
        _animator.SetBool("Interact", interact);
    }
}
