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
    }
}