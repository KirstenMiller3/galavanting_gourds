using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    public enum MoveType
    {
        YMove
    }

    [SerializeField] private string _id;
    [SerializeField] private GridSquare _gridSquare;
    [SerializeField] private bool _startsOccupied = true;
    [SerializeField] private bool _setsGapToTraversable = false;
    [SerializeField] private MoveType _moveType;
    [SerializeField] private float _movePos;

    public string Id => _id;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;


        if (!_setsGapToTraversable)
        {
            Vector2Int pos = new Vector2Int((int)_gridSquare.transform.position.x, (int)_gridSquare.transform.position.z);
            GameController.Instance.gridManager.SetOccupied(pos, true);
        }
   
    }

    public void OnInteract(bool interact)
    {
        Vector2Int pos = new Vector2Int((int)_gridSquare.transform.position.x, (int)_gridSquare.transform.position.z);

        if (_setsGapToTraversable)
        {
            if (interact)
            {
                GameController.Instance.gridManager.SetGridType(pos, GridType.None);
            }
            else
            {
                GameController.Instance.gridManager.SetGridType(pos, GridType.Gap);
            }
        }
        else {
            GameController.Instance.gridManager.SetOccupied(pos, !interact);
        }

        switch (_moveType)
        {
            case MoveType.YMove:
                YMove(interact);
                break;
            default:
                break;
        }
    }

    private void YMove(bool interact)
    {
        if (interact)
        {
            transform.DOMoveY(_movePos, 2f);
        }
        else
        {
            transform.DOMoveY(_startPos.y, 2f);
        }
    }

}
