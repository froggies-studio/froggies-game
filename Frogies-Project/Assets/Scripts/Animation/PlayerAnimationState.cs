using System;

namespace Animation
{
    [Serializable]
    public enum PlayerAnimationState
    {
        Idle = 0,
        Run = 1,
        RollOver = 2,
        Attack = 3,
        Attack2 = 4,
        QuickTurn = 5,
        Jump = 6,
        Fall = 7,
        GetHit = 8,
        Death = 9,
    }
}