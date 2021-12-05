using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;
using Newtonsoft.Json;

public class CurrencyManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static CurrencyManager_Manager m_Instance;
    //===== STRUCT =====
    [System.Serializable]
    public class c_Data
    {
        public string[] Inventory;
        public c_VirtualCurrency VirtualCurrency = new c_VirtualCurrency();
        public Dictionary<string, string> VirtualCurrencyRechargeTimes;
    }

    [System.Serializable]
    public class c_VirtualCurrency : SerializableDictionary<string, string> { }
    //===== PUBLIC =====
    [Header("UserInventoryResult")]
    public c_Data m_InventoryData;
    //===== PRIVATES =====
    const string m_CurrencyCode ="SESUAIKAN DENGAN YANG DI PLAYFAB";
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
    }

    void Update() {

    }
    
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    /// <summary>
    /// Method used for update currency balance using playfabAPI GetUserInventory
    /// </summary>
    public void f_GetCurrency() {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            OnUpdateCurrencySuccess, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method that will be called after GetUserInventory API return success
    /// </summary>
    /// <param name="p_Result">Result details </param>
    public void OnUpdateCurrencySuccess(GetUserInventoryResult p_Result) {
        Debug.Log("Currecny");
        //m_InventoryData = JsonConvert.DeserializeObject<c_Data>(p_Result.ToJson()); //INI JADI DICTIONARY
        int t_Currency;
        p_Result.VirtualCurrency.TryGetValue("BE", out t_Currency);
        Player_Manager.m_Instance.m_Berry = t_Currency;
        p_Result.VirtualCurrency.TryGetValue("BF", out t_Currency);
        Fragment_Manager.m_Instance.m_BeginnerFragment = t_Currency;
        p_Result.VirtualCurrency.TryGetValue("MF", out t_Currency);
        Fragment_Manager.m_Instance.m_MediocoreFragment = t_Currency;
        p_Result.VirtualCurrency.TryGetValue("AF", out t_Currency);
        Fragment_Manager.m_Instance.m_AdvanceFragment = t_Currency;
        p_Result.VirtualCurrency.TryGetValue("MA", out t_Currency);
        Fragment_Manager.m_Instance.m_MasterFragment = t_Currency;
        p_Result.VirtualCurrency.TryGetValue("EN", out t_Currency);
        Energy_Manager.m_Instance.m_EnergyAmount = t_Currency;
        if (p_Result.VirtualCurrencyRechargeTimes.TryGetValue("EN", out VirtualCurrencyRechargeTime t_RechargeDetails)) {
            Energy_Manager.m_Instance.
                f_CheckEnergy(t_RechargeDetails);
        }
        UIManager_Manager.m_Instance.f_LoadingFinish();
    }

    /// <summary>
    /// Method used for adding virtual currency, if the currency will be set now
    /// </summary>
    /// <param name="p_Amount">The amount of currency to be added</param>
    /// <param name="p_Currency">Currency type</param>
    public void f_AddVirtualCurrencyRequest(string p_CurrencyCode,int p_Amount) {
        UIManager_Manager.m_Instance.f_LoadinStart();
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest {
            Amount = p_Amount,
            VirtualCurrency = p_CurrencyCode
        }, OnModifiedInGameCurrency, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }

    /// <summary>
    /// Method used for substract virtual currency, if the currency already set beforehand
    /// </summary>
    /// <param name="p_Amount">The amount of currency to be substract</param>
    public void f_RemoveVirtualCurrencyRequest(string p_CurrencyCode, int p_Amount) {
        UIManager_Manager.m_Instance.f_LoadinStart();
        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest {
            Amount = p_Amount,
            VirtualCurrency = p_CurrencyCode,
        }, OnModifiedInGameCurrency, PlayFab_Error.m_Instance.f_OnPlayFabError);
    }


    /// <summary>
    /// Method that will be called after add/substract virtual currency request return true/succeed
    /// </summary>
    /// <param name="p_Result">Result details from request</param>
    void OnModifiedInGameCurrency(ModifyUserVirtualCurrencyResult p_Result) {
        f_GetCurrency();
    }
}
