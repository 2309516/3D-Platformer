using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 15f, jumpForce = 30f, groundDrag = 5f, airControlFactor = 0.5f, maxVelocity = 10f;

    private Rigidbody _rb;
    private bool _isGrounded;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
        Debug.Log("Player Velocity: " + _rb.velocity.magnitude);
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToMenu();
        }
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (cameraForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal")).normalized;
        
        _rb.AddForce(movement * playerSpeed * (_isGrounded ? 1 : airControlFactor), ForceMode.Force);

        if (_rb.velocity.magnitude > maxVelocity)
        {
            _rb.velocity = _rb.velocity.normalized * maxVelocity;
        }

        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, 2f, ~0);
        _rb.drag = _isGrounded ? groundDrag : 0f;
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
