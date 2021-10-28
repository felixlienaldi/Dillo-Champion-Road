using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using MoreMountains.NiceVibrations;
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
    [Header("Audio")]
    public List<AudioClip> m_PlayerHitClip;
    public AudioClip[] m_EnemyHitClip;
    public AudioClip m_PlayerDamaged;
    public AudioClip m_NormalBGM;
    public AudioClip m_FeverBGM;

    [Header("Combo System")]
    public int m_Combo;
    public bool m_IsCombo;
    public float m_DefaultMinimumCombo;
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
    public int m_Level = 0;
    public float m_CurrentFeverTimer;

    [Header("Timing")]
    public float m_Timer;
    public float m_DefaultTimer=5f;
    [Header("Animation Name")]
    public string m_AttackAnimation;

    [Header("Other System")]
    public float m_BarrierCount;
    public float m_ReviveCount;
    public float m_AbsoluteHitCount;
    public GameObject m_Barrier;
    public GameObject m_BarrierAfterEffect;
    public GameObject m_Revive;
    public GameObject m_PrecisionLeft;
    public GameObject m_PrecisionRight;
    public GameObject m_FeverGainEffect;
    public GameObject m_AndroidEffect;
    public GameObject m_MasterEffect;
    public UnityEvent m_OnRevived;
    public UnityEvent<float> m_OnAbsoluteHit;
    public GameObject m_NormalEnviorment;
    public GameObject m_FeverEnviorment;
    public GameObject m_FeverEffect;
    public List<AudioClip> m_IdleAudio;
    public bool m_IsCrimson = false;
    public bool m_IsAndroid = false;
    public bool m_IsGrandMaster = false;
    public int m_MaxHit = 4;
    //===== PRIVATES =====
    private int m_HitCount;
    private int m_AndroidHit = 0;
    private bool p_Input = false;
    public bool m_Invincible = false;
    bool m_IsTakeDamageCoroutineRunning;
    float m_CurrentTimer;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    private void Start() {

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
       // m_SpriteRenderer.color = Color.white;
        UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
        UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
    }

    void Update(){
        if (GameManager_Manager.m_Instance.m_GameState == Enumerator.GAME_STATE.GAME) {
            if (f_GetCurrentHealth() <= 0) {
                if (m_ReviveCount > 0) {
                    m_ReviveCount--;
                    m_OnRevived?.Invoke();
                    m_CurrentHealthPoint++;
                    UIManager_Manager.m_Instance.f_AddHP(m_CurrentHealthPoint);
                    //UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
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
            if (Input.GetKeyUp(KeyCode.L)) f_BombBeyond();
        }

        f_CheckBuff();

    }

    //=====================================================================
    //				        OTHER METHOD
    //=====================================================================
    public void f_Reset() {
        m_AndroidHit = 0;
        p_Input = false;
        m_EnemyKilled = 0;
        m_IsCombo = false;
        m_IsFever = false;
        m_CurrentHealthPoint = f_GetMaxHealth();
        m_PerfectHit = 0;
        m_Timer = m_DefaultTimer;
        m_Combo = 0;
        m_FeverGainMultiplier = 1;
        m_BarrierCount = 0;
        m_ReviveCount = 0;
        m_AbsoluteHitCount = 0;
        m_Level = 0;
        m_CurrentTimer = m_Timer;
        m_Invincible = false;
        Audio_Manager.m_Instance.f_ChangeBgm(m_NormalBGM);
        // m_SpriteRenderer.color = Color.white;
        UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
        UIManager_Manager.m_Instance.f_SetHpBar(f_GetCurrentHealth());
        f_CheckTimer();
        f_Move();
        f_CheckCombo();
        f_CheckFever();
        f_CheckBuff();
    }

    public void f_ResetBuffRelated() {
        p_Input = false;
        m_EnemyKilled = 0;
        m_IsCombo = false;
        m_IsFever = false;
        m_CurrentHealthPoint = f_GetMaxHealth();
        m_PerfectHit = 0;
        m_Timer = m_DefaultTimer;
        m_Combo = 0;
        m_FeverGainMultiplier = 1;
        m_BarrierCount = 0;
        m_ReviveCount = 0;
        m_AbsoluteHitCount = 0;
        m_CurrentTimer = m_Timer;
        m_Level = 0;
    }

    public override void f_Flip(bool p_Right) {
        if (p_Right == true) {
            m_SpriteRenderer.flipX = true;
        }
        else {
            m_SpriteRenderer.flipX = false;
        }
    }

    public void f_CheckBuff() {
        //if (m_AbsoluteHitCount > 0) {
        //    m_Animator.SetBool("Potion", true);
        //}
        //else {
        //    m_Animator.SetBool("Potion", false);
        //}

        if (f_GainFever() <= 1) m_FeverGainEffect.SetActive(false);
        else m_FeverGainEffect.SetActive(true);

        if (m_BarrierCount > 0) m_Barrier.SetActive(true);
        else {
            if (m_Barrier.activeInHierarchy) {
                m_Barrier.SetActive(false);
                m_BarrierAfterEffect.SetActive(true);
            }
        }
    }

    public float f_GetFeverTimer() {
        return m_FeverTimer + (m_FeverTimer * m_FeverTimeMultiplier);
    }

    public float f_GainFever() {
        return m_FeverGain * m_FeverGainMultiplier;
    }

    public void f_PlayFXAudio() {
        Audio_Manager.m_Instance.f_PlayOneShot(m_IdleAudio[Random.Range(0, m_IdleAudio.Count)]);
    }

    public void f_CheckCombo() {
        if (m_Combo >= 5) {
            m_IsCombo = true;
        }
        else {
            m_IsCombo = false;
        }
        if (m_IsCombo) {
            UIManager_Manager.m_Instance.m_Combo.SetActive(true);
            UIManager_Manager.m_Instance.f_SetComboText(m_Combo.ToString());
        }
        else {
            UIManager_Manager.m_Instance.m_Combo.SetActive(false);
            UIManager_Manager.m_Instance.f_SetComboText("");
        }
    }

    public void f_CheckFever() {
        if (m_IsFever) {
            //m_SpriteRenderer.color = Color.blue;
            m_CurrentFeverTimer -= Time.deltaTime;
            if(m_CurrentFeverTimer<= 0f) {
                m_CurrentFeverTimer = 0f;
                m_IsFever = false;
                m_NormalEnviorment.gameObject.SetActive(true);
                m_FeverEnviorment.gameObject.SetActive(false);
                Audio_Manager.m_Instance.f_ChangeBgm(m_NormalBGM);
                m_FeverEffect.gameObject.SetActive(false);
                //m_SpriteRenderer.color = Color.white;
                m_Invincible = true;
                Timing.KillCoroutines("Invincible");
                Timing.RunCoroutine(ie_Invincible(), "Invincible");
                UIManager_Manager.m_Instance.f_ChangeFever(m_IsFever);
            }
            UIManager_Manager.m_Instance.f_SetFeverFillBar(m_CurrentFeverTimer, f_GetFeverTimer());
        }

    }

    public void f_CheckTimer() {
        if (!m_IsTakeDamageCoroutineRunning) {
            m_CurrentTimer -= Time.deltaTime;
        }
        if (GameManager_Manager.m_Instance.m_ListActiveEnemies.Count > 0) {
            GameManager_Manager.m_Instance.m_ListActiveEnemies[0].f_Timer(m_CurrentTimer,m_Timer);
        }
        if (m_CurrentTimer <= 0) {
            if(m_BarrierCount > 0) {
                m_BarrierCount--;
            }
            else {
                f_TakeDamage();
            }
            m_CurrentTimer = m_Timer;
            GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
        }
    }

  
    public void f_CheckTiming(Enemy_GameObject p_EnemyObject) {
        m_Combo++;
        DailyMission_Manager.m_Instance.f_AddDestroyedEnemy(1);
        DailyMission_Manager.m_Instance.f_CompareCombo(m_Combo);
        if(DailyMission_Manager.m_Instance.m_EnemyID == 0 && p_EnemyObject.m_Type == Enumerator.ENEMY_TYPE.NORMAL ) DailyMission_Manager.m_Instance.f_AddCertainEnemy();
        else if (DailyMission_Manager.m_Instance.m_EnemyID == 0 && p_EnemyObject.m_Type == Enumerator.ENEMY_TYPE.INVERSE) DailyMission_Manager.m_Instance.f_AddCertainEnemy();

        if (m_Combo % m_MinimumCombo == 0 && !m_IsAndroid) {
            if (f_GetCurrentHealth() < f_GetMaxHealth()) {
                m_CurrentHealthPoint++;
                UIManager_Manager.m_Instance.f_AddHP(f_GetCurrentHealth());
            }
        }

        if (!m_IsFever) {
            m_EnemyKilled++;

            if (m_Timer > .5f) {
                if (m_EnemyKilled >= 50 - (m_Level * 2)) {
                    m_EnemyKilled -= 50 - (m_Level * 2);
                    m_Level++;
                    m_Timer -= 0.25f;
                }
            }

            Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 1f));
        }
        else {
            Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 5f));
        }

        m_CurrentTimer = m_Timer;

       // GameManager_Manager.m_Instance.f_NextLine(p_EnemyObject);
    }

    public void f_Attack() {
        m_HitCount++;
        Audio_Manager.m_Instance.f_PlayOneShot(m_PlayerHitClip[ Random.Range(0, m_PlayerHitClip.Count)]);
        if (m_HitCount > m_MaxHit) m_HitCount = 1;
        m_Animator.SetInteger("Hit",m_HitCount);
        m_Animator.SetTrigger("Punch");
        if (m_IsFever) {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
        else {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
        Audio_Manager.m_Instance.f_PlayOneShot(m_EnemyHitClip[Random.Range(0, m_EnemyHitClip.Length)]);
        f_CheckTiming(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
        p_Input = true;
    }

    public void f_TakeDamage() {
        m_Combo = 0;
        //m_PerfectHit = 0;
        CameraGameObject_GameObject.m_Instance.f_Reset();
        Audio_Manager.m_Instance.f_PlayOneShot(m_PlayerDamaged);
        Timing.RunCoroutine(CameraGameObject_GameObject.m_Instance.ie_Shake(0.2f, 3f));
#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
        UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
        //GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
        if (!m_Invincible) {
            if (!f_CheckMaster()) {
                f_TakeDamage(1);
                UIManager_Manager.m_Instance.f_MinHp(f_GetCurrentHealth());
            }
        }
        m_CurrentTimer = m_Timer;
        if (!m_IsTakeDamageCoroutineRunning) Timing.RunCoroutine(ie_WaitTimer());        
    }

    public void f_OnAbsoluteHit(float p_TransformScaleX) {
        m_PrecisionLeft.gameObject.SetActive(false);
        m_PrecisionRight.gameObject.SetActive(false);
        if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x > transform.position.x) {
            m_PrecisionRight.gameObject.SetActive(true);
        }
        else {
            m_PrecisionLeft.gameObject.SetActive(true);
        }
        f_SetColliderAttack();
    }

    public void f_OnRevived() {
        Timing.RunCoroutine(ie_Revive());
    }

    public IEnumerator<float> ie_Revive() {
        m_Revive.gameObject.SetActive(true);
        float m_InvisTimer = 0;
        do {
           // m_SpriteRenderer.color = new Color(1, 1, 1, 0);
            yield return Timing.WaitForSeconds(0.05f);
           // m_SpriteRenderer.color = new Color(1, 1, 1, 1);
            yield return Timing.WaitForSeconds(0.05f);
            m_InvisTimer += 0.2f;
        } while (m_InvisTimer < 1f);
        //m_SpriteRenderer.color = new Color(1, 1, 1, 1);
        yield return Timing.WaitForSeconds(1f);
    }

    public override void f_Move() {
    }

    public void f_SetColliderAttack() {
        //Audio_Manager.m_Instance.f_PlayOneShot(m_EnemyHitClip[Random.Range(0, m_EnemyHitClip.Length)]);
        //f_CheckTiming(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
    }

    public void f_CallFX() {
        if (transform.position.x > GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x) {
            FX_Manager.m_Instance.f_Left(m_IsFever);
        }
        else {
            FX_Manager.m_Instance.f_Right(m_IsFever);
        }
    }

    public void f_TapAttack() {
            f_Attack(Input.mousePosition.x < Screen.width / 2 ? false : true);
    }

    public void f_Attack(bool p_Right) {
        if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Damage_Potion")) {
            if(!m_IsFever) m_AndroidHit++;
            if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].m_Type == Enumerator.ENEMY_TYPE.INVERSE) {
                if (GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x - transform.position.x >= 0) {
                    if (p_Right == false) {
                        f_Flip(!p_Right);
                        CameraGameObject_GameObject.m_Instance.f_Move(!p_Right, m_Combo);
                        f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0 && !m_IsFever) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                CameraGameObject_GameObject.m_Instance.f_Move(true, m_Combo);
                                f_Flip(true);
                                f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                                f_Attack();
                            }
                           
                        }
                        else f_TakeDamage();
                    }
                }
                else {
                    if (p_Right == true) {
                        f_Flip(!p_Right);
                        CameraGameObject_GameObject.m_Instance.f_Move(!p_Right, m_Combo);
                        f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0 && !m_IsFever) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(false);
                                CameraGameObject_GameObject.m_Instance.f_Move(false, m_Combo);
                                f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
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
                        CameraGameObject_GameObject.m_Instance.f_Move(p_Right, m_Combo);
                        f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0 && !m_IsFever) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(true);
                                CameraGameObject_GameObject.m_Instance.f_Move(true, m_Combo);
                                f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
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
                        CameraGameObject_GameObject.m_Instance.f_Move(p_Right, m_Combo);
                        f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                        f_Attack();
                    }
                    else {
                        if (m_IsFever || m_AbsoluteHitCount > 0) {
                            if (m_AbsoluteHitCount > 0 && !m_IsFever) {
                                m_AbsoluteHitCount--;
                                m_OnAbsoluteHit?.Invoke(transform.localScale.x);
                            }
                            else {
                                f_Flip(false);
                                CameraGameObject_GameObject.m_Instance.f_Move(false, m_Combo);
                                f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                                f_Attack();
                            }
                            
                        }
                        else {
                            f_TakeDamage();
                        }
                        
                    }
                }

            }
            f_CallFX();
            if (!p_Input) {
                GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
            }
            else {
                GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                p_Input = false;
                f_AttackBehind();
                f_CheckAndroid();                
            }
        }
    }


    public void f_CheckScore(Enemy_GameObject p_EnemyObject) {
        if (!m_IsFever) {
            if (m_CurrentTimer / m_Timer >= .7f || m_IsGrandMaster) {
                GameManager_Manager.m_Instance.f_AddScore(p_EnemyObject.m_ScoreValue * 3);
                m_PerfectHit += 3 * f_GainFever();
            }
            else if (m_CurrentTimer / m_Timer >= .4f) {
                GameManager_Manager.m_Instance.f_AddScore(p_EnemyObject.m_ScoreValue * 2);
                m_PerfectHit += 2 * f_GainFever();
            }
            else if (m_CurrentTimer / m_Timer > 0) {
                GameManager_Manager.m_Instance.f_AddScore(p_EnemyObject.m_ScoreValue * 1);
                m_PerfectHit += 1 * f_GainFever();
            }

            UIManager_Manager.m_Instance.f_SetFeverFillBar(m_PerfectHit, m_MinimumHit);
            if (m_PerfectHit >= m_MinimumHit) {
                f_ChangetoFever();
            }
        }
        else {
            GameManager_Manager.m_Instance.f_AddScore(p_EnemyObject.m_ScoreValue * 4);
        }
    }

    public void f_ChangetoFever() {
        m_PerfectHit = 0;
        m_IsFever = true;
        m_Invincible = true;
        m_NormalEnviorment.gameObject.SetActive(false);
        m_FeverEnviorment.gameObject.SetActive(true);
        m_FeverEffect.gameObject.SetActive(true);
        Audio_Manager.m_Instance.f_ChangeBgm(m_FeverBGM);
        m_CurrentFeverTimer = f_GetFeverTimer();
        UIManager_Manager.m_Instance.f_ChangeFever(m_IsFever);
    }

    public void f_AttackBehind() {
        if (m_IsCrimson && !m_IsFever) {
            int t_Index = Random.Range(0, 100);
            if (t_Index <= 50) {
                f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
                f_Attack();
                if (transform.position.x > GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position.x) {
                    FX_Manager.m_Instance.f_LeftBehind();
                }
                else {
                    FX_Manager.m_Instance.f_RightBehind();
                }
                GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
            }
        }
    }

    public void f_CheckAndroid() {
        if (m_IsAndroid && !m_IsFever) {
            if (m_AndroidHit >= 20) {
                m_AndroidHit = 0;
                int t_Index = Random.Range(0, 100);
                if (t_Index <= 20) {
                    f_BombBeyond();
                }
            }
        }
    }

    public void f_BombBeyond() {
        m_AndroidEffect.SetActive(false);
        m_AndroidEffect.SetActive(true);

        for (int i = 0; i < 6; i++) {
            f_CheckScore(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
            Explosion_Manager.m_Instance.f_SpawnExplosion(GameManager_Manager.m_Instance.m_ListActiveEnemies[0].transform.position);
            GameManager_Manager.m_Instance.f_NextLine(GameManager_Manager.m_Instance.m_ListActiveEnemies[0]);
        }
        //for (int i = 0; i < 6; i++) {
        //    GameManager_Manager.m_Instance.f_Spawn(i);
        //}
    }

    public bool f_CheckMaster() {
        if (m_IsGrandMaster) {
            int t_Index = Random.Range(0, 100);
            if (t_Index > 50) {
                m_MasterEffect.SetActive(false);
                m_MasterEffect.SetActive(true);
                return true;
            }
            else {
                return false;
            }
        }
        else return false;
    }

    public void f_SetHP(int p_HP) {
        m_CurrentHealthPoint = p_HP;
    }

    IEnumerator<float> ie_Invincible() {
        yield return Timing.WaitForSeconds(1.0f);
        m_Invincible = false;
    }

    IEnumerator<float> ie_WaitTimer() {
        m_Animator.SetTrigger("Damage");
        m_IsTakeDamageCoroutineRunning = true;
        do {
            yield return Timing.WaitForOneFrame;
        } while (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Damage") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Damage_Potion"));
        m_IsTakeDamageCoroutineRunning = false;
    }
}
