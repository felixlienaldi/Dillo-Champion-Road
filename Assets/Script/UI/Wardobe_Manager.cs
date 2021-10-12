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
    public List<GameObject> m_SkinEffect;
    public Button m_WearButton;
    public Button m_BuyButton;
    public TextMeshProUGUI m_PriceText;
    public TextMeshProUGUI m_DescLoreText;
    public TextMeshProUGUI m_DescEffectText;
    public GameObject m_ComingSoon;
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
        m_CurrentID = Player_Manager.m_Instance.m_EquipedClothes;
        f_OnChangeClothesID();
    }

    public void f_PreviousClothes() {
        m_CurrentID--;
        if (m_CurrentID < 0) m_CurrentID = 0;
        f_OnChangeClothesID();
    }

    public void f_NextClothes() {
        m_CurrentID++;
        if (m_CurrentID >= m_Clothes.Count) m_CurrentID = m_Clothes.Count - 1;
        f_OnChangeClothesID();
    }


    public void f_CloseWardobe() {
        Player_Manager.m_Instance.m_EquipedClothes = m_Clothes[m_CurrentID].m_ID;
        Player_GameObject.m_Instance.m_PlayerHitClip.Clear();
        Player_GameObject.m_Instance.m_PlayerHitClip.AddRange(m_Clothes[m_CurrentID].m_HitAudio);
    }

    public void f_OnChangeClothesID() {
        Player_GameObject.m_Instance.m_Animator.runtimeAnimatorController = m_Clothes[m_CurrentID].m_Animator;
        m_DescEffectText.text = m_Clothes[m_CurrentID].m_DescEffect;
        m_DescLoreText.text = m_Clothes[m_CurrentID].m_DescriptionLore;
        for (int i = 0; i < m_SkinEffect.Count; i++) {
            if (m_SkinEffect[i] != null) m_SkinEffect[i].SetActive(false);
        }
        if (m_SkinEffect[m_CurrentID] != null) m_SkinEffect[m_CurrentID].SetActive(true);

        if (m_Clothes[m_CurrentID].m_Locked || m_Clothes[m_CurrentID].m_ComingSoon) {
            m_WearButton.gameObject.SetActive(false);
            m_BuyButton.gameObject.SetActive(true);
            if (!m_Clothes[m_CurrentID].m_ComingSoon) {
                m_ComingSoon.SetActive(false);
                m_PriceText.text = m_Clothes[m_CurrentID].m_Price.ToString("0");
                if (m_Clothes[m_CurrentID].m_Price > Player_Manager.m_Instance.m_Berry) {
                    m_PriceText.color = Color.red;
                    m_BuyButton.interactable = false;
                }
                else {
                    m_PriceText.color = Color.white;
                    m_BuyButton.interactable = true;
                }
            }
            else {
                m_ComingSoon.SetActive(true);
                m_BuyButton.interactable = false;
                m_PriceText.color = Color.white;
                m_PriceText.text = m_Clothes[m_CurrentID].m_Price.ToString("-");
            }
        }
        else {
            m_ComingSoon.SetActive(false);
            m_WearButton.gameObject.SetActive(true);
            m_BuyButton.gameObject.SetActive(false);
        }
    }

    public void f_Buy() {
        m_Clothes[m_CurrentID].m_Locked = false;
        f_OnChangeClothesID();
    }
}
