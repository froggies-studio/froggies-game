using Fighting;

namespace Enemies
{
    public class EnemyInputFightingProvider : IFightingInputProvider
    {
        public int ActiveAttackIndex { get; private set; } = -1;

        public void ResetAttackIndex(int index)
        {
            ActiveAttackIndex = -1;
        }

        public void CalculateAttackInput(bool isInAttackRange)
        {
            if (!isInAttackRange)
            {
                ActiveAttackIndex = -1;
                return;
            }
            
            ActiveAttackIndex = 0;
        }
    }
}