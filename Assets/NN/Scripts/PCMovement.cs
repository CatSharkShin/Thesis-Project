using UnityEngine;


public class PCMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 0.1f;
    [SerializeField] private float zoomingSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header("Controls")]
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode grabbedMovementKey = KeyCode.Mouse2;
    [SerializeField] private KeyCode grabbedRotationKey = KeyCode.Mouse1;
    private void LateUpdate()
    {
        HandleMovement();
        HandleRotation();
        HandleScrolling();
    }
    private void HandleRotation()
    {
        float mouseMoveY = Input.GetAxis("Mouse Y");
        float mouseMoveX = Input.GetAxis("Mouse X");
        if (Input.GetKey(grabbedRotationKey))
        {
            transform.RotateAround(transform.position, transform.right, mouseMoveY * -rotationSpeed);
            transform.RotateAround(transform.position, Vector3.up, mouseMoveX * rotationSpeed);
        }
    }
    private void HandleMovement()
    {
        Vector3 moveBy = Vector3.zero;

        if (Input.GetKey(forwardKey))
            moveBy += Vector3.forward * movementSpeed;
        if (Input.GetKey(backKey))
            moveBy += Vector3.back * movementSpeed;
        if (Input.GetKey(leftKey))
            moveBy += Vector3.left * movementSpeed;
        if (Input.GetKey(rightKey))
            moveBy += Vector3.right * movementSpeed;

        float mouseMoveY = Input.GetAxis("Mouse Y");
        float mouseMoveX = Input.GetAxis("Mouse X");


        if (Input.GetKey(grabbedMovementKey))
        {
            moveBy += Vector3.up * mouseMoveY * -movementSpeed;
            moveBy += Vector3.right * mouseMoveX * -movementSpeed;
        }
        transform.Translate(moveBy);
    }

    private void HandleScrolling()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * mouseScroll * zoomingSpeed);
    }
}
