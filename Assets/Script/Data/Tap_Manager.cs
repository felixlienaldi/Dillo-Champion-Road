using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObjectPooler;
public class Tap_Manager : PoolingManager_Manager<PrefabParticle_Gameobject>{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Tap_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public Camera m_UiCam;
    public Transform m_Parent;
    //===== PRIVATES =====
    PrefabParticle_Gameobject t_Temp;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        if (Input.GetMouseButtonUp(0)) {
            f_Spawn(m_UiCam.ScreenToWorldPoint(Input.mousePosition));   
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_Spawn(Vector3 p_Pos) {
        t_Temp = f_SpawnObject();
        t_Temp.gameObject.SetActive(false);
        t_Temp.transform.parent = m_Parent;
        p_Pos.z = 100;
        t_Temp.f_Init();
        t_Temp.transform.position = p_Pos;
        t_Temp.gameObject.SetActive(true);
    }
}
