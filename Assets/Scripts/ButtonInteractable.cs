using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private Animator _animator;
    [SerializeField] private GridSquare _gridSquare;
    [SerializeField] private bool _startsOccupied = true;

    public string Id => _id;

    private void Start()
    {
        Vector2Int pos = new Vector2Int((int)_gridSquare.transform.position.x, (int)_gridSquare.transform.position.z);
        GameController.Instance.gridManager.SetOccupied(pos, true);
    }

    public void OnInteract(bool interact)
    {
        Vector2Int pos = new Vector2Int((int)_gridSquare.transform.position.x, (int)_gridSquare.transform.position.z);
        GameController.Instance.gridManager.SetOccupied(pos, !interact);
        _animator.SetBool("Interact", interact);
    }
}
