using System;
using Core.Player;
using StatsSystem.Health;
using UnityEngine;

namespace Enemies
{
    public class BasicEntity : MonoBehaviour
    {
        // protected EntityBrain _brain;
        public HealthSystem HealthSystem { get; protected set; }

        // protected void Update()
        // {
        //     _brain.Update();
        // }
        //
        // protected void FixedUpdate()
        // {
        //     _brain.FixedUpdate();
        // }
    }
}