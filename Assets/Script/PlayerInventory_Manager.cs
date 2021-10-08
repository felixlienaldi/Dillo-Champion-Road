/*
 * INI GA KEPAKE SEMENTARA
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class PlayerInventory_Manager : MonoBehaviour {
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PlayerInventory_Manager m_Instance;
    //===== STRUCT =====
    [System.Serializable]
    public class c_CatalogList
    {
        public c_CatalogDetail[] Catalog;
    }

    [System.Serializable]
    public class c_CatalogDetail {
        public string DisplayName;
        public string ItemId;
        public string ItemClass;
        public string CatalogVersion;
        public c_ItemPrices VirtualCurrencyPrices;
        public string[] Tags;
        public string CustomData;
        public c_Consumables Consumable;
        public bool CanBecomeCharacter;
        public bool IsStackable;
        public bool IsTradable;
        public bool IsLimitedEdition;
        public int InitialLimitedEditionCount;
    }

    [System.Serializable]
    public class c_Consumables { 
    
    }

    [System.Serializable]
    public class c_ItemPrices : SerializableDictionary<string, int> { }
    //===== PUBLIC =====
    public c_CatalogList m_CatalogList;
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

    public void f_GetCatalogList() {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest {
            CatalogVersion = "1.0"
        },f_OnGetCatalogSuccess,PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    public void f_OnGetCatalogSuccess(GetCatalogItemsResult p_Result) {
        m_CatalogList = JsonConvert.DeserializeObject<c_CatalogList>(p_Result.ToJson());
    }
    
}
