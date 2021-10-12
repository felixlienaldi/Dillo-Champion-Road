using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
public class UIManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static UIManager_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public Image m_FeverFillBar;
    public Image m_NormalFillBar;
    public TextMeshProUGUI m_ComboCount;
    public TextMeshProUGUI m_TextImage;
    public TextMeshProUGUI m_Score;
    public Animator[] m_HPIcon;
    public Sprite[] m_TimerType; //0 = green, 1= yellow,2 = red
    public Image m_TimerFillBar;
    public Image m_ContinueTimerFill;
    public GameObject m_TimerFill;
    public GameObject m_Position1;
    public GameObject m_Position2;
    public GameObject m_Combo;
    public GameObject m_NormalBar;
    public GameObject m_FeverBar;
    public Camera m_UICam;
    //===== PRIVATES =====
    Vector3 t_Vector;
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

    public void f_SetComboText(string p_Text) {
        m_ComboCount.text = p_Text;
    }

    public void f_SetFeverFillBar(float p_FillAmount, float p_MaxAmount) {
        m_FeverFillBar.fillAmount = p_FillAmount / p_MaxAmount;
        m_NormalFillBar.fillAmount = p_FillAmount / p_MaxAmount;
    }

    public void f_ChangeFever(bool p_Fever) {
        if (p_Fever) {
            m_FeverBar.gameObject.SetActive(true);
            m_NormalBar.gameObject.SetActive(false);
        }
        else {
            m_FeverBar.gameObject.SetActive(false);
            m_NormalBar.gameObject.SetActive(true);
        }
    }

    public void f_SetTimerFillBar(float p_FillAmount, float p_MaxAmount, Enemy_GameObject p_Enemy) {
        if ((p_FillAmount / p_MaxAmount) >= .67f) {
            m_TimerFillBar.sprite = m_TimerType[0];
        }
        else if ((p_FillAmount / p_MaxAmount) >= .33f) {
            m_TimerFillBar.sprite = m_TimerType[1];
        }
        else {
            m_TimerFillBar.sprite = m_TimerType[2];
        }

        m_TimerFillBar.fillAmount = p_FillAmount / p_MaxAmount;
        if (p_Enemy.m_ListGrids == GameManager_Manager.m_Instance.m_LeftGrids) {
            t_Vector = Camera.main.WorldToScreenPoint(m_Position1.transform.position);
            t_Vector = m_UICam.ScreenToWorldPoint(t_Vector);
            t_Vector.z = 100;
            m_TimerFill.transform.position = t_Vector;
        }
        else {
            t_Vector = Camera.main.WorldToScreenPoint(m_Position2.transform.position);
            t_Vector = m_UICam.ScreenToWorldPoint(t_Vector);
            t_Vector.z = 100;
            m_TimerFill.transform.position = t_Vector;
        }
    }

    public void f_SetHpBar(float p_Health) {
        for (int i = 0; i < m_HPIcon.Length; i++) {
            m_HPIcon[i].Play("Gain");
        }
    }

    public void f_SetContinueTimerFill(float m_CurrentTimer,float m_MaxTimer =5) {
        m_ContinueTimerFill.fillAmount = (m_MaxTimer-m_CurrentTimer) / m_MaxTimer; 
    }

    public void f_MinHp(float p_Health) {
        m_HPIcon[(int) p_Health].Play("Loss");
    }

    public void f_AddHP(float p_Health) {
        m_HPIcon[(int)p_Health-1].Play("Gain");
    }

    public void f_SetScoreText(string p_Score) {
        m_Score.text =  p_Score;
    }
    public void f_SetPopUpHpBar() {
        //m_HpAdded.gameObject.SetActive(false);
        //m_HpAdded.gameObject.SetActive(true);
    }

    public IEnumerator<float> f_SetAccuracyText(string p_Text) {
        //m_TextImage.text = p_Text;
        yield return Timing.WaitForSeconds(0.75f);
        //m_TextImage.text = "";
    }
}
