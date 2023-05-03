using Movement;

namespace Enemies
{
    public class EnemyMovementInput : IMovementInputProvider
    {
        public MovementInput Input => new();
        public void ResetOneTimeActions()
        {
            //nothing
        }
    }
}