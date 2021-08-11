using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPP
{
    public class PlayerController : MonoBehaviour
    {
        //Movement Properties
        public MovementProperties PlayerMovementProperties;

        //Controller Settings
        public ControllerSettings _ControllerSettings;

        //Other Settings
        float cameraRotation;

        //Needed components
        private PlayerCharacter playerCharacter;
        private Transform mainCameraTransform;

        //Properties

        private void Awake()
        {
            playerCharacter = GetComponent<PlayerCharacter>();
            if(Camera.main.transform != null)
                mainCameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            Cursor.lockState = CursorLockMode.Locked;
           
            if (PlayerMovementProperties.lookingHorizontalSensivity == 0)
                PlayerMovementProperties.lookingHorizontalSensivity = 90;
           
            if (PlayerMovementProperties.lookingVerticalSensivity == 0)
                PlayerMovementProperties.lookingVerticalSensivity = 90;
        }

        private void Update()
        {
            PerformMoving();
            PerformJumping();
            RotatePlayer();
        }

        private void PerformJumping()
        {
            if (PlayerMovementProperties.jump)
            {
                playerCharacter.Jump(PlayerMovementProperties.jumpingStrengh);
                PlayerMovementProperties.jump = false;
            }
        }

        private void PerformMoving()
        {
            if (PlayerMovementProperties.moveVector.sqrMagnitude >= _ControllerSettings.movingDeadZone)
            {
                if (!PlayerMovementProperties.sprint)
                    playerCharacter.Move(PlayerMovementProperties.moveVector, PlayerMovementProperties.walkingSpeed);
                else
                    playerCharacter.Move(PlayerMovementProperties.moveVector, PlayerMovementProperties.runningSpeed);
            }
        }

        private void RotatePlayer()
        {
            if (PlayerMovementProperties.lookVector.sqrMagnitude >= _ControllerSettings.lookingDeadZone)
            {
                //Make sure that mouse and gamepad has same sensitivity
                Vector2 _lookVector = Vector2.ClampMagnitude(PlayerMovementProperties.lookVector, 1);

                //Make it independent of the number of fps
                _lookVector *= Time.deltaTime;

                _lookVector.x *= PlayerMovementProperties.lookingHorizontalSensivity;
                _lookVector.y *= PlayerMovementProperties.lookingVerticalSensivity;
               
                //Rotating player horizontal
                transform.Rotate(new Vector3(0, _lookVector.x, 0));

                //Rotating camera vertical
                cameraRotation -= _lookVector.y;
                //Clamp vertical rotation
                cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);
                mainCameraTransform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
                   

            }
        }
    }
}
