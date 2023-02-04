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
        public bool IsOccupied;
        public bool IsHazard;
    }

    [SerializeField] private GridSquare[] _points;
    [SerializeField] private int _rowSize = 5;

    private GridSquareData[,] _grid;

    private Vector2Int _gridPos = new Vector2Int();

    private void Awake()  {
        _grid = new GridSquareData[_rowSize, _rowSize];
        for(int i = 0; i  < _points.Length; i++) {
            int row = (int)(i / _rowSize);
            int col = (int)(i % _rowSize);
            _grid[row, col].Transform = _points[i].transform;
            _grid[row, col].IsOccupied = _points[i].IsHazard;
            _grid[row, col].IsHazard = _points[i].IsHazard;
        }

        _grid[0, 0].IsOccupied = true;
    }

    public bool Move(Vector2Int movement, bool isOccupied, out bool isHazard)
    {
        Vector2Int desiredPos = new Vector2Int();
        desiredPos = _gridPos;
        desiredPos += movement;

        desiredPos.x = Mathf.Clamp(desiredPos.x, 0, _rowSize - 1);
        desiredPos.y = Mathf.Clamp(desiredPos.y, 0, _rowSize - 1);

        isHazard = _grid[desiredPos.x, desiredPos.y].IsHazard;

        if (isHazard)
        {
            Debug.Log("HIT HAZARD");
            return false;
        }

        if (_grid[desiredPos.x, desiredPos.y].IsOccupied)
        {
            return false;
        }

        _gridPos = desiredPos;
        Debug.Log(_gridPos);
        _grid[_gridPos.x, _gridPos.y].IsOccupied = isOccupied;
        return true;
    }

    public void UndoMove(Vector2Int movement)
    {
        _grid[_gridPos.x, _gridPos.y].IsOccupied = false;

        _gridPos -= movement;

        _gridPos.x = Mathf.Clamp(_gridPos.x, 0, _rowSize - 1);
        _gridPos.y = Mathf.Clamp(_gridPos.y, 0, _rowSize - 1);
    }


    public Vector3 GetPosition()
    {
        Vector3 gridPos = _grid[_gridPos.x, _gridPos.y].Transform.position;
        return new Vector3(gridPos.x, 0f, gridPos.z);
    }
}
