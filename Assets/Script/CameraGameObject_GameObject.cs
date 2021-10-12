using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;

public class CameraGameObject_GameObject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static CameraGameObject_GameObject m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public GameObject m_Parent;
    [Header("Position")]
    public Vector3 m_HitLeftDefaultPos;
    public Vector3 m_HitRightDefaultPos;
    public Vector3 m_HitLeftComboPos;
    public Vector3 m_HitRightComboPos;
    [Header("Rotation")]
    public Vector3 m_HitLeftDefaultRot;
    public Vector3 m_HitRightDefaultRot;
    public Vector3 m_HitLeftComboRot;
    public Vector3 m_HitRightComboRot;
    //===== PRIVATES =====
    private Vector3 m_DefaultPos = new Vector3(0, 0, -45);
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
    public void f_Move(bool p_Right,bool p_Combo) {
        iTween.StopByName("Move Camera");
        if (p_Right && p_Combo) {
            iTween.MoveTo(m_Parent, iTween.Hash("name","Move Camera","position",m_HitRightComboPos,"time",1.0f));
            iTween.RotateTo(m_Parent, iTween.Hash("name", "Move Camera", "rotation", m_HitRightComboRot, "time", 1.0f));
        }
        else if (p_Right && !p_Combo) {
            iTween.MoveTo(m_Parent, iTween.Hash("name", "Move Camera", "position", m_HitRightDefaultPos, "time", 1.0f));
            iTween.RotateTo(m_Parent, iTween.Hash("name", "Move Camera", "rotation", m_HitRightDefaultRot, "time", 1.0f));
        }
        else if (!p_Right && p_Combo) {
            iTween.MoveTo(m_Parent, iTween.Hash("name", "Move Camera", "position", m_HitLeftComboPos, "time", 1.0f));
            iTween.RotateTo(m_Parent, iTween.Hash("name", "Move Camera", "rotation", m_HitLeftComboRot, "time", 1.0f));
        }
        else if (!p_Right && !p_Combo) {
            iTween.MoveTo(m_Parent, iTween.Hash("name", "Move Camera", "position", m_HitLeftDefaultPos, "time", 1.0f));
            iTween.RotateTo(m_Parent, iTween.Hash("name", "Move Camera", "rotation", m_HitLeftDefaultRot, "time", 1.0f));
        }
    }

    public void f_Reset() {
        iTween.StopByName("Move Camera");
        iTween.MoveTo(m_Parent, iTween.Hash("name", "Move Camera", "position", m_DefaultPos, "time", 1.0f));
        iTween.RotateTo(m_Parent, iTween.Hash("name", "Move Camera", "rotation", Vector3.zero, "time", 1.0f));
    }


    public IEnumerator<float> ie_Shake(float p_Duration, float p_Magnitude) {
        Vector3 m_OriginalPosition = transform.localPosition;
        float m_Elapsed = 0f;
        while(m_Elapsed < p_Duration) {
            float x = UnityEngine.Random.Range(-0.1f, 0.1f) * p_Magnitude;

            transform.localPosition = new Vector3(x, m_OriginalPosition.y, m_OriginalPosition.z);

            m_Elapsed += Time.deltaTime;

            yield return Timing.WaitForOneFrame;
        }

        transform.localPosition = m_OriginalPosition;
    }
}