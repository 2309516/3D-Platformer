using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 15f, jumpForce = 30f, groundDrag = 5f;
    public float airControlFactor = 0.5f;

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
        if (_isGrounded)
        {
            _rb.AddForce(movement * playerSpeed, ForceMode.Force);
        }
        else
        {
            _rb.AddForce(movement * playerSpeed * airControlFactor, ForceMode.Force);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
            _isGrounded = true;
        else
            _isGrounded = false;

        // Jump logic
        if (Input.GetKey("space") && _isGrounded)
        {
            Vector3 jump = new Vector3(0.0f, jumpForce, 0.0f);
            _rb.AddForce(jump, ForceMode.Impulse);
            _isGrounded = false;
        }

        if (_isGrounded)
        {
            _rb.drag = groundDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }
}

