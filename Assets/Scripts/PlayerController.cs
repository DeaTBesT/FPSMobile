using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float crouchSpeed;

    [Header("Jump")]
    [SerializeField] private float forceJump;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch")]
    [SerializeField] private float speedCrouching;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float crouchHeight;

    [Header("Camera")]
    [SerializeField] private float sensitivity;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Inputs")]
    [Space, SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick cameraJoystick;

    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;
    private Camera _camera;

    private Vector3 movement;

    private bool isCrouch;

    private float mouseX;
    private float mouseY;

    private float currentSpeed;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _camera = Camera.main;
    }

    private void Update()
    {
        CameraController();

        _collider.height = Mathf.Lerp(_collider.height, isCrouch ? crouchHeight : defaultHeight, speedCrouching * Time.deltaTime);
        currentSpeed = isCrouch ? crouchSpeed : defaultSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        movement.x = moveJoystick.Horizontal;
        movement.z = moveJoystick.Vertical;

        _rigidbody.MovePosition(transform.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }

        _rigidbody.AddForce(forceJump * Vector3.up, ForceMode.Impulse);
    }

    public void Crouch()
    {
        isCrouch = !isCrouch;
    }

    private void CameraController()
    {
        mouseX += cameraJoystick.Horizontal * sensitivity;
        mouseY -= cameraJoystick.Vertical * sensitivity;

        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        _camera.transform.localRotation = Quaternion.AngleAxis(mouseY, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(mouseX, Vector3.up);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, radius, groundLayer);
    }
}
