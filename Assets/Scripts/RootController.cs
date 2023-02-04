using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public struct BodySection
    {
        public GameObject body;
        public Vector2Int movement;
    }

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _bodySection;

    private Stack<BodySection> _body = new Stack<BodySection>();

    public void Update()
    {
        InputUpdate();
    }

    private void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
    }

    private void Move(Vector2Int movement)
    {
        if(_gridManager.Move(movement, true))
        {
            SpawnBodySection(movement);
            Vector3 newPos = _gridManager.GetPosition();
            transform.position = newPos;
        }
    }

    private void SpawnBodySection(Vector2Int movement)
    {
        GameObject _section = Instantiate(_bodySection, transform.position, Quaternion.identity);
        _body.Push(new BodySection() { body = _section, movement = movement });
    }

    private void Undo()
    {
        if (_body.Count == 0)
        {
            return;
        }

        GameObject partToUndo = _body.Peek().body;
        Destroy(partToUndo);

        _gridManager.UndoMove(_body.Peek().movement);
        transform.position = _gridManager.GetPosition();

        _body.Pop();
    }
}
