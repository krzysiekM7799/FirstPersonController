namespace FPP
{
    public class HandleIdle : BTNode
    {
        private ControllerMovingStateMachine controllerStateMachine;
        public ControllerMovingStateMachine.MovingStateActionDelegate idleStateActionDel;

        public HandleIdle(ControllerMovingStateMachine controllerStateMachine, ControllerMovingStateMachine.MovingStateActionDelegate[] movingStateActionDelegate = null)
        {
            this.controllerStateMachine = controllerStateMachine;
            if (movingStateActionDelegate != null)
            {
                foreach (ControllerMovingStateMachine.MovingStateActionDelegate action in movingStateActionDelegate)
                {
                    idleStateActionDel += action;
                }
            }
        }

        public override NodeStates Evaluate()
        {
            if (controllerStateMachine.PreviousState != MovingState.Idle)
            {
                controllerStateMachine.CurrentState = MovingState.Idle;
                idleStateActionDel?.Invoke();
            }
            return NodeStates.SUCCESS;

        }
    }
}
