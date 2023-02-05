using UnityEngine;
using System.Collections;
using Milo.Tools;

public class ScreenShake : Singleton<ScreenShake>   
{
    [SerializeField] private bool _debugMode;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && _debugMode)
        {
            Shake(0.2f, 0.1f);
        }

        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Shake(float duration, float amount)
    {
        _originalPos = gameObject.transform.localPosition;
        StopAllCoroutines();
        StartCoroutine(cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= _fakeDelta;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }


}