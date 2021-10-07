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
    public List<GameObject> m_HTPTitle;
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

    public void f_OnChangeHTPPageNumber(int p_Page) {
        for (int i = 0; i < m_HTPTitle.Count; i++) {
            if (i == p_Page) {
                m_HTPTitle[i].SetActive(true);
            }
            else {
                m_HTPTitle[i].SetActive(false);
            }
        }
    }
}
