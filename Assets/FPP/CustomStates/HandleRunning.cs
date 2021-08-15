namespace FPP
{
    public class HandleRunning : BTNode
    {
        private PlayerCharacter playerCharacter;
        private ControllerMovingStateMachine controllerStateMachine;
        public ControllerMovingStateMachine.MovingStateActionDelegate runningStateActionDel;

        public HandleRunning(PlayerCharacter playerCharacter, ControllerMovingStateMachine controllerStateMachine)
        {
            this.playerCharacter = playerCharacter;
            this.controllerStateMachine = controllerStateMachine;
        }

        public override NodeStates Evaluate()
        {
            if (controllerStateMachine.PreviousState != MovingState.Running)
            {
                controllerStateMachine.CurrentState = MovingState.Running;
                runningStateActionDel?.Invoke();
            }
            return NodeStates.SUCCESS;

        }
    }
}
