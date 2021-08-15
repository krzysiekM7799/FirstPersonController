public enum MovingState
{
    None,
    Idle,
    Walking,
    Running,
    CrouchingIdle,
    CrouchingWalk,
    Airborn,
}
public struct MoveInfo
{
    public float speed;
    public float acceleration;
}