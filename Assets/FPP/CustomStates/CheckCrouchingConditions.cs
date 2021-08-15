namespace FPP
{
    [System.Serializable]
    public class CheckCrouchingConditions : BTNode
    {
        PlayerCharacter playerCharacter;
        FirstPersonPlayerController firstPersonPlayerController;

        public CheckCrouchingConditions(PlayerCharacter playerCharacter, FirstPersonPlayerController firstPersonPlayerController)
        {
            this.playerCharacter = playerCharacter;
            this.firstPersonPlayerController = firstPersonPlayerController;
        }

        public override NodeStates Evaluate()
        {
            if (firstPersonPlayerController.Crouch)
            {
                return NodeStates.SUCCESS;
            }
            return NodeStates.FAILURE;
        }
    }
}
