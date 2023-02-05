using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public partial class GridManager : MonoBehaviour
{
    public struct GridSquareData
    {
        public Vector3 TilePosition;
        public GridType GridType;
        public bool IsOccupied;
        public bool IsButton => !string.IsNullOrEmpty(ButtonId);
        public string ButtonId;
        public Vector3 Position => TilePosition + (Vector3.up * 0.5f);
    }

    [SerializeField] private int _rowSize = 5;
    [SerializeField] private int _colSize = 5;

    private GridSquareData[,] _grid;

    public GridSquareData GridOrigin => _grid[0,0];

    private Vector2Int _gridPos = new Vector2Int();

    private void Awake()  {
        GridSquare[] tiles = GetComponentsInChildren<GridSquare>();

        _grid = new GridSquareData[_rowSize, _colSize];

        //int count = 0;
        //for(int rows = 0; rows < _rowSize; rows++)
        //{
        //    for(int cols = 0; cols < _colSize; cols++)
        //    {
        //        _grid[rows, cols].TilePosition = new Vector3(rows, 0f, cols);
        //        _grid[rows, cols].GridType = tiles[count].GridType;
        //        _grid[rows, cols].IsOccupied = tiles[count].IsOccupied;
        //        _grid[rows, cols].ButtonId = tiles[count].ButtonId;

        //        count++;
        //    }
        //}

        //for(int i = 0; i  < _points.Count; i++) {
        //    int row = (int)(i / _rowSize);
        //    int col = (int)(i % _rowSize);
        //    _grid[row, col].TilePosition = new Vector3(row, 0f, col);
        //    _grid[row, col].GridType = _points[i].GridType;
        //    _grid[row, col].IsOccupied = _points[i].IsOccupied;
        //    _grid[row, col].ButtonId = _points[i].ButtonId;
        //}

        for (int i = 0; i < tiles.Length; i++)
        {
            int row = (int)tiles[i].transform.position.x;
            int col = (int)tiles[i].transform.position.z;
            _grid[row, col].TilePosition = new Vector3(row, 0f, col);
            _grid[row, col].GridType = tiles[i].GridType;
            _grid[row, col].IsOccupied = tiles[i].IsOccupied;
            _grid[row, col].ButtonId = tiles[i].ButtonId;
        }

        _grid[0, 0].IsOccupied = true;
    }

    public bool Move(Vector2Int movement, out GridType gridType, out string buttonId)
    {
        buttonId = string.Empty;
        Vector2Int desiredPos = new Vector2Int();
        desiredPos = _gridPos;
        desiredPos += movement;

        desiredPos.x = Mathf.Clamp(desiredPos.x, 0, _rowSize - 1);
        desiredPos.y = Mathf.Clamp(desiredPos.y, 0, _rowSize - 1);

        gridType = _grid[desiredPos.x, desiredPos.y].GridType;


        if (_grid[desiredPos.x, desiredPos.y].IsOccupied)
        {
            Debug.Log("OCCUPADO!");
            return false;
        }

        if (_grid[desiredPos.x, desiredPos.y].GridType == GridType.Gap)
        {
            Vector2Int desiredPos2 = new Vector2Int();
            desiredPos2 = desiredPos;
            desiredPos2 += movement;

            desiredPos2.x = Mathf.Clamp(desiredPos2.x, 0, _rowSize - 1);
            desiredPos2.y = Mathf.Clamp(desiredPos2.y, 0, _rowSize - 1);

            gridType = _grid[desiredPos2.x, desiredPos2.y].GridType;
            buttonId = _grid[desiredPos2.x, desiredPos2.y].ButtonId;

            if (_grid[desiredPos2.x, desiredPos2.y].IsOccupied || _grid[desiredPos2.x, desiredPos2.y].GridType == GridType.Gap)
            {
                return false;
            }


            _gridPos = desiredPos2;
            Debug.Log(_gridPos);
            _grid[_gridPos.x, _gridPos.y].IsOccupied = true;
            return true;
        }

        buttonId = _grid[desiredPos.x, desiredPos.y].ButtonId;

        _gridPos = desiredPos;
        Debug.Log(_gridPos);
        _grid[_gridPos.x, _gridPos.y].IsOccupied = true;
        return true;
    }

    public void UndoMove(Vector2Int movement)
    {
        _grid[_gridPos.x, _gridPos.y].IsOccupied = false;


        if (_grid[_gridPos.x, _gridPos.y].IsButton)
        {
            ButtonManager.Instance.ActivateButton(_grid[_gridPos.x, _gridPos.y].ButtonId);
        }

        if (_grid[_gridPos.x, _gridPos.y].GridType == GridType.Poison)
        {
            GameController.Instance.RemovePoisonDamage();
        }

        _gridPos -= movement;

        _gridPos.x = Mathf.Clamp(_gridPos.x, 0, _rowSize - 1);
        _gridPos.y = Mathf.Clamp(_gridPos.y, 0, _rowSize - 1);

        if(_grid[_gridPos.x, _gridPos.y].GridType == GridType.Gap)
        {
            UndoMove( movement);
        }
    }

    public void SetOccupied(Vector2Int pos, bool occupied) {
        _grid[pos.x, pos.y].IsOccupied = occupied;
    }

    public Vector3 GetPosition()
    {
        Vector3 gridPos = _grid[_gridPos.x, _gridPos.y].TilePosition;
        return new Vector3(gridPos.x, 0f, gridPos.z);
    }
}
