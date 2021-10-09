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
    public List<PowerupCheckmark_Gameobject> m_Potions;
    public List<Buff_GameObject> m_ListPotions;
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
            if (m_Potions[i].m_BuffType.m_Applied) {
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
    public void f_Buy() {
        Player_Manager.m_Instance.m_Berry -= (int)m_ChoosenBuff.f_GetPrice();
        if (m_ChoosenBuff.f_IsPotion()) {
            m_ChoosenBuff.m_Applied = true;
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
                if (m_ChoosenBuff.m_Applied) m_BuyButton.interactable = false;
                else m_BuyButton.interactable = true;
            }
            else {
                m_UpgradeText.text = "UPGRADE";
                m_LevelObject.SetActive(true);
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
            m_Price.gameObject.SetActive(false); m_BuyButton.interactable = false;
        }
    }

    public void f_ChoosePowerUp(Buff_GameObject p_Buff) {
        m_ChoosenBuff = p_Buff;
        f_OnChangePowerup();
    }
}
