namespace FPP
{
    public class CheckAirBornConditions : BTNode
    {
        PlayerCharacter playerCharacter;

        public CheckAirBornConditions(PlayerCharacter playerCharacter)
        {
            this.playerCharacter = playerCharacter;
        }

        public override NodeStates Evaluate()
        {
            if (!playerCharacter.IsGrounded)
            {
                return NodeStates.SUCCESS;
            }
            return NodeStates.FAILURE;
        }

    }
}
