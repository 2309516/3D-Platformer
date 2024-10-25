using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public Vector3 rotationDirection = Vector3.up;
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(rotationDirection.normalized, rotationSpeed * Time.deltaTime);
    }
}
