using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enumerator;

public class Enemy_GameObject : Character_GameObject {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====

    //===== PUBLIC =====
    public ENEMY_TYPE m_Type;
    public int m_SpawnPositionIndex;
    public List<Transform> m_ListGrids;
    public float m_Speed;
    public float m_ScoreValue;
    public SpriteRenderer m_Sp;
    public float m_MoveDuration = 5f;
    public bool m_Right = false;
    public float m_KnockbackSpeed = 1f;
    public float m_ShakeSpeed = 1.0f;
    public float m_AmountShake = 1.0f;
    //===== PRIVATES =====
    Vector3 t_Target;
    Vector3 t_Vector;
    Color t_Col;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){

    }

    void Start(){
        
    }

    void Update(){
        if (Vector3.Distance(transform.position, t_Target) > .1f) {
            transform.position = Vector3.MoveTowards(transform.position, t_Target, m_MoveDuration*Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D p_Collision) {
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Timer(float p_Timer,float p_MaxTimer) {
        t_Col.r = 1;
        t_Col.g = (p_Timer / p_MaxTimer) * 1;
        t_Col.b = (p_Timer / p_MaxTimer) * 1;
        t_Col.a = 1;
        m_Sp.color = t_Col;
        t_Vector = m_Sp.transform.localPosition;
        t_Vector.x = (Mathf.Sin(Time.time * m_ShakeSpeed) * m_AmountShake) *((p_MaxTimer- p_Timer)/p_MaxTimer);
        m_Sp.transform.localPosition = t_Vector;
    }

    public void f_Knockback() {
        if (m_Right) {
            transform.Translate(-Vector3.right *m_KnockbackSpeed*Time.deltaTime*Player_GameObject.m_Instance.m_Combo/5);
        }
        else {
            transform.Translate(Vector3.right * m_KnockbackSpeed * Time.deltaTime * Player_GameObject.m_Instance.m_Combo / 5);
        }
    }

    public void f_Init() {
        m_Sp.transform.localPosition = Vector3.zero;
        m_Sp.color = Color.white;
        if (transform.position.x < Player_GameObject.m_Instance.transform.position.x) {
            f_Flip(false);
            m_Right = false;
        }
        else {
            f_Flip(true);
            m_Right = true;
        }
    }

    public void f_LineAssigned() {
        t_Target = transform.position;
    }

    public override void f_Flip(bool p_Right) {
        if (p_Right) {
            m_Sp.flipX = false;
        }
        else {
            m_Sp.flipX = true;
        }
    }

    public void f_getHit() {
        gameObject.SetActive(false);
    }

    public override void f_Move() {
        m_SpawnPositionIndex--;
        t_Target = m_ListGrids[m_SpawnPositionIndex].position;
        //transform.position = m_ListGrids[m_SpawnPositionIndex].position;
    }
  

}
