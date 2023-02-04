using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GridManager : MonoBehaviour
{
    public struct GridSquare
    {
        public Transform Transform;
        public bool IsOccupied;
    }

    [SerializeField] private Transform[] _points;
    [SerializeField] private int _rowSize = 5;

    private GridSquare[,] _grid;

    private Vector2Int _gridPos = new Vector2Int();

    private void Awake()  {
        _grid = new GridSquare[_rowSize, _rowSize];
        for(int i = 0; i  < _points.Length; i++) {
            int row = (int)(i / _rowSize);
            int col = (int)(i % _rowSize);
            _grid[row, col].Transform = _points[i];
        }

        _grid[0, 0].IsOccupied = true;
    }

    public bool Move(Vector2Int movement, bool isOccupied)
    {
        Vector2Int desiredPos = new Vector2Int();
        desiredPos = _gridPos;
        desiredPos += movement;

        desiredPos.x = Mathf.Clamp(desiredPos.x, 0, _rowSize - 1);
        desiredPos.y = Mathf.Clamp(desiredPos.y, 0, _rowSize - 1);

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
