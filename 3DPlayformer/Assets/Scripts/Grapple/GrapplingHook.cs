using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Transform player;
    public LineRenderer rope;
    public LayerMask grappleable;
    public float maxDistance = 20f;
    public float springForce = 5f;
    public float damper = 5f;
    public float massScale = 5f;
    private Vector3 grapplePoint;
    private SpringJoint springJoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grapple();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
        if (springJoint)
        {
            rope.SetPosition(0, player.position);
            rope.SetPosition(1, grapplePoint);
        }
        Debug.DrawRay(player.transform.position, Camera.main.transform.forward, Color.green);
    }

    private void Grapple()
    {
        RaycastHit hit;
        Debug.Log("RAYCASTING");

        // Perform the raycast
        if (Physics.Raycast(player.transform.position, Camera.main.transform.forward, out hit, maxDistance, grappleable))
        {
            Vector3 grapplePoint = hit.point;
            Debug.Log(grapplePoint);

            // Update the rope positions
            rope.positionCount = 2;
            rope.SetPosition(0, player.position);
            rope.SetPosition(1, grapplePoint);

            // Create the SpringJoint and set its properties
            springJoint = player.gameObject.AddComponent<SpringJoint>();

            // Get the Rigidbody from the hit object and connect it to the SpringJoint
            Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                springJoint.connectedBody = hitRigidbody;
            }
            else
            {
                Debug.LogWarning("The hit object does not have a Rigidbody. The grapple will not work properly.");
                return; // Exit if there's no Rigidbody to connect to
            }

            //springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint; // The position where the joint connects on the hit object

            // Calculate the distance to keep the joint length consistent
            float distanceToPoint = Vector3.Distance(player.position, grapplePoint);

            // Set the distances for the SpringJoint
            springJoint.maxDistance = distanceToPoint; // Maintain original distance
            springJoint.minDistance = distanceToPoint; // Also set minDistance to the same to keep it consistent
            springJoint.damper = damper;
            springJoint.massScale = massScale;
            springJoint.spring = springForce;

            // Log the anchor and connected anchor for debugging
            Debug.Log($"Anchor (local): {springJoint.anchor}, Connected Anchor (world): {springJoint.connectedAnchor}");
        }
    }


    private void StopGrapple()
    {
        rope.positionCount = 0;
        if (springJoint)
        {
            Destroy(springJoint);
        }
    }
}
