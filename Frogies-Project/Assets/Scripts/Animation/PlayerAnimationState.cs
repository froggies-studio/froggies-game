using System;

namespace Animation
{
    [Serializable]
    public enum PlayerAnimationState
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Attack2 = 3,
        QuickTurn = 4,
        Jump = 5,
        Fall = 6,
        GetHit = 7,
        Death = 8,
    }
}