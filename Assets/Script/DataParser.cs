using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataParser : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static DataParser m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public List<bool> m_PlayerAvatarList;
    public List<bool> m_PlayerPantsAvatarList;
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
    public void f_AvatarListStringToData(string p_List) {
        for (int i = 0; i < p_List.Length; i++) {
            m_PlayerAvatarList[i] = int.Parse(p_List[i].ToString()) > 0 ? true : false;
        }
    }

    public string f_AvatarListDataToString() {
        string t_ToString = "";
        for(int i = 0; i < m_PlayerAvatarList.Count; i++) {
            t_ToString += m_PlayerAvatarList[i] ? 1 : 0;
        }

        return t_ToString;
    }
}
