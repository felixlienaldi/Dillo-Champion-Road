using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Fragment_Manager : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Fragment_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    [Header("Variable")]
    public int m_BeginnerFragment = 0;
    public int m_MediocoreFragment = 0;
    public int m_AdvanceFragment = 0;
    public int m_MasterFragment = 0;
    public int m_GachaTicket = 5;
    public int m_Source = 0;
    public int m_Target = 0;
    public int m_ConverTotal = 0;
    public float m_BeginnerPercentage = 70f;
    public float m_MediocorePercentage = 25f;
    public float m_AdvancePercentage = 10f;
    public float m_MasterPercentage = 5f;
    public bool m_DailyShopAvailable = true;
    public bool m_DailyBoxAvailable = false;
    public int m_DailyShopFrags = 0;
    [Header("UI")]
    public Slider m_Slider;
    public Image m_SourceFragmentImage;
    public Image m_TargetFragmentImage;
    public TextMeshProUGUI m_SourceTextAmount;
    public TextMeshProUGUI m_TargetTextAmount;
    public TextMeshProUGUI m_SliderValueText;
    public Button m_ConvertButton;
    public List<Sprite> m_ImageList;
    public List<Color> m_Color;
    public Button m_GachaButton;
    public Button m_BuyButton;
    public Image m_DailyShopFragsImage;
    public GameObject m_DailyButton;
    public GameObject m_OutOfStockDailyButton;
    public GameObject m_GachaButtonObj;
    public GameObject m_OutOfStockGachaButton;
    public TextMeshProUGUI m_DailyShopPriceText;
    public TextMeshProUGUI m_GachaShopPriceText;
    public TextMeshProUGUI m_GachaTicketText;
    public Button m_EnergyBuyButton;
    public TextMeshProUGUI m_EnergyBuyButtonText;
    public List<GameObject> m_MenuList;
    public GameObject m_BoxButton;
    //===== PRIVATES =====
    float t_RandomIndex;
    float t_MediocorePercentage = 0;
    float t_AdvancePercentage = 0;
    int m_CurrentID = 0;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake() {
        m_Instance = this;
    }

    void Start() {
        t_AdvancePercentage = m_MasterPercentage + m_AdvancePercentage;
        t_MediocorePercentage = m_MediocorePercentage + t_AdvancePercentage;
    }

    void Update() {
        m_SourceFragmentImage.sprite = m_ImageList[m_Source];
        m_TargetFragmentImage.sprite = m_ImageList[m_Target];
        m_SourceTextAmount.color = m_Color[m_Source];
        m_TargetTextAmount.color = m_Color[m_Source];
        m_DailyShopFragsImage.sprite = m_ImageList[m_DailyShopFrags];

        f_CheckDailyShop();

        if (m_DailyShopAvailable) {
            if (!m_DailyButton.activeSelf) m_DailyButton.SetActive(true);
            if (m_OutOfStockDailyButton.activeSelf) m_OutOfStockDailyButton.SetActive(false);
        }
        else {
            if (m_DailyButton.activeSelf) m_DailyButton.SetActive(false);
            if (!m_OutOfStockDailyButton.activeSelf) m_OutOfStockDailyButton.SetActive(true);
        }

        f_CheckGacha();

        if (m_GachaTicket > 0) {
            if (!m_GachaButtonObj.activeSelf) m_GachaButtonObj.SetActive(true);
            if (m_OutOfStockGachaButton.activeSelf) m_OutOfStockGachaButton.SetActive(false);
        }
        else {
            if (m_GachaButtonObj.activeSelf) m_GachaButtonObj.SetActive(false);
            if (!m_OutOfStockGachaButton.activeSelf) m_OutOfStockGachaButton.SetActive(true);
        }

        m_GachaTicketText.text = "Available " + m_GachaTicket.ToString() + "/5";
        f_CheckEnergyButton();

        if (m_DailyBoxAvailable) {
            if (!m_BoxButton.activeSelf) m_BoxButton.gameObject.SetActive(true);
        }
        else {
            if (m_BoxButton.activeSelf) m_BoxButton.gameObject.SetActive(false);
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_OpenMenu() {
        Timer_Manager.m_Instance.f_GetTimeServer();
    }
    public void f_PreviousMenu() {
        m_CurrentID--;
        if (m_CurrentID < 0) m_CurrentID = m_MenuList.Count - 1;
        for (int i = 0; i < m_MenuList.Count; i++) {
            if (i == m_CurrentID) {
                m_MenuList[i].gameObject.SetActive(true);
            }
            else {
                m_MenuList[i].gameObject.SetActive(false);
            }
        }
    }
    public void f_NextMenu() {
        m_CurrentID++;
        if (m_CurrentID >= m_MenuList.Count) m_CurrentID = 0;
        for (int i = 0; i < m_MenuList.Count; i++) {
            if (i == m_CurrentID) {
                m_MenuList[i].gameObject.SetActive(true);
            }
            else {
                m_MenuList[i].gameObject.SetActive(false);
            }
        }
    }
    public void f_SetSource(int p_ID) {
        m_Source = p_ID;
    }
    public void f_SetTarget(int p_ID) {
        m_Target = p_ID;
    }
    public void f_CalculateMaxValue() {
        if (m_Source == 0 && m_Target == 1) {
            m_Slider.maxValue = Mathf.FloorToInt(m_BeginnerFragment / 2);
        }
        else if (m_Source == 1 && m_Target == 2) {
            m_Slider.maxValue = Mathf.FloorToInt(m_MediocoreFragment / 5);
        }
        else if (m_Source == 2 && m_Target == 3) {
            m_Slider.maxValue = Mathf.FloorToInt(m_AdvanceFragment / 10);
        }
        else if (m_Source == 3 && m_Target == 2) {
            m_Slider.maxValue = m_MasterFragment;
        }
        else if (m_Source == 2 && m_Target == 1) {
            m_Slider.maxValue = m_AdvanceFragment;
        }
        else if (m_Source == 1 && m_Target == 0) {
            m_Slider.maxValue = m_MediocoreFragment;
        }

        f_Convert(0);
    }
    public void f_Convert(float p_SliderValue) {
        m_ConvertButton.interactable = (p_SliderValue > 0);
        if (m_Source == 0 && m_Target == 1) {
            m_SourceTextAmount.text = (p_SliderValue * 2).ToString();
            m_TargetTextAmount.text = p_SliderValue.ToString();
            m_SliderValueText.text = p_SliderValue.ToString();
        }
        else if (m_Source == 1 && m_Target == 2) {
            m_SourceTextAmount.text = (p_SliderValue * 5).ToString();
            m_TargetTextAmount.text = p_SliderValue.ToString();
            m_SliderValueText.text = p_SliderValue.ToString();
        }
        else if (m_Source == 2 && m_Target == 3) {
            m_SourceTextAmount.text = (p_SliderValue * 10).ToString();
            m_TargetTextAmount.text = p_SliderValue.ToString();
            m_SliderValueText.text = p_SliderValue.ToString();
        }
        else if (m_Source == 3 && m_Target == 2) {
            m_SourceTextAmount.text = p_SliderValue.ToString();
            m_TargetTextAmount.text = (p_SliderValue * 10).ToString();
            m_SliderValueText.text = (p_SliderValue * 10).ToString();
        }
        else if (m_Source == 2 && m_Target == 1) {
            m_SourceTextAmount.text = p_SliderValue.ToString();
            m_TargetTextAmount.text = (p_SliderValue * 5).ToString();
            m_SliderValueText.text = (p_SliderValue * 5).ToString();
        }
        else if (m_Source == 1 && m_Target == 0) {
            m_SourceTextAmount.text = p_SliderValue.ToString();
            m_TargetTextAmount.text = (p_SliderValue * 2).ToString();
            m_SliderValueText.text = (p_SliderValue * 2).ToString();
        }
    }
    public void f_ConfirmConvert() {
        if (m_Source == 0 && m_Target == 1) {
            m_BeginnerFragment -= Mathf.RoundToInt(m_Slider.value * 2);
            m_MediocoreFragment += (int)m_Slider.value;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BF", Mathf.RoundToInt(m_Slider.value * 2));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MF", (int)m_Slider.value);
            PopupReward_Manager.m_Instance.f_ShowPopup(3,(int)m_Slider.value);
        }
        else if (m_Source == 1 && m_Target == 2) {
            m_MediocoreFragment -= Mathf.RoundToInt(m_Slider.value * 5);
            m_AdvanceFragment += (int)m_Slider.value;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("MF", Mathf.RoundToInt(m_Slider.value * 5));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("AF", (int)m_Slider.value);
            PopupReward_Manager.m_Instance.f_ShowPopup(4, (int)m_Slider.value);
        }
        else if (m_Source == 2 && m_Target == 3) {
            m_AdvanceFragment -= Mathf.RoundToInt(m_Slider.value * 10);
            m_MasterFragment += (int)m_Slider.value;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("AF", Mathf.RoundToInt(m_Slider.value * 10));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MA", (int)m_Slider.value);
            PopupReward_Manager.m_Instance.f_ShowPopup(5, (int)m_Slider.value);
        }
        else if (m_Source == 3 && m_Target == 2) {
            m_MasterFragment -= Mathf.RoundToInt(m_Slider.value);
            m_AdvanceFragment += (int)m_Slider.value * 10;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("MA", Mathf.RoundToInt(m_Slider.value));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("AF", (int)m_Slider.value*10);
            PopupReward_Manager.m_Instance.f_ShowPopup(4, (int)m_Slider.value*10);
        }
        else if (m_Source == 2 && m_Target == 1) {
            m_AdvanceFragment -= Mathf.RoundToInt(m_Slider.value);
            m_MediocoreFragment += (int)m_Slider.value * 5;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("AF", Mathf.RoundToInt(m_Slider.value));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MF", (int)m_Slider.value * 5);
            PopupReward_Manager.m_Instance.f_ShowPopup(3, (int)m_Slider.value*5);

        }
        else if (m_Source == 1 && m_Target == 0) {
            m_MediocoreFragment -= Mathf.RoundToInt(m_Slider.value);
            m_BeginnerFragment += (int)m_Slider.value * 2;
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("MF", Mathf.RoundToInt(m_Slider.value));
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BF", (int)m_Slider.value * 2);
            PopupReward_Manager.m_Instance.f_ShowPopup(2, (int)m_Slider.value*2);
        }
    }
     public void f_CheckGacha(){
        if (Player_Manager.m_Instance.m_Berry >= 20) {
            m_GachaButton.interactable = true;
            m_GachaShopPriceText.color = Color.white;
        }
        else {
            m_GachaButton.interactable = false;
            m_GachaShopPriceText.color = Color.red;
        }
    }
    public void f_Gacha() { 
        m_GachaTicket--;
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("GachaTicket", m_GachaTicket);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 20);
        f_CalculateGacha();
        f_CheckGacha();
    }
    public void f_CalculateGacha() {
        t_RandomIndex = Random.Range(0f, 100f);
        if (t_RandomIndex <= m_MasterPercentage) {
            //Return Master Fragment
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MA", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(5, 1);
        }
        else if (t_RandomIndex > m_MasterPercentage && t_RandomIndex <= t_AdvancePercentage) {
            //Return Advance Fragment
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("AF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(4, 1);
        }
        else if (t_RandomIndex > t_AdvancePercentage && t_RandomIndex <= t_MediocorePercentage) {
            //Return Mediocore Fragment
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(3, 1);
        }
        else {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(2, 1);
            //Return Beginner Fragment
        }
    }
    public void f_CheckDailyShop() {
        if (m_DailyShopFrags == 0) {
            if (Player_Manager.m_Instance.m_Berry >= 20) {
                m_BuyButton.interactable = true;
                m_DailyShopPriceText.color = Color.white;
            }
            else {
                m_BuyButton.interactable = false;
                m_DailyShopPriceText.color = Color.red;
            }
        }
        else if (m_DailyShopFrags == 1) {
            if (Player_Manager.m_Instance.m_Berry >= 50) {
                m_BuyButton.interactable = true;
                m_DailyShopPriceText.color = Color.white;
            }
            else {
                m_BuyButton.interactable = false;
                m_DailyShopPriceText.color = Color.red;
            }
        }
        else if (m_DailyShopFrags == 2) {
            if (Player_Manager.m_Instance.m_Berry >= 75) {
                m_BuyButton.interactable = true;
                m_DailyShopPriceText.color = Color.white;
            }
            else {
                m_BuyButton.interactable = false;
                m_DailyShopPriceText.color = Color.red;
            }
        }
        else if (m_DailyShopFrags == 3) {
            if (Player_Manager.m_Instance.m_Berry >= 150) {
                m_BuyButton.interactable = true;
                m_DailyShopPriceText.color = Color.white;
            }
            else {
                m_BuyButton.interactable = false;
                m_DailyShopPriceText.color = Color.red;
            }
        }


        else {
            m_GachaButton.interactable = false;
            m_DailyShopPriceText.color = Color.red;
        }
    }
    public void f_BuyDailyShop() {
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("ShopBought", 1);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
        if (m_DailyShopFrags == 0) {
            //Add VC Beginner.
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 20);
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(2,1);
        }
        else if (m_DailyShopFrags == 1) {
            //Add VC Mediocore
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 50);
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(3,1);
        }
        else if (m_DailyShopFrags == 2) {
            //Add VC Advance
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 75);
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("AF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(4,1);
        }
        else if (m_DailyShopFrags == 3) {
            //Add VC Master
            CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 150);
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MA", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(5,1);
        }
    }
    public void f_CheckEnergyButton() {
        if (Player_Manager.m_Instance.m_Berry >= 20) {
            if (Energy_Manager.m_Instance.m_EnergyAmount <= 5) m_EnergyBuyButton.interactable = true;
            else m_EnergyBuyButton.interactable = false;
            m_EnergyBuyButtonText.color = Color.white;
        }
        else {
            m_EnergyBuyButton.interactable = false;
            m_EnergyBuyButtonText.color = Color.red;
        }
    }
    public void f_BuyEnergy() {
        CurrencyManager_Manager.m_Instance.f_RemoveVirtualCurrencyRequest("BE", 20);
        CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("EN",5);
        PopupReward_Manager.m_Instance.f_ShowPopup(1,5);
    }

    public void f_GetDailyBox() {
        t_RandomIndex = Random.Range(0f, 100f);
        if (t_RandomIndex > 98) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MA", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(5, 1);
        }
        else if (t_RandomIndex > 95) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("AF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(4, 1);
        }
        else if (t_RandomIndex > 90) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("MF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(3, 1);
        }
        else if (t_RandomIndex > 80) {
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BF", 1);
            PopupReward_Manager.m_Instance.f_ShowPopup(2, 1);
        }
        else {
            int t_Int;
            if (Player_Manager.m_Instance.m_HighScore>0) t_Int = Mathf.CeilToInt(Player_Manager.m_Instance.m_HighScore * .2f);
            else t_Int = 1;
            CurrencyManager_Manager.m_Instance.f_AddVirtualCurrencyRequest("BE",t_Int);
            PopupReward_Manager.m_Instance.f_ShowPopup(0, t_Int);
        }
        PlayerStatistic_Manager.m_Instance.f_UpdateStatistics("DailyBox", 1);
        PlayerStatistic_Manager.m_Instance.f_UpdatePlayerStatistics();
    }
}
