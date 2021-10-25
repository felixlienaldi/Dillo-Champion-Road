using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerupUI_Manager : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PowerupUI_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public TextMeshProUGUI m_Price;
    public TextMeshProUGUI m_Names;
    public TextMeshProUGUI m_Desc;
    public TextMeshProUGUI m_LevelText;
    public TextMeshProUGUI m_UpgradeText;
    public Image m_Icon;
    public GameObject m_LevelObject;
    public Button m_BuyButton;
    public Button m_WearButton;
    public Button m_UnwearButton;
    public List<PowerupCheckmark_Gameobject> m_Potions;
    public List<Buff_GameObject> m_ListPotions;
    public Buff_GameObject m_ScoreMultiplierBuff;
    public Buff_GameObject m_HpGainUpBuff;
    public Buff_GameObject m_FeverTimeBuff;
    public Buff_GameObject m_DefaultPowerup;
    //===== PRIVATES =====
    private Buff_GameObject m_ChoosenBuff;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake() {
        m_Instance = this;
    }

    void Start() {
        f_ChoosePowerUp(m_DefaultPowerup);
    }

    public void f_OnEnables() {
        f_ChoosePowerUp(m_DefaultPowerup);
        for (int i = 0; i < m_Potions.Count; i++) {
            if (m_Potions[i].m_BuffType.m_Applied && m_Potions[i].m_BuffType.m_Bought) {
                m_Potions[i].m_CheckMark.SetActive(true);
            }
            else {
                m_Potions[i].m_CheckMark.SetActive(false);
            }
        }
    }

    void Update() {

    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Wear() {
        for (int i = 0; i < m_Potions.Count; i++) {
            if (m_Potions[i].m_BuffType.m_UpgradeType == m_ChoosenBuff.m_UpgradeType) {
                m_Potions[i].m_CheckMark.SetActive(true);
            }
        }
        m_ChoosenBuff.m_Applied = true;
        GameManager_Manager.m_Instance.m_ListPotion.Add(m_ChoosenBuff);
        f_OnChangePowerup();
        GameManager_Manager.m_Instance.f_ApplyPotion();
        f_SaveDataPotion();
    }

    public void f_Unapply() {
        for (int i = 0; i < m_Potions.Count; i++) {
            if (m_Potions[i].m_BuffType.m_UpgradeType == m_ChoosenBuff.m_UpgradeType) {
                m_Potions[i].m_CheckMark.SetActive(false);
            }
        }
        m_ChoosenBuff.m_Applied = false;
        GameManager_Manager.m_Instance.m_ListPotion.Remove(m_ChoosenBuff);
        f_OnChangePowerup();
        GameManager_Manager.m_Instance.f_ApplyPotion();
        f_SaveDataPotion();
    }

    public void f_Buy() {
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE",(int)m_ChoosenBuff.f_GetPrice());
        if (m_ChoosenBuff.f_IsPotion()) {
            m_ChoosenBuff.m_Applied = true;
            m_ChoosenBuff.m_Bought = true;
            for (int i = 0; i < m_Potions.Count; i++) {
                if (m_Potions[i].m_BuffType.m_UpgradeType == m_ChoosenBuff.m_UpgradeType) {
                    m_Potions[i].m_CheckMark.SetActive(true);
                }
            }
            GameManager_Manager.m_Instance.m_ListPotion.Add(m_ChoosenBuff);
        }
        else {
            m_ChoosenBuff.f_SetLevel(m_ChoosenBuff.m_Level + 1);
        }
        f_SaveDataPotion();
        f_SaveDataBuff();
        f_OnChangePowerup();
    }

    public void f_ResetPot() {
        for (int i = 0; i < m_ListPotions.Count; i++) {
            m_ListPotions[i].m_Applied = false;
        }
    }

    public void f_OnChangePowerup() {
        m_Names.text = m_ChoosenBuff.m_Names;
        m_Icon.sprite = m_ChoosenBuff.m_BuffSprite;
        m_LevelText.text = (m_ChoosenBuff.m_Level + 1).ToString("00");
        m_Desc.text = m_ChoosenBuff.f_GetDesc();

        if ((m_ChoosenBuff.m_Level != m_ChoosenBuff.m_MaxLevel && !m_ChoosenBuff.f_IsPotion()) || (m_ChoosenBuff.f_IsPotion())) {
            m_Price.gameObject.SetActive(true);
            m_Price.text = m_ChoosenBuff.f_GetPrice().ToString();

            if (m_ChoosenBuff.f_IsPotion()) {
                m_LevelObject.SetActive(false);
                m_UpgradeText.text = "BUY";
                if (m_ChoosenBuff.m_Bought) {
                    m_WearButton.gameObject.SetActive(true);
                    if (m_ChoosenBuff.m_Applied) {
                        m_WearButton.interactable = false;
                        m_UnwearButton.gameObject.SetActive(true);
                    }
                    else {
                        m_WearButton.interactable = true;
                        m_UnwearButton.gameObject.SetActive(false);
                    }
                    m_BuyButton.gameObject.SetActive(false);
                    m_BuyButton.interactable = false;
                }
                else {
                    m_WearButton.gameObject.SetActive(false);
                    m_BuyButton.gameObject.SetActive(true);
                    m_BuyButton.interactable = true;
                }
            }
            else {
                m_WearButton.gameObject.SetActive(false);
                m_UpgradeText.text = "UPGRADE";
                m_LevelObject.SetActive(true);
                m_BuyButton.gameObject.SetActive(true);
                m_BuyButton.interactable = true;
            }

            if (m_ChoosenBuff.f_GetPrice() > Player_Manager.m_Instance.m_Berry) {
                m_Price.color = Color.red;
                m_BuyButton.interactable = false;
            }
            else {
                m_Price.color = Color.white;
            }
        }
        else {
            m_UpgradeText.text = "UPGRADE";
            m_Price.gameObject.SetActive(false);
            m_BuyButton.interactable = false;
        }
    }

    public void f_ChoosePowerUp(Buff_GameObject p_Buff) {
        m_ChoosenBuff = p_Buff;
        f_OnChangePowerup();
    }

    public void f_SaveDataBuff() {
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("FeverGain",m_FeverTimeBuff.m_Level);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("HpGainUp",m_HpGainUpBuff.m_Level);
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("ScoreMultiplier",m_ScoreMultiplierBuff.m_Level);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
    }

    public void f_SaveDataPotion() {
        for (int i = 0; i < m_Potions.Count; i++) {
            string t_KeyData = "";

            if (m_Potions[i].m_BuffType.m_Bought) t_KeyData += "1";
            else t_KeyData += "0";

            if (m_Potions[i].m_BuffType.m_Applied) t_KeyData += "1";
            else t_KeyData += "0";

            if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.ACCURACY) PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("ACCURACY", t_KeyData);
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.FEVERGAIN) PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("FEVERGAIN", t_KeyData);
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.BARRIER) PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("BARRIER", t_KeyData);
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.REVIVE) PlayerData_Manager.m_Instance.f_UpdatePlayerAvatarList("REVIVE", t_KeyData);
        }
    }

    public void f_LoadDataPotion(string p_Type,string p_Key) {
        for (int i = 0; i < m_Potions.Count; i++) {
            if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.ACCURACY && p_Type == "ACCURACY") {
                if (p_Key[0] == '1') {
                    m_Potions[i].m_BuffType.m_Bought = true;
                }
                else {
                    m_Potions[i].m_BuffType.m_Bought = false;
                }

                if (p_Key[1] == '1') {
                    m_Potions[i].m_BuffType.m_Applied = true;
                    GameManager_Manager.m_Instance.m_ListPotion.Add(m_Potions[i].m_BuffType);
                    m_Potions[i].m_CheckMark.SetActive(true);
                }
                else {
                    m_Potions[i].m_BuffType.m_Applied = false;
                    m_Potions[i].m_CheckMark.SetActive(false);
                }
            }
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.FEVERGAIN && p_Type == "FEVERGAIN") {
                if (p_Key[0] == '1') {
                    m_Potions[i].m_BuffType.m_Bought = true;
                }
                else {
                    m_Potions[i].m_BuffType.m_Bought = false;
                }

                if (p_Key[1] == '1') {
                    m_Potions[i].m_BuffType.m_Applied = true;
                    GameManager_Manager.m_Instance.m_ListPotion.Add(m_Potions[i].m_BuffType);
                    m_Potions[i].m_CheckMark.SetActive(true);

                }
                else {
                    m_Potions[i].m_BuffType.m_Applied = false;
                    m_Potions[i].m_CheckMark.SetActive(false);
                }
            }
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.BARRIER && p_Type == "BARRIER") {
                if (p_Key[0] == '1') {
                    m_Potions[i].m_BuffType.m_Bought = true;
                }
                else {
                    m_Potions[i].m_BuffType.m_Bought = false;
                }

                if (p_Key[1] == '1') {
                    m_Potions[i].m_BuffType.m_Applied = true;
                    GameManager_Manager.m_Instance.m_ListPotion.Add(m_Potions[i].m_BuffType);
                    m_Potions[i].m_CheckMark.SetActive(true);
                }
                else {
                    m_Potions[i].m_BuffType.m_Applied = false;
                    m_Potions[i].m_CheckMark.SetActive(false);
                }
            }
            else if (m_Potions[i].m_BuffType.m_UpgradeType == Enumerator.UPGRADE_TYPE.REVIVE && p_Type == "REVIVE") {
                if (p_Key[0] == '1') {
                    m_Potions[i].m_BuffType.m_Bought = true;
                }
                else {
                    m_Potions[i].m_BuffType.m_Bought = false;
                }

                if (p_Key[1] == '1') {
                    m_Potions[i].m_BuffType.m_Applied = true;
                    GameManager_Manager.m_Instance.m_ListPotion.Add(m_Potions[i].m_BuffType);
                    m_Potions[i].m_CheckMark.SetActive(true);
                }
                else {
                    m_Potions[i].m_BuffType.m_Applied = false;
                    m_Potions[i].m_CheckMark.SetActive(false);
                }
            }
        }
    }
}
