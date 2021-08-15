using UnityEngine;

namespace FPP
{
    public class HandleCrouching : BTNode
    {
        private PlayerCharacter playerCharacter;
        private ControllerMovingStateMachine controllerStateMachine;
        public ControllerMovingStateMachine.MovingStateActionDelegate crouchingStateActionDel;
        private MovingState crouchingState;

        public HandleCrouching(PlayerCharacter playerCharacter, ControllerMovingStateMachine controllerStateMachine, MovingState crouchingState, ControllerMovingStateMachine.MovingStateActionDelegate[] movingStateActionDelegate = null)
        {
            this.playerCharacter = playerCharacter;
            this.controllerStateMachine = controllerStateMachine;
            if (crouchingState == MovingState.CrouchingIdle || crouchingState == MovingState.CrouchingWalk)
                this.crouchingState = crouchingState;
            else
                Debug.LogError("Bad type of MovingState in HandleCrouchingClass, it must be CrouchingIdle or CrouchinWalk");

            if (movingStateActionDelegate != null)
            {
                foreach (ControllerMovingStateMachine.MovingStateActionDelegate action in movingStateActionDelegate)
                {
                    crouchingStateActionDel += action;
                }
            }
        }

        public override NodeStates Evaluate()
        {

            if (controllerStateMachine.PreviousState != crouchingState)
            {
                controllerStateMachine.CurrentState = crouchingState;
                crouchingStateActionDel?.Invoke();
            }
            return NodeStates.SUCCESS;

        }
    }
}
