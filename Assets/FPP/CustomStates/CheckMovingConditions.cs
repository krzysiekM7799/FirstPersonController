namespace FPP
{
    public class CheckMovingConditions : BTNode
    {
        private FirstPersonPlayerController firstPersonController;


        public CheckMovingConditions(FirstPersonPlayerController firstPersonController)
        {
            this.firstPersonController = firstPersonController;

        }

        public override NodeStates Evaluate()
        {
            if (firstPersonController.MoveVector.sqrMagnitude >= firstPersonController.MovingDeadZone)
            {
                firstPersonController.LastNot0Vector = firstPersonController.MoveVector;
                return NodeStates.SUCCESS;
            }
            return NodeStates.FAILURE;
        }
    }
}
