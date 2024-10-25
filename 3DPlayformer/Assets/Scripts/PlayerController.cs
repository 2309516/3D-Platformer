using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 15f, jumpForce = 30f;

    private Rigidbody _rb;
    private bool _isGrounded;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (cameraForward * moveVertical + _mainCamera.transform.right * moveHorizontal).normalized;

        _rb.AddForce(movement * playerSpeed);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f)) _isGrounded = true;
        else _isGrounded = false;

        if (Input.GetKey("space") && _isGrounded)
        {
            Vector3 jump = new Vector3(0.0f, (jumpForce * 10), 0.0f);
            _rb.AddForce(jump);
            _isGrounded = false;
        }
    }
}
