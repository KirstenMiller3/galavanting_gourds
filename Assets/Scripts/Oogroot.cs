using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Oogroot : MonoBehaviour
{
    private Vector3[] _targets;

    private int _count = 0;

    float speed;

    public bool Arrived;

    private void Update()
    {

        if (_targets == null)
        {
            return;
        }
        //Debug.Log("count " + _count + " targetlength " + _targets.Length);


        if (_count >= _targets.Length)
        {
            //Debug.Log("setting arrived to true");
            Arrived = true;
            return;
        }

        if (!HasReachedTarget(_targets[_count]))
        {
            transform.position = Vector3.Lerp(transform.position, _targets[_count], Time.deltaTime * speed);
        }
        else
        {
            _count++;
        }


    }


    public void SetTargetPositions(Vector3[] targets)
    {
         speed = Random.Range(1.5f, 3.0f);

        targets.ToList().ForEach(p => Debug.Log("targets " + p));

        _count = 0;
        _targets = targets;
    }

    private bool HasReachedTarget(Vector3 target)
    {
        float dist = Mathf.Abs(Vector3.Distance(transform.position, target));
        return Mathf.Abs(Vector3.Distance(transform.position, target)) <= 0.1f;
    }

}
