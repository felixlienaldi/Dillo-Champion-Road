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
    public GameObject m_BuyButtonObject;
    public Button m_BuyButton;
    public Button m_FragBuyButton;
    public Image m_FragImage;
    public TextMeshProUGUI m_PriceText;
    public TextMeshProUGUI m_PriceFragText;
    public TextMeshProUGUI m_DescLoreText;
    public TextMeshProUGUI m_DescEffectText;
    public GameObject m_ComingSoon;
    public GameObject m_Vignette;
    public AudioClip m_BoughtClip;
    public AudioClip m_NewEQClip;
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

    public Clothes_Scriptable f_GetEquippedSkin() {
        return m_Clothes[Player_Manager.m_Instance.m_EquipedClothes];
    }

    public void f_CloseWardobe() {
        Player_Manager.m_Instance.m_EquipedClothes = m_Clothes[m_CurrentID].m_ID;
        Player_GameObject.m_Instance.m_PlayerHitClip.Clear();
        Player_GameObject.m_Instance.m_PlayerHitClip.AddRange(m_Clothes[m_CurrentID].m_HitAudio);
        Player_GameObject.m_Instance.m_IsCrimson = m_Clothes[m_CurrentID].m_IsCrimson;
        Player_GameObject.m_Instance.m_IsAndroid = m_Clothes[m_CurrentID].m_IsAndroid;
        Player_GameObject.m_Instance.m_IsGrandMaster = m_Clothes[m_CurrentID].m_IsGrandMaster;
        Player_GameObject.m_Instance.m_MaxHit = m_Clothes[m_CurrentID].m_MaxHit;
        f_SaveSkinData();
    }

    public void f_OnChangeClothesID() {
        Player_GameObject.m_Instance.m_Animator.runtimeAnimatorController = m_Clothes[m_CurrentID].m_Animator;
        m_DescEffectText.text = m_Clothes[m_CurrentID].m_DescEffect;
        m_DescLoreText.text = m_Clothes[m_CurrentID].m_DescriptionLore;
        for (int i = 0; i < m_SkinEffect.Count; i++) {
            if (m_SkinEffect[i] != null) m_SkinEffect[i].SetActive(false);
        }
        if (m_SkinEffect[m_Clothes[m_CurrentID].m_IdleFXId] != null) m_SkinEffect[m_Clothes[m_CurrentID].m_IdleFXId].SetActive(true);

        m_Vignette.SetActive(m_Clothes[m_CurrentID].m_IsGrandMaster);

        if (m_Clothes[m_CurrentID].m_Locked || m_Clothes[m_CurrentID].m_ComingSoon) {
            m_WearButton.gameObject.SetActive(false);
            m_BuyButtonObject.gameObject.SetActive(true);
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
                m_PriceFragText.text = m_Clothes[m_CurrentID].m_FragPrice.ToString();
                if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.BEGINNER) {
                    m_FragImage.sprite = Fragment_Manager.m_Instance.m_ImageList[0];
                    if (Fragment_Manager.m_Instance.m_BeginnerFragment >= m_Clothes[m_CurrentID].m_FragPrice) {
                        m_FragBuyButton.interactable = true;
                        m_PriceFragText.color = Color.white;
                    }
                    else {
                        m_FragBuyButton.interactable = false;
                        m_PriceFragText.color = Color.red;
                    }
                }
                else if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.MEDIOCORE) {
                    m_FragImage.sprite = Fragment_Manager.m_Instance.m_ImageList[1];
                    if (Fragment_Manager.m_Instance.m_MediocoreFragment >= m_Clothes[m_CurrentID].m_FragPrice) {
                        m_FragBuyButton.interactable = true;
                        m_PriceFragText.color = Color.white;
                    }
                    else {
                        m_FragBuyButton.interactable = false;
                        m_PriceFragText.color = Color.red;
                    }
                }
                else if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.ADVANCE) {
                    m_FragImage.sprite = Fragment_Manager.m_Instance.m_ImageList[2];
                    if (Fragment_Manager.m_Instance.m_AdvanceFragment >= m_Clothes[m_CurrentID].m_FragPrice) {
                        m_FragBuyButton.interactable = true;
                        m_PriceFragText.color = Color.white;
                    }
                    else {
                        m_FragBuyButton.interactable = false;
                        m_PriceFragText.color = Color.red;
                    }
                }
                else {
                    m_FragImage.sprite = Fragment_Manager.m_Instance.m_ImageList[3];
                    if (Fragment_Manager.m_Instance.m_MasterFragment >= m_Clothes[m_CurrentID].m_FragPrice) {
                        m_FragBuyButton.interactable = true;
                        m_PriceFragText.color = Color.white;
                    }
                    else {
                        m_FragBuyButton.interactable = false;
                        m_PriceFragText.color = Color.red;
                    }
                }
            }
            else {
                m_ComingSoon.SetActive(true);
                m_BuyButton.interactable = false;
                m_FragBuyButton.interactable = false;
                m_PriceText.color = Color.white;
                m_PriceFragText.color = Color.white;
                m_PriceFragText.text = "-";
                m_PriceText.text = "-";
            }
        }
        else {
            m_ComingSoon.SetActive(false);
            m_WearButton.gameObject.SetActive(true);
            m_BuyButtonObject.gameObject.SetActive(false);
        }
    }

    public void f_Buy() {
        m_Clothes[m_CurrentID].m_Locked = false;
        Player_Manager.m_Instance.m_EquipedClothes = m_Clothes[m_CurrentID].m_ID;
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE",(int)m_Clothes[m_CurrentID].m_Price);
        Audio_Manager.m_Instance.f_PlayOneShot(m_BoughtClip);
        Audio_Manager.m_Instance.f_PlayOneShot(m_NewEQClip);
        f_OnChangeClothesID();
        f_SaveSkinData();
    }

    public void f_BuyWithFrags() {
        m_Clothes[m_CurrentID].m_Locked = false;
        Player_Manager.m_Instance.m_EquipedClothes = m_Clothes[m_CurrentID].m_ID;
        if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.BEGINNER) {
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BF", (int)m_Clothes[m_CurrentID].m_FragPrice);
        }
        else if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.MEDIOCORE) {
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("MF", (int)m_Clothes[m_CurrentID].m_FragPrice);
        }
        else if (m_Clothes[m_CurrentID].m_Type == Enumerator.FRAGMENT_TYPE.ADVANCE) {
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("AF", (int)m_Clothes[m_CurrentID].m_FragPrice);
        }
        else {
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("MA", (int)m_Clothes[m_CurrentID].m_FragPrice);
        }
        Audio_Manager.m_Instance.f_PlayOneShot(m_BoughtClip);
        Audio_Manager.m_Instance.f_PlayOneShot(m_NewEQClip);
        f_OnChangeClothesID();
        f_SaveSkinData();
    }

    public void f_SaveSkinData() {
        PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("EquippedSkin", Player_Manager.m_Instance.m_EquipedClothes.ToString());
        string t_Key = "";
        for (int i = 0; i < m_Clothes.Count; i++) {
            if (m_Clothes[i].m_Locked) t_Key += "0";
            else t_Key += "1";
        }
        PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("SkinList", t_Key);
    }

    public void f_LoadSkinData(string p_Key) {
        for (int i = 0; i < p_Key.Length; i++) {
            if (p_Key[i] == '0') m_Clothes[i].m_Locked = true;
            else m_Clothes[i].m_Locked = false;
        }
    }

    public void f_LoadEquipedSkinData(string p_ID) {
        Player_Manager.m_Instance.m_EquipedClothes = int.Parse(p_ID);
        m_CurrentID = Player_Manager.m_Instance.m_EquipedClothes;
        Player_GameObject.m_Instance.m_PlayerHitClip.Clear();
        Player_GameObject.m_Instance.m_PlayerHitClip.AddRange(m_Clothes[m_CurrentID].m_HitAudio);
        Player_GameObject.m_Instance.m_IsCrimson = m_Clothes[m_CurrentID].m_IsCrimson;
        Player_GameObject.m_Instance.m_IsAndroid = m_Clothes[m_CurrentID].m_IsAndroid;
        Player_GameObject.m_Instance.m_IsGrandMaster = m_Clothes[m_CurrentID].m_IsGrandMaster;
        Player_GameObject.m_Instance.m_MaxHit = m_Clothes[m_CurrentID].m_MaxHit;
        f_OnChangeClothesID();
    }
}
