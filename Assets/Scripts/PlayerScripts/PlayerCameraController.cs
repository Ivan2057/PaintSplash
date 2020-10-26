using Cinemachine;
using Assets.Scripts.Inputs;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        [SerializeField] public float mouseSensitivity = 1f;
        [SerializeField] Transform cameraTransform;
        float pitch = 0f;
        [Range(1f, 90f)]
        public float maxPitch = 85f;
        [Range(-1f, -90f)]
        public float minPitch = -85f;
        public bool vivo;

        [Header("GameObject - Player")]
        [SerializeField] private Transform pecho = null;
        [SerializeField] private Transform brazoIzq = null;
        [SerializeField] private Transform brazoDer = null;



        private Controls controls;
        private Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        private CinemachineTransposer transposer;

        public override void OnStartAuthority()
        {
            //transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            virtualCamera.gameObject.SetActive(true);

            enabled = true;

            Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();

        private void Update()
        {
            if (!this.gameObject.GetComponent<NetworkIdentity>().hasAuthority) return;
            if (vivo)
            {
                CameraRotate();
                PlayerPrefabRotate();
            }
            else
            {

            }
        }

        private void Look(Vector2 lookAxis)
        {
            
        }

        private float GetPitch()
        {
            //get the mouse inpuit axis values
            float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
            float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
            //turn the whole object based on the x input
            transform.Rotate(0, xInput, 0);
            //now add on y input to pitch, and clamp it
            pitch -= yInput;
            return Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        [Command]
        void CmdProvideRotationsToServer(Quaternion playerRot, GameObject goJugador)
        {
            RpcCameraRotate(playerRot, goJugador);
        }
        [Client]
        private void CameraRotate()
        {
            //create the local rotation value for the camera and set it
            Quaternion rot = Quaternion.Euler(GetPitch(), 0, 0);
            CmdProvideRotationsToServer(rot, this.gameObject);
        }
        [ClientRpc]
        private void RpcCameraRotate(Quaternion rot, GameObject goJugador)
        {
            goJugador.GetComponent<PlayerCameraController>().cameraTransform.localRotation = rot;
        }



        private void PlayerPrefabRotate()
        {
            Quaternion rot = Quaternion.Euler(GetPitch(), 0, 0);
            pecho.localRotation = rot;
        }
    }
}
