using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _rowSize = 5;

    public Transform[] Points => _points;

    private Transform[,] _grid;

    private Vector2Int _gridPos = new Vector2Int();

    private void Awake()  {
        _grid = new Transform[_rowSize, _rowSize];
        for(int i = 0; i  < _points.Length; i++) {
            int row = (int)(i / _rowSize);
            int col = (int)(i % _rowSize);
            _grid[row, col] = _points[i];
        }    
    }

    public void Move(Vector2Int movement)
    {
        Debug.Log($"Move: {movement}");

        _gridPos += movement;

        Debug.Log($"1: {_gridPos}");

        _gridPos.x = Mathf.Clamp(_gridPos.x, 0, _rowSize - 1);
        _gridPos.y = Mathf.Clamp(_gridPos.y, 0, _rowSize - 1);

        Debug.Log(_gridPos);
    }

    public Vector3 GetPosition()
    {
        Vector3 gridPos = _grid[_gridPos.x, _gridPos.y].position;
        return new Vector3(gridPos.x, 0f, gridPos.z);
    }
}
