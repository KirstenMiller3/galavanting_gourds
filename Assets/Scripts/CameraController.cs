using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _oogrootCamera;

    public Camera Camera => _activeCam;

    private Camera _activeCam;
    private Transform _focusOogroot;

    private float _baseFOV;

    private void Start()
    {
        _activeCam = _gameCamera;
        _baseFOV = _oogrootCamera.fieldOfView;
        _oogrootCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameController.Instance.State == GameController.GameState.Pikmining || GameController.Instance.State == GameController.GameState.End)
        {
            if (_gameCamera.gameObject.activeSelf)
            {
                _gameCamera.gameObject.SetActive(false);
                _oogrootCamera.gameObject.SetActive(true);
                _activeCam = _oogrootCamera;
            }

            if(_focusOogroot == null)
            {
                _focusOogroot = OogrootController.Instance.oogroots[Random.Range(0, OogrootController.Instance.oogroots.Count)].transform;
            }

            float dist = Vector3.Distance(_focusOogroot.position, _oogrootCamera.transform.position);
            _oogrootCamera.fieldOfView = _baseFOV - (dist * 6);
            _oogrootCamera.transform.LookAt(_focusOogroot.position);
        }
        else
        {
            if (!_gameCamera.gameObject.activeSelf)
            {
                _gameCamera.gameObject.SetActive(true);
                _oogrootCamera.gameObject.SetActive(false);
                _activeCam = _gameCamera;
            }
        }
    }
}
