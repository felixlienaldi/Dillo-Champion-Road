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
    public TextMeshProUGUI m_ComboCount;
    public TextMeshProUGUI m_TextImage;
    public TextMeshProUGUI m_Score;
    public Image[] m_HPIcon;
    public Image m_HpAdded;
    public Image m_TimerFillBar;
    public Image m_TimerFillBG;
    public GameObject m_Position1;
    public GameObject m_Position2;
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

    public void f_SetComboText(string p_Text) {
        m_ComboCount.text = p_Text;
    }

    public void f_SetFeverFillBar(float p_FillAmount, float p_MaxAmount) {
        m_FeverFillBar.fillAmount = p_FillAmount / p_MaxAmount;
    }

    public void f_SetTimerFillBar(float p_FillAmount, float p_MaxAmount, Enemy_GameObject p_Enemy) {
        m_TimerFillBar.fillAmount = p_FillAmount / p_MaxAmount;
        if (p_Enemy.m_ListGrids == GameManager_Manager.m_Instance.m_LeftGrids) {
            m_TimerFillBar.transform.position = m_Position1.transform.position;
            m_TimerFillBG.transform.position = m_Position1.transform.position;
        }
        else {
            m_TimerFillBar.transform.position = m_Position2.transform.position;
            m_TimerFillBG.transform.position = m_Position2.transform.position;
        }
    }

    public void f_SetHpBar(float p_Health) {
        for(int i = 0; i < m_HPIcon.Length; i++) {
            if(i < p_Health) m_HPIcon[i].gameObject.SetActive(true);
            else m_HPIcon[i].gameObject.SetActive(false);
        }
    }
    public void f_SetScoreText(string p_Score) {
        m_Score.text = "Score : " + p_Score;
    }
    public void f_SetPopUpHpBar() {
        m_HpAdded.gameObject.SetActive(false);
        m_HpAdded.gameObject.SetActive(true);
    }

    public IEnumerator<float> f_SetAccuracyText(string p_Text) {
        m_TextImage.text = p_Text;
        yield return Timing.WaitForSeconds(0.75f);
        m_TextImage.text = "";
    }
}
