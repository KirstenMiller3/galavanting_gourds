using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public partial class GridManager : MonoBehaviour
{
    public struct GridSquareData
    {
        public Transform Transform;
        public Vector3 Position;
        public GridType GridType;
        public bool IsButton => !string.IsNullOrEmpty(ButtonId);
        public string ButtonId;
    }

    [SerializeField] private GridSquare[] _points;
    [SerializeField] private int _rowSize = 5;

    private GridSquareData[,] _grid;

    public GridSquareData GridOrigin => _grid[0,0];

    private Vector2Int _gridPos = new Vector2Int();

    private void Awake()  {
        _grid = new GridSquareData[_rowSize, _rowSize];
        for(int i = 0; i  < _points.Length; i++) {
            int row = (int)(i / _rowSize);
            int col = (int)(i % _rowSize);
            _grid[row, col].Transform = _points[i].transform;
            //_grid[row, col].IsOccupied = _points[i].IsHazard;
            _grid[row, col].GridType = _points[i].GridType;
            _grid[row, col].ButtonId = _points[i].ButtonId;
        }

        _grid[0, 0].GridType = GridType.Occupied;
    }

    public bool Move(Vector2Int movement, out bool isHazard, out string buttonId)
    {
        buttonId = string.Empty;
        Vector2Int desiredPos = new Vector2Int();
        desiredPos = _gridPos;
        desiredPos += movement;

        desiredPos.x = Mathf.Clamp(desiredPos.x, 0, _rowSize - 1);
        desiredPos.y = Mathf.Clamp(desiredPos.y, 0, _rowSize - 1);

        isHazard = _grid[desiredPos.x, desiredPos.y].GridType == GridType.Hazard;

        if (_grid[desiredPos.x, desiredPos.y].GridType == GridType.Gap)
        {
            Vector2Int desiredPos2 = new Vector2Int();
            desiredPos2 = desiredPos;
            desiredPos2 += movement;

            desiredPos2.x = Mathf.Clamp(desiredPos2.x, 0, _rowSize - 1);
            desiredPos2.y = Mathf.Clamp(desiredPos2.y, 0, _rowSize - 1);

            isHazard = _grid[desiredPos2.x, desiredPos2.y].GridType == GridType.Hazard;
            buttonId = _grid[desiredPos2.x, desiredPos2.y].ButtonId;

            if (_grid[desiredPos2.x, desiredPos2.y].GridType == GridType.Occupied || _grid[desiredPos2.x, desiredPos2.y].GridType == GridType.Gap)
            {
                return false;
            }


            _gridPos = desiredPos2;
            Debug.Log(_gridPos);
            _grid[_gridPos.x, _gridPos.y].GridType = GridType.Occupied;
            return true;
        }

        buttonId = _grid[desiredPos.x, desiredPos.y].ButtonId;

        if (_grid[desiredPos.x, desiredPos.y].GridType == GridType.Occupied)
        {
            return false;
        }

        _gridPos = desiredPos;
        Debug.Log(_gridPos);
        _grid[_gridPos.x, _gridPos.y].GridType = GridType.Occupied;
        return true;
    }

    public void UndoMove(Vector2Int movement)
    {
        _grid[_gridPos.x, _gridPos.y].GridType = GridType.None;


        if (_grid[_gridPos.x, _gridPos.y].IsButton)
        {
            ButtonManager.Instance.ActivateButton(_grid[_gridPos.x, _gridPos.y].ButtonId);
        }

        _gridPos -= movement;

        _gridPos.x = Mathf.Clamp(_gridPos.x, 0, _rowSize - 1);
        _gridPos.y = Mathf.Clamp(_gridPos.y, 0, _rowSize - 1);

        if(_grid[_gridPos.x, _gridPos.y].GridType == GridType.Gap)
        {
            UndoMove( movement);
        }
    }


    public Vector3 GetPosition()
    {
        Vector3 gridPos = _grid[_gridPos.x, _gridPos.y].Transform.position;
        return new Vector3(gridPos.x, 0f, gridPos.z);
    }
}
