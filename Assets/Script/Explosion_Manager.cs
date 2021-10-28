using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObjectPooler;
public class Explosion_Manager : PoolingManager_Manager<PrefabParticle_Gameobject>{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Explosion_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====

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
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_SpawnExplosion(Vector3 p_Pos) {
        t_Temp = f_SpawnObject();
        t_Temp.transform.position = p_Pos;
        t_Temp.gameObject.SetActive(true);
    }
}
