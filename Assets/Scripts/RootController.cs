using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour {
    [SerializeField] private GridManager _gridManager;  
    [SerializeField] private float _inputPollSpeed = 0.1f;

    private float _inputTimer = 0f;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) {
            _gridManager.Move(Vector2Int.up);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            _gridManager.Move(Vector2Int.down);
        }
        else if(Input.GetKeyDown(KeyCode.D)) { 
            _gridManager.Move(Vector2Int.right); 
        }  
        else if(Input.GetKeyDown(KeyCode.A)) { 
            _gridManager.Move(Vector2Int.left);    
        }

        //Debug.Log(_gridManager.GetPosition());
        transform.position = _gridManager.GetPosition();
    }

}
