namespace FPP
{
    public class HandleAirBorn : BTNode
    {
        ControllerMovingStateMachine controllerStateMachine;

        public HandleAirBorn(ControllerMovingStateMachine controllerStateMachine)
        {
            this.controllerStateMachine = controllerStateMachine;
        }

        public override NodeStates Evaluate()
        {
            controllerStateMachine.CurrentState = MovingState.Airborn;
            return NodeStates.SUCCESS;
        }

    }
}
