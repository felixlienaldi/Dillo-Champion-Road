using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Enumerator
{
    public enum ENEMY_TYPE { 
        NORMAL = 0,
        INVERSE
    }

    public enum GAME_STATE
    {
        MENU,
        PAUSED,
        GAME,
        ENDGAME
    }

    public enum ATTACK_TYPE { 
        NICE,
        GREAT,
        PERFECT
    }

    public enum UPGRADE_TYPE
    {
        SCORE,
        HPGAIN,
        FEVER,
        BARRIER,
        REVIVE,
        ACCURACY
    }
}
