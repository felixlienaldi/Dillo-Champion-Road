using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Wardobe_Manager : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Wardobe_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public List<Clothes_Scriptable> m_Clothes;
    //===== PRIVATES =====
    private int m_CurrentID = 0;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake() {
        m_Instance = this;
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_OpenWardobe() {

    }

    public void f_PreviousClothes() {
        m_CurrentID--;
        if (m_CurrentID < 0) m_CurrentID = 0;
        f_OnChangeClothesID();
    }

    public void f_NextClothes() {
        m_CurrentID++;
        if (m_CurrentID >= m_Clothes.Count) m_CurrentID = m_Clothes.Count-1;
        f_OnChangeClothesID();
    }

    public void f_CloseWardobe() { 
    
    }

    public void f_OnChangeClothesID() { 
        
    }
}
