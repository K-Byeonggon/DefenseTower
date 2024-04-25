using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateEnums
{
    public enum RedBatState
    {
        Moving,
        Charging,
        Rushing,
        Dead
    }

    public enum BansheeState
    {
        Moving,
        Attacking,
        Dead
    }

    public enum SwordKnightState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    public enum GolemState
    {
        Idle,
        Rore,
        Moving, 
        Attacking, 
        Dead
    }

    public enum TurretState
    {
        Idle,
        Tracking,
        Attacking
    }
}

namespace Monsters
{
    public enum MonsterName
    {
        RedBat,
        Banshee,
        SwordKnight,
        Golem
    }
}
