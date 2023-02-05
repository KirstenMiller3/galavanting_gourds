using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
  
    void Start()
    {
        transform.DOPunchScale(Vector3.up, 1f);
    }


}
