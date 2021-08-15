using UnityEngine;
using UnityEngine.InputSystem;

namespace FPP
{
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerInputManager : MonoBehaviour
    {
        //Needed components
        private FirstPersonPlayerController playerController;
      
        private void Awake()
        {
            playerController = GetComponent<FirstPersonPlayerController>();
            if (GetComponent<PlayerInput>().actions == null)
            {
                Debug.LogError("Set actions in PlayerInput");
            }
        }

        public void OnMove(InputValue value)
        {   
            playerController.MoveVector = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {        
            playerController.LookVector = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            playerController.Sprint = value.isPressed;
        }

        public void OnCrouch(InputValue value)
        {
            playerController.Crouch = value.isPressed;
        }

        public void OnJump(InputValue value)
        {   
            playerController.Jump = value.isPressed;            
        }
    }
}
