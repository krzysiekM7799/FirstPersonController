using UnityEngine;

[System.Serializable]
public struct MovementProperties
{
    public Vector2 moveVector;
    public Vector2 lookVector;
    public bool sprint;
    public bool crouch;
    public bool jump;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpingStrengh;
    public float lookingHorizontalSensivity;
    public float lookingVerticalSensivity;
}

[System.Serializable]
public struct ControllerSettings
{
    public float movingDeadZone;
    public float lookingDeadZone;
}
