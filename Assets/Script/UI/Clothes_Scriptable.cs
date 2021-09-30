using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[CreateAssetMenu(fileName = "ClothesSkin", menuName = "ScriptableObjects/ClothesSkin", order = 1)]
public class Clothes_Scriptable : ScriptableObject{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== STRUCT =====

    //===== PUBLIC =====
    public int m_ID = 0;
    public bool m_Locked = true;
    public RuntimeAnimatorController m_Animator;
    public int m_IdleFXId = 0;
    public int m_TapFXId = 0;
}
