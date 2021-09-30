using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static UI_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public List<StartMenu_Gameobject> m_StartMenu_Object;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_OpenMenu(int p_ID) {
        for (int i = 0; i < m_StartMenu_Object.Count; i++) {
            m_StartMenu_Object[i].f_CheckID(p_ID);
        }
    }

    public void f_CloseMenu() {
        for (int i = 0; i < m_StartMenu_Object.Count; i++) {
            m_StartMenu_Object[i].gameObject.SetActive(true);
        }
    }
}
