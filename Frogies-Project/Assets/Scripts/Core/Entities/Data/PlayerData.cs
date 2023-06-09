﻿using System;
using Animation;
using Fighting;
using Movement;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Core.Entities.Data
{
    [Serializable]
    public class PlayerData
    {
        [field: SerializeField] public HealthBar HealthBar { get; private set; }
        [field: SerializeField] public EnduranceControlBar EnduranceControlBar { get; private set; }
        [field: SerializeField] public Transform SpriteFlipper { get; private set; }
        [field: SerializeField] public DirectionalMover DirectionalMover { get; private set; }
        [field: SerializeField] public AnimationStateManager AnimationStateManager { get; private set; }
        [field: SerializeField] public MovementData MovementData { get; private set; }
        [field: SerializeField] public AttacksData AttacksData { get; private set; }
        [field: SerializeField] public Collider2D[] AttackColliders { get; private set; }
        [field: SerializeField] public DamageReceiver DamageReceiver { get; private set; }
        [field: SerializeField] public HitVisualisationData HitVisualisation { get; private set; }
    }
}