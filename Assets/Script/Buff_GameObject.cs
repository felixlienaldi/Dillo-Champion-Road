using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enumerator;

[CreateAssetMenu(fileName = "new Upgrade")]
public class Buff_GameObject : ScriptableObject {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====

    //===== STRUCT =====

    //===== PUBLIC =====
    public UPGRADE_TYPE m_UpgradeType;
    public float m_Value;
    public float m_UpgradeValue;
    public int m_Level;
    public int m_MaxLevel;
    //===== PRIVATES =====

    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_ApplyValue(ref float p_Value) {
        p_Value += m_Value;
    }

    public void f_SetValue(float p_Value) {
        m_Value = p_Value;
    }

    public void f_SetUpgradeValue(float p_UpgradeValue) {
        m_UpgradeValue = p_UpgradeValue;
    }

    public void f_SetLevel(int p_LevelValue) {
        m_Level = p_LevelValue;
    }
    
    public float f_GetTotalMultiplier() {
        return m_Value + (m_UpgradeValue * m_Level);
    }
    public void f_Upgrade() {
        
    }

}
