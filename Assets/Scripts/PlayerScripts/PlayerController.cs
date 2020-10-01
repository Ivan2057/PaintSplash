using Assets.Scripts.Inputs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls the player
/// </summary>
namespace Assets.Scripts.PlayerScripts
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] public float moveSpeed;
        [SerializeField] public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
        [SyncVar] public float curHealth;
        [SerializeField] public float _gravity = 20;
        //Struct - Contains Multiple Variables (eg...3 floats)
        private Vector3 _moveDir;
        [SerializeField] private Rigidbody rb = null;
        //Reference Variable
        [SerializeField] public GameObject cuerpo;
        
        [SerializeField] public bool isZoomedIn;
        [SerializeField] public bool damaged;
        [SerializeField] public static bool isGrounded;

        [SerializeField] private float verticalDirection;
        [SerializeField] private float horizontalDirection;

        public static bool localPlayer;
        private Controls playerControls;
        private Controls PlayerControls
        {
            get
            {
                if (playerControls != null) return playerControls;
                return playerControls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            PlayerControls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            PlayerControls.Player.Move.canceled += ctx => ResetMovement();
        }

        [ClientCallback]
        private void OnEnable() => PlayerControls.Enable();

        [ClientCallback]
        private void OnDisable() => PlayerControls.Disable();
        [Client]
        private void Start()
        {
            if (!GetComponent<NetworkIdentity>().hasAuthority) return;
            localPlayer = true;
            cuerpo.SetActive(false);

        }
        [Client]
        void Update()
        {

            if (!GetComponent<NetworkIdentity>().hasAuthority) return;
            localPlayer = true;

            verticalDirection = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            horizontalDirection = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            transform.Translate(horizontalDirection, 0, verticalDirection);

            Move(horizontalDirection, verticalDirection);
        }
        /// <summary>
        /// Making the player be able to walking around
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        [Client]
        public void Move(float horizontal, float vertical)
        {
            bool isCrouchPressed = Input.GetButton("Agacharse");
            bool isSprintPressed = Input.GetButton("Sprint");

            //set speed
            if (isCrouchPressed && isSprintPressed)
            {
                moveSpeed = walkSpeed;
            }
            else if (isSprintPressed)
            {
                moveSpeed = runSpeed;
            }
            else if (isCrouchPressed)
            {
                moveSpeed = crouchSpeed;
            }
            else if (isZoomedIn) //isZoomedIn == true
            {
                moveSpeed = crouchSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            //move this direction based off inputs
            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
                    isGrounded = false;
                }


            }
        }
        /// <summary>
        /// Recieve damage when getting shot from another player
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            Debug.Log("Take Damage");
            if (isServer)
            {
                RpcTakeDamage(damage);
            }
            else
            {
                CmdTakeDamage(damage);
            }

        }

        [ClientRpc]
        void RpcTakeDamage(float damage)
        {
            Debug.Log("RPC Taking Damage");
            this.curHealth -= damage;
        }
        [Command]
        void CmdTakeDamage(float damage)
        {
            Debug.Log("cmd Taking Damage");
            TakeDamage(damage);
        }

        [Client]
        void OnCollisionStay()
        {
            isGrounded = true;
        }

        [Client]
        private void SetMovement(Vector3 movement)
        {
            _moveDir = movement;
        }

        [Client]
        private void ResetMovement()
        {
            _moveDir = Vector3.zero;
        }
    }
}