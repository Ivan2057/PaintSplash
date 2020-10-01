using Assets.Scripts.LobbyScripts;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private CharacterController controller = null;
        [Header("Player Settings")]
        [SerializeField] private float movementSpeed = 5f;
        [Header("Environment")]
        [SerializeField] private float Gravity = 9.81f;


        private Vector2 previousInput;

        public override void OnStartAuthority()
        {
            enabled = true;

            InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            InputManager.Controls.Player.Move.canceled += ctx => ResetMovement();
        }

        [ClientCallback]
        private void Update() => Move();

        [Client]
        private void SetMovement(Vector2 movement) => previousInput = movement;

        [Client]
        private void ResetMovement() => previousInput = Vector2.zero;

        [Client]
        private void Move()
        {
            if (base.isLocalPlayer)
            { 
            Vector3 right = controller.transform.right;
            Vector3 forward = controller.transform.forward;
            right.y = 0f;
            forward.y = 0f;

            Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

            controller.Move(movement * movementSpeed * Time.deltaTime);



            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Saltar tecla down");

                movement.y += 1500f * Time.deltaTime;
                controller.Move(movement * movementSpeed * Time.deltaTime);

            } else {

                movement.y += Gravity * Time.deltaTime * -1;
                controller.Move(movement * movementSpeed * Time.deltaTime);

            }


        }

    }
}
    }
