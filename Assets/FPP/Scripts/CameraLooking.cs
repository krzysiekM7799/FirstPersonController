using UnityEngine;

namespace FPP
{
    public class CameraLooking : MonoBehaviour
    {
        //Camera control settings
        [Tooltip("How fast camera follows player character")]
        [SerializeField] private float cameraLerpFollowSpeed = 13;
        [Tooltip("How fast camera rotate")]
        [SerializeField] private float cameraLerpRotationSpeed = 13;
        [Range(30, 120)]
        [Tooltip("Field of view of camera")]
        [SerializeField] private float cameraFov = 90f;
        [Range(0,90)]
        [Tooltip("How much can you tilt the camera up")]
        [SerializeField] private float cameraUpMaxAngle = 90f;
        [Range(0, -90)]
        [Tooltip("How much can you tilt the camera down")]
        [SerializeField] private float cameraDownMaxAngle = -90f;
        [SerializeField] private float lookingHorizontalSensivity = 14f;
        [SerializeField] private float lookingVerticalSensivity = 14f;

        //Camera properties
        private float cameraRotation;
        private Transform mainCameraTransform;
        private Camera mainCamera;
        private Transform cameraHolder;
        [SerializeField] private Transform cameraTargetPosition;
     
        //Changing camera position properties
        private bool camAtDestinationPointAlready = true;
        private Vector3 destinationCameraPoint;
        public Vector3 DestinationCameraPoint { get { return destinationCameraPoint; } set { destinationCameraPoint = value; camAtDestinationPointAlready = false; } }

        //Properites
        public Transform CameraHolder { get => cameraHolder; }
        public float LookingHorizontalSensivity { get => lookingHorizontalSensivity; }
        public float LookingVerticalSensivity { get => lookingVerticalSensivity; }

        private void Awake()
        {
            if (cameraTargetPosition != null)
            {
                GameObject cameraHolderGO = new GameObject("cameraHolder");
                cameraHolderGO.transform.parent = transform;
                cameraHolder = cameraHolderGO.transform;
                cameraHolder.localPosition = Vector3.zero;

                cameraTargetPosition.transform.parent = cameraHolder;
            }
            else
            {
                Debug.LogError("You have to add empty gameobject  with animator or not as a child of PlayerGO and assign it to CameraLookingComponent at cameraTargetPosition");
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;     
        }

        private void Update()
        {
            HandleSwitchCameraPosition();
            LerpCamera();
            mainCamera.fieldOfView = cameraFov;
        }

        //Looking around methods
        public void LookAround(float verticalLook)
        {
            //Rotating camera vertical
            cameraRotation -= verticalLook * lookingVerticalSensivity * Time.deltaTime;

            //Clamp vertical rotation
            cameraRotation = Mathf.Clamp(cameraRotation, cameraDownMaxAngle, cameraUpMaxAngle);
            cameraTargetPosition.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
        }
        public void LookAround(float verticalLook, float horizontalLook)
        {
            LookAround(verticalLook);
            cameraHolder.Rotate(new Vector3(0, horizontalLook * lookingHorizontalSensivity * Time.deltaTime, 0));
        }

        private void LerpCamera()
        {          
            mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, cameraTargetPosition.position, cameraLerpFollowSpeed * Time.deltaTime);
            mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, cameraTargetPosition.rotation, Time.time * cameraLerpRotationSpeed);
        }
       
        private void HandleSwitchCameraPosition()
        {
            if (!camAtDestinationPointAlready)
            {
                cameraHolder.localPosition = destinationCameraPoint;
                camAtDestinationPointAlready = true;
            }          
        }
    }
}
