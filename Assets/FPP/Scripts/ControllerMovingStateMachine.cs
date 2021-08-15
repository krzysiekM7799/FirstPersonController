using FPP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPP
{
    [System.Serializable]
    public class ControllerMovingStateMachine : StateMachine<MovingState>
    {
        //AirBorn state
        private Sequence AirBornStates;
        private CheckAirBornConditions checkAirBornConditions;
        public HandleAirBorn handleAirBorn;

        //WhileMovingStates stores all the states that take place during the move 
        private Sequence MovingStates;
        private CheckMovingConditions checkMovingConditions;
        private Selector chooseMovingStateType;
        //Crouch while moving state
        private Sequence crouchingWalkState;
        private CheckCrouchingConditions checkCrouchingWalkConditions;
        public HandleCrouching handleCrouchingWalk;
        //Running state
        private Sequence runningState;
        private CheckRunningConditions checkRunningConditions;
        public HandleRunning handleRunning;
        //Walking state
        private Sequence walkingState;
        private CheckWalkingConditions checkWalkingConditions;
        public HandleWalking handleWalking;

        //IdlingStates stores all the states that take place when player standing in place
        private Selector IdlingStates;
        //Crouch in place state
        private Sequence crouchingIdleState;
        private CheckCrouchingConditions checkCrouchingIdleConditions;
        public HandleCrouching handleCrouchingIdle;
        //Idle state
        private Sequence idleState;
        public HandleIdle handleIdle;

        //Needed components
        private FirstPersonPlayerController firstPersonPlayerController;
        private PlayerCharacter playerCharacter;

        //Delegates
        public delegate void MovingStateActionDelegate();

        public ControllerMovingStateMachine(FirstPersonPlayerController firstPersonPlayerController, PlayerCharacter playerCharacter)
        {
            this.firstPersonPlayerController = firstPersonPlayerController;
            this.playerCharacter = playerCharacter;
        }

        public override void MakeStateMachine()
        {
            SetIdlingStates();
            SetMovingStates();
            SetAirbornStates();

            root = new Selector();
            root.SetChildrenOfNode(AirBornStates);
            root.SetChildrenOfNode(MovingStates);
            root.SetChildrenOfNode(IdlingStates);
        }

        private void SetAirbornStates()
        {
            //Airborn state
            checkAirBornConditions = new CheckAirBornConditions(playerCharacter);
            handleAirBorn = new HandleAirBorn(this);
            AirBornStates = new Sequence();
            AirBornStates.SetChildrenOfNode(checkAirBornConditions);
            AirBornStates.SetChildrenOfNode(handleAirBorn);
        }

        private void SetMovingStates()
        {
            //Walking state
            checkWalkingConditions = new CheckWalkingConditions(firstPersonPlayerController);
            handleWalking = new HandleWalking(playerCharacter, this);
            walkingState = new Sequence();
            walkingState.SetChildrenOfNode(checkWalkingConditions);
            walkingState.SetChildrenOfNode(handleWalking);

            //Running state
            checkRunningConditions = new CheckRunningConditions(firstPersonPlayerController);
            handleRunning = new HandleRunning(playerCharacter, this);
            runningState = new Sequence();
            runningState.SetChildrenOfNode(checkRunningConditions);
            runningState.SetChildrenOfNode(handleRunning);

            //Crouching Walk state
            checkCrouchingWalkConditions = new CheckCrouchingConditions(playerCharacter, firstPersonPlayerController);
            handleCrouchingWalk = new HandleCrouching(playerCharacter, this, MovingState.CrouchingWalk);
            crouchingWalkState = new Sequence();
            crouchingWalkState.SetChildrenOfNode(checkCrouchingWalkConditions);
            crouchingWalkState.SetChildrenOfNode(handleCrouchingWalk);

            chooseMovingStateType = new Selector();
            chooseMovingStateType.SetChildrenOfNode(crouchingWalkState);
            chooseMovingStateType.SetChildrenOfNode(runningState);
            chooseMovingStateType.SetChildrenOfNode(walkingState);
            checkMovingConditions = new CheckMovingConditions(firstPersonPlayerController);

            //Making main Moving States
            MovingStates = new Sequence();
            MovingStates.SetChildrenOfNode(checkMovingConditions);
            MovingStates.SetChildrenOfNode(chooseMovingStateType);
        }

        private void SetIdlingStates()
        {
            //Crouching Idle state
            checkCrouchingIdleConditions = new CheckCrouchingConditions(playerCharacter, firstPersonPlayerController);
            handleCrouchingIdle = new HandleCrouching(playerCharacter, this, MovingState.CrouchingIdle);
            crouchingIdleState = new Sequence();
            crouchingIdleState.SetChildrenOfNode(checkCrouchingIdleConditions);
            crouchingIdleState.SetChildrenOfNode(handleCrouchingIdle);

            //Idle state
            handleIdle = new HandleIdle(this);
            idleState = new Sequence(handleIdle);

            IdlingStates = new Selector();
            IdlingStates.SetChildrenOfNode(crouchingIdleState);
            IdlingStates.SetChildrenOfNode(idleState);
        }

        protected override void Evaluate()
        {
            root.Evaluate();
            playerCharacter.Move(firstPersonPlayerController.LastNot0Vector, GetMovingStateInfo(CurrentState));
        }

        private MoveInfo GetMovingStateInfo(MovingState movingState)
        {
            switch (movingState)
            {
                case MovingState.Idle:
                    {
                        MoveInfo moveInfo;
                        moveInfo.speed = 0;
                        moveInfo.acceleration = playerCharacter.BreakingSpeed;
                        return moveInfo;
                    }
                case MovingState.Walking:
                    {
                        if (playerCharacter.CurrentSpeed <= playerCharacter.WalkingSpeed)
                        {
                            MoveInfo moveInfo;
                            moveInfo.speed = playerCharacter.WalkingSpeed;
                            moveInfo.acceleration = playerCharacter.AccelerationSpeed;
                            return moveInfo;
                        }
                        else
                        {
                            MoveInfo moveInfo;
                            moveInfo.speed = playerCharacter.WalkingSpeed;
                            moveInfo.acceleration = playerCharacter.BreakingSpeed;
                            return moveInfo;
                        }
                    }
                case MovingState.Running:
                    {
                        MoveInfo moveInfo;
                        moveInfo.speed = playerCharacter.RunningSpeed;
                        moveInfo.acceleration = playerCharacter.AccelerationSpeed;
                        return moveInfo;
                    }
                case MovingState.CrouchingWalk:
                    {
                        if (playerCharacter.CurrentSpeed <= playerCharacter.CrouchingSpeed)
                        {
                            MoveInfo moveInfo;
                            moveInfo.speed = playerCharacter.CrouchingSpeed;
                            moveInfo.acceleration = playerCharacter.AccelerationSpeed;
                            return moveInfo;
                        }
                        else
                        {
                            MoveInfo moveInfo;
                            moveInfo.speed = playerCharacter.CrouchingSpeed;
                            moveInfo.acceleration = playerCharacter.BreakingSpeed;
                            return moveInfo;
                        }
                    }
                case MovingState.CrouchingIdle:
                    {
                        MoveInfo moveInfo;
                        moveInfo.speed = 0;
                        moveInfo.acceleration = playerCharacter.BreakingSpeed;
                        return moveInfo;
                    }
            }

            return new MoveInfo { speed = 0, acceleration = 0 };
        }
    }
}
