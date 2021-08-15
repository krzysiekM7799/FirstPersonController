using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPP
{
    [RequireComponent(typeof(PlayerCharacter))]
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(CameraLooking))]
    public class FirstPersonPlayerController : MonoBehaviour
    {
        //Contoller Properties
        private Vector2 moveVector;
        private Vector2 lookVector;
        private bool sprint;
        private bool crouch;
        private bool IsCrouched;
        private bool jump;
        [SerializeField] private float movingDeadZone = 0.02f;
        [SerializeField] private float lookingDeadZone = 0.02f;

        //Vector which remember last direction of player move
        private Vector3 lastNot0Vector;

        //Class that handles state transitions and calling additional actions
        private ControllerMovingStateMachine movingStateMachine;

        //Needed components
        private PlayerCharacter playerCharacter;
        [Tooltip("Assign here previously prepared CameraLooking, check documentation")]
        private CameraLooking cameraLooking;
        [SerializeField] private Animator headBobAnimator;

        //HashIDs
        public static int Idle = Animator.StringToHash("Idle");
        public static int Walk = Animator.StringToHash("Walk");
        public static int Run = Animator.StringToHash("Run");

        //Properties to handle camera swith when character crouching
        private Vector3 positionStanding;
        private Vector3 positionCrouching;
        private bool camAtStandingPointAlready = true;
        private bool camAtCrouchingPointAlready;

        //Delegates for moving states actions
        public ControllerMovingStateMachine.MovingStateActionDelegate crouchingIdleStateActionDel;
        public ControllerMovingStateMachine.MovingStateActionDelegate crouchingWalkStateActionDel;
        public ControllerMovingStateMachine.MovingStateActionDelegate idleStateActionDel;
        public ControllerMovingStateMachine.MovingStateActionDelegate walkingStateActionDel;
        public ControllerMovingStateMachine.MovingStateActionDelegate runnningStateActionDel;
        public ControllerMovingStateMachine.MovingStateActionDelegate airbornStateActionDel;

        //Properties
        public Vector2 MoveVector { get => moveVector; set => moveVector = value; }
        public Vector2 LookVector { get => lookVector; set => lookVector = value; }
        public bool Sprint { get => sprint; set => sprint = value; }
        public bool Crouch { get => crouch; set => crouch = value; }
        public bool Jump { get => jump; set => jump = value; }
        public float MovingDeadZone { get => movingDeadZone; set => movingDeadZone = value; }
        public Vector3 LastNot0Vector { get => lastNot0Vector; set => lastNot0Vector = value; }

        private void Awake()
        {
            playerCharacter = GetComponent<PlayerCharacter>();
            cameraLooking = GetComponent<CameraLooking>();
            if (Camera.main.transform == null)
                Debug.LogError("You have to put on scene active camera with MainCamera Tag");
        }

        private void Start()
        {
            if (headBobAnimator != null)
                SetHeadBobProperties();
            Cursor.lockState = CursorLockMode.Locked;

            SetCameraValues();

            //State machine instantiation
            movingStateMachine = new ControllerMovingStateMachine(this, playerCharacter);
            movingStateMachine.MakeStateMachine();
            
            //Adding extra actions on states switches, f.e triggering animations
            AddActionsToStateMachine();
        }

        private void FixedUpdate()
        {
            movingStateMachine.EvaluateStateMachine();
            HandleJumping();
            HandleCrouching();
        }

        private void Update()
        {
            HandleRotating();
            HandleSwitchCameraWhenCrouching();
        }

        private void AddActionsToStateMachine()
        {
            movingStateMachine.handleIdle.idleStateActionDel += idleStateActionDel;
            movingStateMachine.handleWalking.walkingStateActionDel += walkingStateActionDel;
            movingStateMachine.handleRunning.runningStateActionDel += runnningStateActionDel;
            movingStateMachine.handleCrouchingIdle.crouchingStateActionDel += crouchingIdleStateActionDel;
            movingStateMachine.handleCrouchingWalk.crouchingStateActionDel += crouchingWalkStateActionDel;
        }

        private void SetCameraValues()
        {
            //Set the position based on the dimensions of the collider
            positionStanding = new Vector3(0, playerCharacter.CharacterCenter + playerCharacter.CharacterHeight / 2, 0);
            positionCrouching = new Vector3(0, playerCharacter.CharacterCenterCrouching + playerCharacter.CharacterHeightCrouching / 2, 0);          
            //Set start position for camera
            cameraLooking.CameraHolder.localPosition = positionStanding;
        }

        private void SetHeadBobProperties()
        {
            idleStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Idle);
            };

            walkingStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Walk);
            };

            runnningStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Run);
            };
            crouchingIdleStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Idle);
            };
            crouchingWalkStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Walk);
            };
            airbornStateActionDel += delegate ()
            {
                headBobAnimator.SetTrigger(Idle);
            };
        }

        private void HandleJumping()
        {
            if (jump)
            {
                playerCharacter.Jump();
                jump = false;
            }
        }

        private void HandleCrouching()
        {
            if (movingStateMachine.CurrentState == MovingState.CrouchingIdle || movingStateMachine.CurrentState == MovingState.CrouchingWalk)
            {
                playerCharacter.Crouch(true);
                IsCrouched = true;

            }
            else if (IsCrouched)
            {
                playerCharacter.Crouch(false);
                IsCrouched = false;
            }
        }

        private void HandleRotating()
        {
            if (lookVector.sqrMagnitude >= lookingDeadZone)
            {
                Vector2 _lookVector = lookVector;
                cameraLooking.LookAround(_lookVector.y);
                playerCharacter.transform.Rotate(new Vector3(0, _lookVector.x * cameraLooking.LookingHorizontalSensivity * Time.deltaTime, 0));
            }
        }

        private void HandleSwitchCameraWhenCrouching()
        {
            if (movingStateMachine.CurrentState == MovingState.CrouchingWalk || movingStateMachine.CurrentState == MovingState.CrouchingIdle)
            {
                if (!camAtCrouchingPointAlready)
                {
                    cameraLooking.DestinationCameraPoint = positionCrouching;
                    camAtCrouchingPointAlready = true;
                    camAtStandingPointAlready = false;
                }
            }
            else
            {
                if (!camAtStandingPointAlready)
                {
                    cameraLooking.DestinationCameraPoint = positionStanding;
                    camAtStandingPointAlready = true;
                    camAtCrouchingPointAlready = false;

                }
            }
        }
    }
}
