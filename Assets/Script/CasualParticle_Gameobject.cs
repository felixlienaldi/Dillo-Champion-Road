using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
public class CasualParticle_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====

    //===== PUBLIC =====
    public float m_Timer = 1f;
    public int m_ID = 0;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void OnEnable() {
        Timing.KillCoroutines("Particle" + m_ID.ToString());
        Timing.RunCoroutine(ie_Kill(),"Particle" + m_ID.ToString());
    }

    IEnumerator<float> ie_Kill() {
        yield return Timing.WaitForSeconds(m_Timer);
        gameObject.SetActive(false);
    }
}
