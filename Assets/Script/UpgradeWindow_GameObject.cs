using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeWindow_GameObject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====

    //===== STRUCT =====
    //===== PUBLIC =====
    public Buff_GameObject m_BuffDetails;
    public TMP_InputField m_BaseValue;
    public TMP_InputField m_ValuePerUpgrade;
    public TMP_InputField m_Level;
    public TextMeshProUGUI m_TotalMultiplier;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){

    }

    void Start(){
        m_BaseValue.text = m_BuffDetails.m_Value.ToString();
        m_ValuePerUpgrade.text = m_BuffDetails.m_UpgradeValue.ToString();
        m_Level.text = m_BuffDetails.m_Level.ToString();
    }

    void Update(){
        m_TotalMultiplier.text = m_BuffDetails.f_GetTotalMultiplier().ToString("F2");
    }

    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_SetValue(string p_Value) {
        m_BaseValue.text = p_Value;
        m_BuffDetails.f_SetValue(float.Parse(p_Value));
    }

    public void f_SetUpgradeValue(string p_Value) {
        m_ValuePerUpgrade.text = p_Value;
        m_BuffDetails.f_SetUpgradeValue(float.Parse(p_Value));
    }

    public void f_SetLevelValue(string p_Value) {
        m_Level.text = p_Value;
        m_BuffDetails.f_SetLevel(int.Parse(p_Value));
    }

    public void f_GetValue(string p_Value) {
        m_BaseValue.text = p_Value;
    }
    public void f_GetUpgradeValue(string p_Value) {
        m_ValuePerUpgrade.text = p_Value;
    }

    public void f_GetLevel(string p_Value) {
        m_Level.text = p_Value;
    }
}
