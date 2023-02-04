using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OogrootAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector3 _baseScale;
    private float yPos;
    private float rotSpeed = 1f;

    private void Start()
    {
        _baseScale = transform.localScale;
    }


    [ContextMenu("StartJump")]
    public void StartJump()
    {
        _animator.SetBool("Jump", true);
        _animator.SetBool("Walk", false);
    }

    [ContextMenu("EndJump")]
    public void EndJump()
    {
        _animator.SetBool("Jump", false);
    }

    [ContextMenu("StartRun")]
    public void StartRun()
    {
        _animator.SetBool("Walk", true);
        _animator.SetBool("Jump", false);
    }

    [ContextMenu("EndRun")]
    public void EndRun()
    {
        _animator.SetBool("Walk", false);
    }

    [ContextMenu("StartDeath")]
    public void StartDeath()
    {
        _animator.SetTrigger("Death");
    }
}
