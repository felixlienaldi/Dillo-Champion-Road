using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionWindow_GameObject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====

    //===== STRUCT =====

    //===== PUBLIC =====
    public Buff_GameObject m_BuffDetails;
    public TMP_InputField m_BaseValue;
    public Button m_Apply;
    public Button m_DisApply;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){

    }

    void Start(){
        m_BaseValue.text = m_BuffDetails.m_Value.ToString();
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================

    public void f_Apply() {
        m_DisApply.gameObject.SetActive(true);
        m_Apply.gameObject.SetActive(false);
        GameManager_Manager.m_Instance.m_ListPotion.Add(m_BuffDetails);
    }

    public void f_DisApply() {
        m_Apply.gameObject.SetActive(true);
        m_DisApply.gameObject.SetActive(false);
        GameManager_Manager.m_Instance.m_ListPotion.Remove(m_BuffDetails);
    }

    public void f_SetValue(string p_Value) {
        m_BuffDetails.f_SetValue(float.Parse(p_Value));
    }

    public void f_GetValue(string p_Value) {
        m_BaseValue.text = p_Value;
    }

}
