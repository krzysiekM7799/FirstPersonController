namespace FPP
{
    public class CheckWalkingConditions : BTNode
    {
        FirstPersonPlayerController firstPersonPlayerController;

        public CheckWalkingConditions(FirstPersonPlayerController firstPersonPlayerController)
        {
            this.firstPersonPlayerController = firstPersonPlayerController;
        }

        public override NodeStates Evaluate()
        {
            return NodeStates.SUCCESS;
        }
    }
}
