using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMover : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 direction = Vector3.left;
    void Update()
    {
        if (transform.position.x > 3) { direction = Vector3.left; }
        if (transform.position.x < -3) { direction = Vector3.right; }
        transform.Translate(direction*Time.deltaTime*speed);
    }
}
