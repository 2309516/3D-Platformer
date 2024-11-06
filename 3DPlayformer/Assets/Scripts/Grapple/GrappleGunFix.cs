using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrappleGunFix : MonoBehaviour
{
    private LineRenderer rope;
    private Vector3 grapplePoint;
    public LayerMask Grappleable;
    public Transform lineRendererPoint;
    public Transform Camera;
    public Transform Player;
    private float maxDistance = 100;
    private SpringJoint springJoint;

    private void Awake()
    {
        rope = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (springJoint != null && Input.GetKey(KeyCode.E))
        {
            springJoint.maxDistance = Mathf.Max(0, springJoint.maxDistance - 0.5f);
        }
    }
    private void LateUpdate()
    {
        Rope();
    }
    private void Grapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance))
        {
            grapplePoint = hit.point;
            springJoint = Player.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint;
            float playerDistance = Vector3.Distance(Player.position, grapplePoint);
            springJoint.maxDistance = playerDistance * 0.8f;
            springJoint.maxDistance = playerDistance * 0.25f;
            springJoint.spring = 7f;
            springJoint.damper = 50f;
            springJoint.massScale = 4.5f;
            rope.positionCount = 2;
        }
    }
    private void StopGrapple()
    {
        rope.positionCount = 0;
        Destroy(springJoint);
    }
    public bool isGrappling()
    {
        return springJoint != null;
    }
    public Vector3 GrapplePoint()
    {
        return grapplePoint;
    }   
    private void Rope()
    {
        if (!springJoint)
        {
            return;
        }
        rope.SetPosition(0, lineRendererPoint.position);
        rope.SetPosition(1, grapplePoint);
    }
}
