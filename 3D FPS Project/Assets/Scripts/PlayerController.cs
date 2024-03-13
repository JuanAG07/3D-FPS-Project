using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpForce = 10f;
    public float gravityModifier = 1f;
    public float mouseSensitivity = 1f;
    public Transform theCamera;
    public Transform groundCheckpoint;
    public LayerMask whatIsGround;
    private bool _canPlayerJump;
    private Vector3 _moveInput;
    private CharacterController _characterController;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Store the y velocity
        float yVelocity = _moveInput.y;
        //Player movement
        //_moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //_moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        Vector3 forwardDirection = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalDirection = transform.right * Input.GetAxis("Horizontal");

        _moveInput = (forwardDirection + horizontalDirection).normalized;
        _moveInput *= moveSpeed;

        //Player Jumping
        _moveInput.y = yVelocity;
        _moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            _moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        _canPlayerJump = Physics.OverlapSphere(groundCheckpoint.position, 0.50f, whatIsGround).Length > 0;

        if (Input.GetKeyDown(KeyCode.Space) && _canPlayerJump)
        {
            _moveInput.y = jumpForce;
        }

        _characterController.Move(_moveInput * Time.deltaTime);

        //Control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        //Player Rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        //Camera Rotation
        theCamera.rotation = Quaternion.Euler(theCamera.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}
