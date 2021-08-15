namespace FPP
{
    public class CheckRunningConditions : BTNode
    {
        FirstPersonPlayerController firstPersonPlayerController;

        public CheckRunningConditions(FirstPersonPlayerController firstPersonPlayerController)
        {
            this.firstPersonPlayerController = firstPersonPlayerController;
        }

        public override NodeStates Evaluate()
        {
            if (firstPersonPlayerController.Sprint)
            {
                return NodeStates.SUCCESS;
            }
            return NodeStates.FAILURE;
        }
    }
}
