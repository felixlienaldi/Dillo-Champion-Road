using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupReward_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PopupReward_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public GameObject m_Popup;
    public List<StartMenu_Gameobject> m_RewardType;
    public TextMeshProUGUI m_AddedText;
    //===== PRIVATES =====
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_ShowPopup(int p_ID,int p_Amount) {
        for (int i = 0; i < m_RewardType.Count; i++) {
            m_RewardType[i].gameObject.SetActive(p_ID == m_RewardType[i].m_ID);
        }
        m_AddedText.text = "+" + p_Amount.ToString();
        m_Popup.SetActive(true);
    }
}
