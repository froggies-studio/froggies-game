namespace Core.Entities
{
    public abstract class BasicEntity
    {
        public EntityBrain Brain { get; protected set; }
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}