using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

namespace grappleGun
{
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
        public int score = 0;
        private TMP_Text scoreText;
        private GameObject tempPoint;

        [Header("Spring Joint")]
        [SerializeField] private float sj_maxDistance = 0.25f;
        [SerializeField] private float sj_spring = 7f;
        [SerializeField] private float sj_damper = 50f;
        [SerializeField] private float sj_massScale = 4.5f;
        [SerializeField] private int sj_postionCount = 2;


        private void Awake()
        {
            rope = GetComponent<LineRenderer>();
            scoreText = GameObject.Find("score").GetComponent<TMP_Text>();

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
                if (hit.collider.CompareTag("ScoreOBJ"))
                {
                    score++;
                    scoreText.text = "Score: " + score.ToString();
                    tempPoint = hit.collider.gameObject;
                }
                grapplePoint = hit.point;
                springJoint = Player.AddComponent<SpringJoint>();
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.connectedAnchor = grapplePoint;
                float playerDistance = Vector3.Distance(Player.position, grapplePoint);
                // springJoint.maxDistance = playerDistance * 0.8f;
                springJoint.maxDistance = playerDistance * sj_maxDistance;
                springJoint.spring = sj_spring;
                springJoint.damper = sj_damper;
                springJoint.massScale = sj_massScale;
                rope.positionCount = sj_postionCount;
            }
        }
        private void StopGrapple()
        {
            rope.positionCount = 0;
            Destroy(springJoint);

            if (tempPoint == null) return;
            Destroy(tempPoint);

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
}