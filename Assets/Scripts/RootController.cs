using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public struct BodySection
    {
        public GameObject body;
        public GameObject root;
        public Vector2Int movement;
        public Vector3 position;
    }

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ProceduralIvy _rootMaker;
    [SerializeField] private GameObject _bodySection;
    [SerializeField] private ParticleSystem _hitFx;

    private Stack<BodySection> _body = new Stack<BodySection>();

    public Stack<BodySection> Route => _body;

    private string rootSoundName;

    public void Update()
    {
        if (GameController.Instance.PauseControls)
        {
            return;
        }


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
        int num = UnityEngine.Random.Range(1, 4);
        AudioManager.instance.Play($"vine_{num}");
        rootSoundName = $"vine_{num}";
        Invoke("ShakeOnDelay_Move", 0.7f);
        bool canMove = _gridManager.Move(movement, out GridType gridType, out string buttonId);

        if (canMove)
        {
            Vector3 newPos = _gridManager.GetPosition();

            SpawnBodySection(movement, newPos);
            transform.position = newPos;
        }

        if(gridType == GridType.Hazard) {
            Invoke("ShakeOnDelay_Hazzard", 0.7f);
            GameController.Instance.SetState(GameController.GameState.Hazard);
        }
        else if(gridType == GridType.Poison)
        {
            GameController.Instance.AddPoisonDamage();
        }
        else if(gridType == GridType.End)
        {
            GameController.Instance.SetState(GameController.GameState.Success);
        }

        if (gridType == GridType.Button)
        {
            ButtonManager.Instance.ActivateButton(buttonId);
        }
    }

    private void ShakeOnDelay_Move()
    {
        if(GameController.Instance.State == GameController.GameState.Pikmining 
            || GameController.Instance.State == GameController.GameState.Success)
        {
            AudioManager.instance.Stop(rootSoundName);
            return;
        }
        ScreenShake.Instance.Shake(0.1f, 0.02f);
        AudioManager.instance.Stop(rootSoundName);

    }

    private void ShakeOnDelay_Hazzard()
    {
        ScreenShake.Instance.Shake(0.2f, 0.4f);
        _hitFx.Play();
    }

    private void SpawnBodySection(Vector2Int movement, Vector3 newPos)
    {
        GameObject _section = Instantiate(_bodySection, newPos, Quaternion.identity);
        GameObject root = _rootMaker.AddIvyBranch(transform.position, newPos);
        _body.Push(new BodySection() { body = _section, movement = movement, root = root, position = newPos });
    }

    public void OnClickUndo()
    {
        Undo();
    }

    public bool Undo()
    {
        if (_body.Count == 0)
        {
            return false;
        }

        int num = UnityEngine.Random.Range(1, 10);
        AudioManager.instance.Play($"vine_retract_{num}");
        ScreenShake.Instance.Shake(0.1f, 0.01f);

        GameObject partToUndo = _body.Peek().body;


        Destroy(partToUndo);
        Destroy(_body.Peek().root);

        _gridManager.UndoMove(_body.Peek().movement);
        transform.position = _gridManager.GetPosition();

        _body.Pop();
        return true;
    }
}
