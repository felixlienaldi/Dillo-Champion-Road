using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Audio_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Audio_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public AudioSource m_BgmSource;
    public AudioSource m_OneShotSource;
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
    public void f_ChangeBgm(AudioClip p_Clip) {
        m_BgmSource.clip = p_Clip;
        m_BgmSource.Stop();
        m_BgmSource.Play();
    }

    public void f_PlayOneShot(AudioClip p_Clip) { 
        m_OneShotSource.PlayOneShot(p_Clip);
    }
}
