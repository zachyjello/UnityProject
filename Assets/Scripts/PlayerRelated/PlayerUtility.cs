namespace Assets.Scripts.PlayerRelated
{
    public static class PlayerUtility
    {
        public enum playerMovementState
        {
            Normal = 0,
            Swimming = 1,
            Gliding = 2
        }

        public enum playerMovementStatus
        {
            Normal = 0,
            Slipping = 1,
            Crouching = 2,
        }
    }
}