using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 distance;
    public float smoothSpeed = 0.125f;

    void Start() {
        distance = target.position - transform.position;
    }

    // Start is called before the first frame update
    void Update() {
        transform.position = target.position - distance;  
    }
}
