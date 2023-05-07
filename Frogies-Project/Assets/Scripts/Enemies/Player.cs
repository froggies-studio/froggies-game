using Core.Player;

namespace Enemies
{
    public class Player : BasicEntity
    {
        private EntityBrain _brain;
        public void Initialize(EntityBrain brain)
        {
            _brain = brain;
            HealthSystem = _brain.HealthSystem;
        }
        
        private void Update()
        {
            _brain.Update();
        }
        
        private void FixedUpdate()
        {
            _brain.FixedUpdate();
        }
    }
}