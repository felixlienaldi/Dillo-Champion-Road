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
