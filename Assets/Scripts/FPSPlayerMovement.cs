using Mirror;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] float speed = 450f;
    [SerializeField] float rotSpeed = 200f;
    [SerializeField] float gravity = 0.98f;
    [SerializeField] float jumpForce = 20f;

    [Header("Head Bob")]
    [SerializeField] float headBobSmoothing = 0.8f;
    [SerializeField] float headbobAmpl = 4;
    [SerializeField] float headbobFreq = 0.3f;

    [Header("Jump Controls")]
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float groundCheckRadius = 0.05f;
    [SerializeField] private LayerMask whatIsGround;

    private Transform fpsCam;

    private Vector3 cameraPivot;

    private Vector3 movement;
    private float rotX;
    private float rotY;
    private bool canJump = false;

    private bool isGrounded = true;
    private void Awake() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fpsCam = Camera.main.transform;
    }

    private void Start() {
        fpsCam.SetParent(transform);
        fpsCam.position = new Vector3(0, 0.75f, 0);
        cameraPivot = fpsCam.localPosition;
    }
    
    void GetInputs() {
        movement.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        movement.z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        movement.Normalize();

        rotX += Input.GetAxisRaw("Mouse X") * rotSpeed * Time.deltaTime;
        rotY += Input.GetAxisRaw("Mouse Y") * rotSpeed * Time.deltaTime;

        isGrounded = Physics.OverlapSphere(feetPosition.position, groundCheckRadius, whatIsGround).Length > 0;
        if(isGrounded && Input.GetButtonDown("Jump")) {
            canJump = true;
        }

    }

    void Update() {
        GetInputs();

        if (movement.magnitude != 0)
        {
            var newPos = fpsCam.localPosition;
            Debug.Log(Time.time);
            newPos.y = Mathf.Lerp(newPos.y, newPos.y + headbobAmpl * Mathf.Sin(headbobFreq * Time.time), headBobSmoothing * Time.deltaTime);
            newPos.x = Mathf.Lerp(newPos.x, newPos.x + headbobAmpl * 0.5f * Mathf.Cos(headbobFreq * 0.5f * Time.time), headBobSmoothing * Time.deltaTime);
            fpsCam.localPosition = newPos;
        }
        else
        {
            fpsCam.localPosition = Vector3.Lerp(fpsCam.localPosition, cameraPivot, headBobSmoothing * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        fpsCam.rotation = Quaternion.Euler(-rotY, rotX, 0);
        transform.rotation = Quaternion.Euler(0, rotX, 0);

    }

    private void FixedUpdate() {
        
        var vel = rb.velocity;
        vel = transform.TransformDirection(movement) * Time.fixedDeltaTime * speed;
        if(!isGrounded) vel.y = rb.velocity.y - gravity;
        rb.velocity = vel;

        if (canJump) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

}
