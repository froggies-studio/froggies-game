using System;
using System.Collections.Generic;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using UnityEngine;

namespace Core.Entities.Data
{
    [Serializable]
    public class EnemyData
    {
        [field: SerializeField] public DirectionalMover DirectionalMover { get; private set; }
        [field: SerializeField] public AnimationStateManager AnimationStateManager { get; private set; }
        [field: SerializeField] public MovementData MovementData { get; private set; }
        [field: SerializeField] public AttacksData AttacksData { get; private set; }
        [field: SerializeField] public Collider2D[] AttackColliders { get; private set; }
        [field: SerializeField] public Transform SpriteFlipper { get; private set; }
        [field: SerializeField] public Transform Player { get; set; }
        [field: SerializeField] public StatsStorage StatsStorage { get; private set; }
        [field: SerializeField] public DamageReceiver DamageReceiver { get; private set; }
        [field: SerializeField] public List<Collider2D> Colliders { get; private set; }
        [field: SerializeField] public Renderer Renderer { get; private set; }
        [field: SerializeField] public HitVisualisationData HitVisualisation { get; private set; }
    }
}