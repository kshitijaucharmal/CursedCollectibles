using Mirror;
using UnityEngine;
using TMPro;

public class M_FPSPlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] float speed = 450f;
    [SerializeField] float rotSpeed = 200f;
    [SerializeField] float gravity = 0.98f;
    [SerializeField] float jumpForce = 20f;

    [Header("Head Bob")]
    [SerializeField] float headBobSmoothing = 10f;
    [SerializeField] float headbobAmpl = 0.15f;
    [SerializeField] float headbobFreq = 18f;

    [Header("Jump Controls")]
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float groundCheckRadius = 0.05f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Weapon/Accesories")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject[] weaponArray;
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced = 0;

    private int selectedWeaponLocal = 0;

    // Weapon Changing
    void OnWeaponChanged(int _Old, int _New)
    {
        // Disable Old weapon
        if(_Old < weaponArray.Length && weaponArray[_Old] != null) weaponArray[_Old].SetActive(false);

        // Enable new
        if(_New < weaponArray.Length && weaponArray[_New] != null) weaponArray[_New].SetActive(true);
    }

    [Command]
    public void CmdChangeActiveWeapon(int newIndex)
    {
        activeWeaponSynced = newIndex;
    }

    void Awake()
    {
        sceneScript = FindObjectOfType<SceneScript>();
        // Disable all weapons
    }

    private Transform fpsCam;

    private Vector3 cameraPivot;

    private Vector3 movement;
    private float rotX;
    private float rotY;
    private bool canJump = false;

    private bool isGrounded = true;

    public override void OnStartLocalPlayer() {
        sceneScript.player = this;
        Camera.main.transform.SetParent(transform);
        fpsCam = Camera.main.transform;
        fpsCam.localPosition = new Vector3(0, 0.75f, 0f);
        cameraPivot = fpsCam.localPosition;

        // weapon holder
        weaponHolder.SetParent(fpsCam);
        weaponHolder.localPosition = Vector3.zero;

        // Set player name
        CmdSetupPlayer("Player" + Random.Range(100, 999));
        CmdChangeActiveWeapon(selectedWeaponLocal);
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
        if (!isLocalPlayer) return;

        GetInputs();

        if (movement.magnitude != 0)
        {
            var newPos = fpsCam.localPosition;
            newPos.y = Mathf.Lerp(newPos.y, newPos.y + headbobAmpl * Mathf.Sin(headbobFreq * Time.time), headBobSmoothing * Time.deltaTime);
            newPos.x = Mathf.Lerp(newPos.x, newPos.x + headbobAmpl * 0.5f * Mathf.Cos(headbobFreq * 0.5f * Time.time), headBobSmoothing * Time.deltaTime);
            fpsCam.localPosition = newPos;
        }
        else
        {
            fpsCam.localPosition = Vector3.Lerp(fpsCam.localPosition, cameraPivot, headBobSmoothing * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            selectedWeaponLocal += 1;
            if(selectedWeaponLocal >= weaponArray.Length)
            {
                selectedWeaponLocal = 0;
            }
            CmdChangeActiveWeapon(selectedWeaponLocal);
        }
    }

    private void FixedUpdate() {
        if (!isLocalPlayer)
        {
            // playerNameText.transform.LookAt(transform.position);
            return;
        }
        var vel = rb.velocity;
        vel = transform.TransformDirection(movement) * Time.fixedDeltaTime * speed;
        if(!isGrounded) vel.y = rb.velocity.y - gravity;
        rb.velocity = vel;

        // Rotation
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        rb.MoveRotation(Quaternion.Euler(0, rotX, 0f));
        fpsCam.localRotation = Quaternion.Euler(-rotY, 0f, 0f);

        if (canJump) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    // Multiplayer Syncing
    [SerializeField] private TMP_Text playerNameText;
    private SceneScript sceneScript;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    void OnNameChanged(string _old, string _new) {
        playerNameText.text = _new;
    }

    [Command]
    public void CmdSetupPlayer(string _name) {
        playerName = _name;
        sceneScript.statusText = $"{playerName} joined";
    }

    [Command]
    public void CmdSendPlayerMessage(string message)
    {
        if (sceneScript) sceneScript.statusText = $"{playerName}: {message}";
    }


    [SyncVar(hook = nameof(OnItemCollected))]
    public int itemsCollected = 0;

    void OnItemCollected(int _Old, int _New)
    {
        var gm = GameObject.FindObjectOfType<GameManager>();
        gm.CollectItem();
    }
    [Command]
    public void CmdCollectItem()
    {
        itemsCollected++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.gameObject) return;
    }


}
