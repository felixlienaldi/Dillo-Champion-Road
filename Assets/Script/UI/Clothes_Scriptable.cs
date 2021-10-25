using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enumerator;
[CreateAssetMenu(fileName = "ClothesSkin", menuName = "ScriptableObjects/ClothesSkin", order = 1)]
public class Clothes_Scriptable : ScriptableObject{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== STRUCT =====

    //===== PUBLIC =====
    public int m_ID = 0;
    public bool m_Locked = true;
    public bool m_ComingSoon = false;
    public float m_Price = 0;
    public int m_FragPrice = 0;
    public FRAGMENT_TYPE m_Type;
    public RuntimeAnimatorController m_Animator;
    public int m_IdleFXId = 0;
    public int m_TapFXId = 0;
    public int m_MaxHit = 4;
    public string m_DescriptionLore;
    public string m_DescEffect;
    public List<AudioClip> m_HitAudio;
    public bool m_IsCrimson = false;
    public bool m_IsAndroid = false;
    public bool m_IsGrandMaster = false;
}
