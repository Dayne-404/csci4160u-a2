using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform DesiredPos;

    void Update()
    {
        transform.position = DesiredPos.position;
    }
}
