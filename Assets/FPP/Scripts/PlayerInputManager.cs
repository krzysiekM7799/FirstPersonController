using UnityEngine;
using UnityEngine.InputSystem;

namespace FPP
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerController playerController;
      
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        public void OnMove(InputValue value)
        {
            Debug.Log("Move");
            playerController.PlayerMovementProperties.moveVector = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {        
            playerController.PlayerMovementProperties.lookVector = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            playerController.PlayerMovementProperties.sprint = value.isPressed;
        }

        public void OnCrouch(InputValue value)
        {
            Debug.Log("Crouch");
        }

        public void OnJump(InputValue value)
        {   
            playerController.PlayerMovementProperties.jump = value.isPressed;            
        }

    }
}
