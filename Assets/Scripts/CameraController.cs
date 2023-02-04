using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _oogrootCamera;

    private void Update()
    {
        if (GameController.Instance.State == GameController.GameState.Pikmining)
        {
            if (_gameCamera.gameObject.activeSelf)
            {
                _gameCamera.gameObject.SetActive(false);
            }
        }
        else
        {

        }
    }
}
