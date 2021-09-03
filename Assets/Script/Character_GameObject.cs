using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character_GameObject : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====

    //===== PUBLIC =====
    public Animator m_Animator;
    public BoxCollider2D m_Collider;
    public List<Buff_GameObject> m_ListBuff;

    [Header("General")]
    [SerializeField] protected float m_HealthPoint;
    [SerializeField] protected float m_Attack;
    //===== PRIVATES =====
    protected float m_CurrentHealthPoint;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Flip(bool p_Right) {
        if ((transform.localScale.x < 0 && p_Right == true) || (transform.localScale.x > 0 && p_Right == false)) {
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
    }

    public virtual void f_Move() { }

    public float f_GetCurrentHealth() {
        return m_CurrentHealthPoint;
    }
    
    public float f_GetMaxHealth() {
        return m_HealthPoint;
    }

    public void f_SetMaxHealth(float p_HealthPoint) {
        m_HealthPoint = p_HealthPoint;
    }

    public void f_TakeDamage(float p_Damage) {
        m_CurrentHealthPoint -= p_Damage;
    }

    public float f_GetAttack() {
        return m_Attack;
    }

    public void f_SetAttack(float p_Attack) {
        m_Attack = p_Attack;
    }

   
}
