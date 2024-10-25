using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f, zoomSpeed = 5f;
    public float minZoomDistance = 2f, maxZoomDistance = 15f;

    private float _rotationX = 0f, _rotationY = 0f, _currentZoomDistance = 10f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (target == null) target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        _rotationX -= mouseY;
        _rotationY += mouseX;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        Quaternion targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);

        transform.position = target.position - targetRotation * Vector3.forward * _currentZoomDistance;
        transform.rotation = targetRotation;

        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            float zoomAmount = scrollWheel * zoomSpeed;
            UpdateZoomDistance(zoomAmount);
        }
    }

    private void UpdateZoomDistance(float zoomAmount)
    {
        _currentZoomDistance = Mathf.Clamp(_currentZoomDistance - zoomAmount, minZoomDistance, maxZoomDistance);
    }
}