﻿namespace Core.Entities.Player
{
    public class Player : BasicEntity
    {
        public void Initialize(EntityBrain brain)
        {
            Brain = brain;
        }

        public override void Update()
        {
            Brain.Update();
        }

        public override void FixedUpdate()
        {
            Brain.FixedUpdate();
        }
    }
}