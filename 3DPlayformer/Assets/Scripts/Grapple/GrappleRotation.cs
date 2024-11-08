using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grappleGun;
public class GrappleRotation : MonoBehaviour
{
   public GrappleGunFix grapple;
    private Quaternion rotationAngle;
    private float rotateSpeed = 5f;

    private void Update()   
    {
        if (!grapple.isGrappling())
        {
            rotationAngle = transform.parent.rotation;
        }
        else
        {
            rotationAngle = Quaternion.LookRotation(grapple.GrapplePoint() - transform.position);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, Time.deltaTime * rotateSpeed);
    }
}
