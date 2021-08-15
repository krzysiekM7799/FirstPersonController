using UnityEngine;

namespace FPP
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Movement Properties")]
        [Tooltip("Max speed of character when walking")]
        [SerializeField] private float walkingSpeed = 3;
        [Tooltip("Max speed of character when running")]
        [SerializeField] private float runningSpeed = 6;
        [Tooltip("Max speed of character when crouching")]
        [SerializeField] private float crouchingSpeed = 1;
        private bool isCrouched;
        [Tooltip("The force with which character is pushed upwards")]
        [SerializeField] private float jumpingForce = 4;
        private float currentSpeed;
        [Tooltip("Speed at which the character accelerates")]
        [SerializeField] private float accelerationSpeed = 7;
        [Tooltip("Speed at which the character slows down")]
        [SerializeField] private float breakingSpeed = 10;

        [Header("Check Ground Properties")]
        [Tooltip("Start point for ground checking")]
        [SerializeField] private Transform GroundChecker;
        private bool isGrounded;
        [Tooltip("Radius of ground checking")]
        [SerializeField] private float radiusOfGroundChecker = 0.4f;
        [Tooltip("Layer on which ground is checking, set this layer to every ground object on scene too")]
        [SerializeField] private LayerMask layerMask;

        //Crouching properties
        [Tooltip("Height of character, capsule collider height")]
        [SerializeField] private float characterHeight = 2;
        [Range(0.1f, 0.9f)]
        [Tooltip("How many percent of the height should crouch be f.e 0.6 is 60% of characterHeight")]
        [SerializeField] private float percentHeightOfCrouch = 0.6f;      
        private float characterHeightCrouching = 1.5f;
        private float characterCenter;
        private float characterCenterCrouching;
              
        //Needed components
        private Rigidbody _rigidbody;        
        private CapsuleCollider capsuleCollider;

        //Properties
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public float CharacterHeight { get => characterHeight; set => characterHeight = value; }
        public float CharacterHeightCrouching { get => characterHeightCrouching; set => characterHeightCrouching = value; }
        public float CharacterCenter { get => characterCenter; set => characterCenter = value; }
        public float CharacterCenterCrouching { get => characterCenterCrouching; set => characterCenterCrouching = value; }
        public bool IsCrouched { get => isCrouched; set => isCrouched = value; }
        public float WalkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
        public float RunningSpeed { get => runningSpeed; set => runningSpeed = value; }
        public float CrouchingSpeed { get => crouchingSpeed; set => crouchingSpeed = value; }
        public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
        public float AccelerationSpeed { get => accelerationSpeed; set => accelerationSpeed = value; }
        public float BreakingSpeed { get => breakingSpeed; set => breakingSpeed = value; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleCollider.material = new PhysicMaterial();
            capsuleCollider.material.dynamicFriction = 0;
            capsuleCollider.material.staticFriction = 0;

            if (GroundChecker == null)
                Debug.LogError("Assign ground checker object");
            
            if (layerMask == LayerMask.GetMask("Nothing"))
                Debug.LogError("Set layerMask to allow detect ground");

            SetCharacterHeightValues();
        }
        private void Update()
        {
            CheckGround();
        }

        public void Move(Vector2 move, MoveInfo moveInfo)
        {
            if (isGrounded)
            {
                SetMove(move, moveInfo);
            }
        }

        private void SetMove(Vector2 move, MoveInfo moveInfo)
        {
            var transformedVector = (move.y * transform.forward + move.x * transform.right).normalized;
            
            Vector2 transformedMove;
            transformedMove.x = transformedVector.x;
            transformedMove.y = transformedVector.z;

            float currentYVelocity = _rigidbody.velocity.y;

            currentSpeed = Mathf.Lerp(currentSpeed, moveInfo.speed, moveInfo.acceleration * Time.deltaTime);

            _rigidbody.velocity = new Vector3(transformedMove.x * currentSpeed, currentYVelocity, transformedMove.y * currentSpeed);
        }

        public void Jump()
        {           
            if (IsGrounded)
                _rigidbody.AddForce(new Vector3(0, jumpingForce, 0), ForceMode.Impulse);      
        }

        public void Crouch(bool isCrouching)
        {
            if (isCrouching)
            {
                capsuleCollider.height = characterHeightCrouching;                
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, characterCenterCrouching, capsuleCollider.center.z);
            }
            else
            {
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, characterCenter, capsuleCollider.center.z);
                capsuleCollider.height = characterHeight;
            }           
        }

        private void SetCharacterHeightValues()
        {
            capsuleCollider.height = characterHeight;
            characterHeightCrouching = characterHeight * percentHeightOfCrouch;
            float characterHeightDiffrence = (characterHeight - characterHeightCrouching) / 2;
            characterCenter = capsuleCollider.center.y;
            characterCenterCrouching = characterCenter - characterHeightDiffrence;
        }

        private void CheckGround()
        {
            IsGrounded = Physics.CheckSphere(GroundChecker.position, radiusOfGroundChecker, layerMask);
        }

        private void OnDrawGizmos()
        {            
            Gizmos.color = Color.red;
            if(GroundChecker != null)
            Gizmos.DrawSphere(GroundChecker.position, radiusOfGroundChecker);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position + new Vector3(0, characterCenter - characterHeight/2,0), transform.position + new Vector3(0, characterCenter + characterHeight / 2, 0));
        }      
    }
}
