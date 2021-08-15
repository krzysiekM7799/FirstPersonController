namespace FPP
{
    public class HandleWalking : BTNode
    {
        private PlayerCharacter playerCharacter;
        private ControllerMovingStateMachine controllerStateMachine;
        public ControllerMovingStateMachine.MovingStateActionDelegate walkingStateActionDel;

        public HandleWalking(PlayerCharacter playerCharacter, ControllerMovingStateMachine controllerStateMachine)
        {
            this.playerCharacter = playerCharacter;
            this.controllerStateMachine = controllerStateMachine;
        }

        public override NodeStates Evaluate()
        {
            if (controllerStateMachine.PreviousState != MovingState.Walking)
            {
                controllerStateMachine.CurrentState = MovingState.Walking;
                walkingStateActionDel?.Invoke();
            }
            return NodeStates.SUCCESS;

        }
    }
}
