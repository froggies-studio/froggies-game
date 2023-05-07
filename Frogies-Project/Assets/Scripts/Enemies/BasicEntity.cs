using System;
using Core.Player;
using StatsSystem;
using StatsSystem.Health;
using UnityEngine;

namespace Enemies
{
    public abstract class BasicEntity
    {
        // protected EntityBrain _brain;
        public HealthSystem HealthSystem { get; protected set; }
        public StatsController StatsController { get;protected set; }

        // protected void Update()
        // {
        //     _brain.Update();
        // }
        //
        // protected void FixedUpdate()
        // {
        //     _brain.FixedUpdate();
        // }

        public abstract void Update();

        public abstract void FixedUpdate();
    }
}