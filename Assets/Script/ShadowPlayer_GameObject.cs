using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShadowPlayer_GameObject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static ShadowPlayer_GameObject m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public string m_AttackAnimation;
    public Animator m_Animator;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void OnEnable (){
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Attack(float p_EnemyTransformX) {
        if(p_EnemyTransformX - transform.position.x >= 0) {
            transform.localScale = new Vector3(0.4f, transform.localScale.y, transform.localScale.z);
        }
        else {
            transform.localScale = new Vector3(-0.4f, transform.localScale.y, transform.localScale.z);
        }
        gameObject.SetActive(true);
       
    }

    public void f_CheckTiming() {
        Player_GameObject.m_Instance.f_CheckTiming(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
    }
    public void f_SetActive() {
        gameObject.SetActive(false);
    }
}
