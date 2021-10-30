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
    public float[] m_Price;
    public float m_Value;
    public float m_UpgradeValue;
    public int m_Level;
    public int m_MaxLevel;
    public bool m_Applied;
    public bool m_Bought = false;
    public Sprite m_BuffSprite;
    public string m_Desc;
    public string m_Names;
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

    public float f_GetPrice() {
        return m_Price[m_Level];
    }

    public string f_GetDesc() {
        if (f_IsPotion()) return m_Desc;
        else {
            if (m_Level != m_MaxLevel) {
                if (m_UpgradeType == UPGRADE_TYPE.SCORE) return m_Desc + " " + (f_GetNextTotalMultiplier()).ToString() + "X";
                else if (m_UpgradeType == UPGRADE_TYPE.HPGAIN) return m_Desc + " -" + f_GetNextTotalMultiplier();
                else return m_Desc + " " + (f_GetNextTotalMultiplier() * 10).ToString() + "%";
            }
            else return "MAX LEVEL";
        }
    }

    public float f_GetNextTotalMultiplier() {        
        return m_Value + (m_UpgradeValue * (m_Level+1));
    }

    public float f_GetTotalMultiplier() {
        return m_Value + (m_UpgradeValue * m_Level);
    }

    public bool f_IsPotion() {
        if (m_UpgradeType == UPGRADE_TYPE.SCORE || m_UpgradeType == UPGRADE_TYPE.FEVERTIME || m_UpgradeType == UPGRADE_TYPE.HPGAIN) return false;
        else return true;
    }

}
