using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
using UnityEngine.Events;
/*Score multiplier (2x) V
HP gain up (+1) (reduce minimum combo to obtain HP) V
Fever time up (x300%) V


Barrier (1x) V
Fever gain up (x150%) V
Accuracy (x2) (certain hit / cannot miss)  V
Revive (1x) V
*/
public class Player_GameObject : Character_GameObject{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Player_GameObject m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public SpriteRenderer m_SpriteRenderer;

    [Header("Combo System")]
    public int m_Combo;
    public bool m_IsCombo;
    public float m_MinimumCombo;

    [Header("Fever System")]
    public int m_MinimumHit;
    public float m_PerfectHit;
    public bool m_IsFever;
    public float m_FeverTimer;
    public float m_FeverTimeMultiplier;
    public float m_FeverGain;
    public float m_FeverGainMultiplier;
    public int m_EnemyKilled;
    float m_CurrentFeverTimer;

    [Header("Timing")]
    public float m_Timer;

    [Header("Animation Name")]
    public string m_AttackAnimation;

    [Header("Other System")]
    public float m_BarrierCount;
    public float m_ReviveCount;
    public float m_AbsoluteHitCount;
    public GameObject m_Barrier;
    public GameObject m_Revive;
    public ShadowPlayer_GameObject m_AbsoluteHitObject;
    public UnityEvent m_OnRevived;
    public UnityEvent<float> m_OnAbsoluteHit;

    //===== PRIVATES =====
    bool m_IsTakeDamageCoroutineRunning;
    float m_CurrentTimer;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void OnEnable(){
        m_EnemyKilled = 0;
        m_IsCombo = false;
        m_IsFever = false;
        m_CurrentHealthPoint = f_GetMaxHealth();
        m_PerfectHit = 0;
        m_Timer = 5;
        m_Combo = 0;
        m_CurrentTimer = m_Timer;
        m_SpriteRenderer.color = Color.white;
        UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
        UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
    }

    void Update(){
        if(f_GetCurrentHealth() <= 0) {
            if(m_ReviveCount > 0) {
                m_ReviveCount--;
                m_OnRevived?.Invoke();
                m_CurrentHealthPoint++;
                UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
            }
            else {
                m_FeverGainMultiplier = 1;
                m_BarrierCount = 0;
                m_ReviveCount = 0;
                m_AbsoluteHitCount = 0;
                GameManager_Manager.m_Instance.f_PostGameManager();
                m_CurrentHealthPoint = f_GetMaxHealth();
            }
        }
        f_CheckTimer();
        f_Move();
        f_CheckCombo();
        f_CheckFever();
    }

    //=====================================================================
    //				        OTHER METHOD
    //=====================================================================
    

    public float f_GetFeverTimer() {
        return m_FeverTimer * m_FeverTimeMultiplier;
    }

    public float f_GainFever() {
        return m_FeverGain * m_FeverGainMultiplier;
    }

    public IEnumerator<float> ie_TakeDamage() {
        m_IsTakeDamageCoroutineRunning = true;
        m_SpriteRenderer.color = Color.red;
        yield return Timing.WaitForSeconds(0.2f);
        m_IsTakeDamageCoroutineRunning = false;
        m_SpriteRenderer.color = Color.white;
    }

    public void f_CheckCombo() {
        if (m_Combo >= 3) {
            m_IsCombo = true;
        }
        else {
            m_IsCombo = false;
        }
        if (m_IsCombo) UIManager_Manager.m_Instance.f_SetComboText("x" + m_Combo);
        else UIManager_Manager.m_Instance.f_SetComboText("");
    }

    public void f_CheckFever() {
        if (m_IsFever) {
            m_SpriteRenderer.color = Color.blue;
            m_CurrentFeverTimer -= Time.deltaTime;
            if(m_CurrentFeverTimer<= 0f) {
                m_CurrentFeverTimer = 0f;
                m_IsFever = false;
                m_SpriteRenderer.color = Color.white;
            }
            UIManager_Manager.m_Instance.f_SetFeverFillBar(m_CurrentFeverTimer, f_GetFeverTimer());
        }

    }

    public void f_CheckTimer() {
        m_CurrentTimer -= Time.deltaTime;
        if(GameManager_Manager.m_Instance.m_ListActiveEnemies.Count > 0) UIManager_Manager.m_Instance.f_SetTimerFillBar(m_CurrentTimer, m_Timer, GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
        if (m_CurrentTimer <= 0) {
            if(m_BarrierCount > 0) {
                m_BarrierCount--;
            }
            else {
                f_TakeDamage();
            }
            m_CurrentTimer = m_Timer;
        }
        if (m_BarrierCount > 0) m_Barrier.SetActive(true);
        else m_Barrier.SetActive(false);

    }

  
    public void f_CheckTiming(Enemy_GameObject p_EnemyObject) {
        m_Combo++;
        if (!m_IsFever) {
            m_PerfectHit += f_GainFever();
        }

        if (m_Combo % m_MinimumCombo == 0) {
            if (f_GetCurrentHealth() < f_GetMaxHealth()) {
                m_CurrentHealthPoint++;
                UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
            }
        }

        if (!m_IsFever) {
            m_EnemyKilled++;
            if ((int)m_EnemyKilled % 10 == 0) {
                if (m_Timer > 0.5f) m_Timer -= 0.25f;
            }

            UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
            if (m_PerfectHit >= m_MinimumHit) {
                m_PerfectHit = 0;
                m_IsFever = true;
                m_CurrentFeverTimer = f_GetFeverTimer();
            }
            Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 1f));
        }
        else {
            Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 5f));
        }
       
        GameManager_Manager.m_Instance.f_AddScore(p_EnemyObject.m_ScoreValue);
        GameManager_Manager.m_Instance.f_NextLine(p_EnemyObject);
    }

    public void f_Attack() {
        m_Animator.Play(m_AttackAnimation);
    }

    public void f_TakeDamage() {
        m_Combo = 0;
        m_PerfectHit = 0;
        Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 3f));
        UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
        f_TakeDamage(1);
        if (!m_IsTakeDamageCoroutineRunning) Timing.RunCoroutine(ie_TakeDamage());
        UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
        GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
    }

    public void f_OnAbsoluteHit(float p_TransformScaleX) {
        m_AbsoluteHitObject.f_Attack(GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x);
      
    }

    public void f_OnRevived() {
        Timing.RunCoroutine(ie_Revive());
    }

    public IEnumerator<float> ie_Revive() {
        m_Revive.gameObject.SetActive(true);
        float m_InvisTimer = 0;
        Debug.Log("TEST");
        do {
            m_SpriteRenderer.color = new Color(1, 1, 1, 0);
            yield return Timing.WaitForSeconds(0.05f);
            m_SpriteRenderer.color = new Color(1, 1, 1, 1);
            yield return Timing.WaitForSeconds(0.05f);
            m_InvisTimer += 0.2f;
        } while (m_InvisTimer < 1f);
        m_SpriteRenderer.color = new Color(1, 1, 1, 1);
        yield return Timing.WaitForSeconds(1f);
        m_Revive.gameObject.SetActive(false);
    }

    public override void f_Move() {
    }

    public void f_SetColliderAttack() {
        f_CheckTiming(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
    }

    public void f_Attack(bool p_Right) {
        m_CurrentTimer = m_Timer;
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].m_Type == Enumerator.ENEMY_TYPE.INVERSE) {
                if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x - transform.position.x >= 0) {
                    if (p_Right == false) {
                        f_Flip(!p_Right);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(true);
                                f_Attack();
                            }
                           
                        }
                        else f_TakeDamage();
                    }
                }
                else {
                    if (p_Right == true) {
                        f_Flip(!p_Right);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(false);
                                f_Attack();
                            }
                        }
                        else {
                            f_TakeDamage();
                        }
                       
                    }
                }
            }
            else if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].m_Type == Enumerator.ENEMY_TYPE.NORMAL) {
                if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x - transform.position.x >= 0) {
                    if (p_Right == true) {
                        f_Flip(p_Right);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(true);
                                f_Attack();
                            }
                           
                        }
                        else {
                            f_TakeDamage();
                        }
                    }
                }
                else {
                    if (p_Right == false) {
                        f_Flip(p_Right);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(false);
                                f_Attack();
                            }
                            
                        }
                        else {
                            f_TakeDamage();
                        }
                        
                    }
                }

            }

        }
        
    }

}
