using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FX_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static FX_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public List<GameObject> m_RightAttackHitEffect;
    public List<GameObject> m_LeftAttackHitEffect;
    public List<GameObject> m_LeftAttackFeverEffect;
    public List<GameObject> m_RightAttackFeverEffect;
    public List<GameObject> m_DilloLeftEffect;
    public List<GameObject> m_DilloRightEffect;
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
    public void f_Left(bool p_Fever) {
        for (int i = 0; i < m_LeftAttackHitEffect.Count; i++) {
            m_LeftAttackHitEffect[i].SetActive(false);
        }

        for (int i = 0; i < m_LeftAttackFeverEffect.Count; i++) {
            m_LeftAttackFeverEffect[i].SetActive(false);
        }

        for (int i = 0; i < m_DilloLeftEffect.Count; i++) {
            if (m_DilloLeftEffect[i] != null) m_DilloLeftEffect[i].SetActive(false);
        }

        if (m_DilloLeftEffect[Player_Manager.m_Instance.m_EquipedClothes] != null) m_DilloLeftEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);

        if (p_Fever) {
            m_LeftAttackFeverEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);
        }
        else {
            m_LeftAttackHitEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);
        }
    }

    public void f_Right(bool p_Fever) {
        for (int i = 0; i < m_RightAttackHitEffect.Count; i++) {
            m_RightAttackHitEffect[i].SetActive(false);
        }

        for (int i = 0; i < m_RightAttackFeverEffect.Count; i++) {
            m_RightAttackFeverEffect[i].SetActive(false);
        }

        for (int i = 0; i < m_DilloRightEffect.Count; i++) {
            if (m_DilloRightEffect[i] != null) m_DilloRightEffect[i].SetActive(false);
        }

        if (m_DilloRightEffect[Player_Manager.m_Instance.m_EquipedClothes] != null) m_DilloRightEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);

        if (p_Fever) {
            m_RightAttackFeverEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);
        }
        else {
            m_RightAttackHitEffect[Player_Manager.m_Instance.m_EquipedClothes].SetActive(true);
        }
    }
}
