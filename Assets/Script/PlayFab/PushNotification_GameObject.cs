using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
public class PushNotification_GameObject : MonoBehaviour
{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static PushNotification_GameObject m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public string m_PushToken;
    public string m_LastMsg;
    //===== PRIVATES =====
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake() {
        m_Instance = this;
    }

    void Start() {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += f_OnTokeRecieve;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += f_OnMessageReceive;
    }

    void Update() {

    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_OnTokeRecieve(object p_Sender,Firebase.Messaging.TokenReceivedEventArgs p_Token) {
        m_PushToken = p_Token.Token;
        f_RegisterForPush();
    }

    public void f_OnMessageReceive(object p_Sender,Firebase.Messaging.MessageReceivedEventArgs p_Message) {
        Debug.Log("PlayFab: Received a new message from: " + p_Message.Message.From);
        m_LastMsg = "";
        if (p_Message.Message.Data != null) {
            m_LastMsg += "DATA: " + PlayFabSimpleJson.SerializeObject(p_Message.Message.Data) + "\n";
            Debug.Log("PlayFab: Received a message with data:");
            foreach (var pair in p_Message.Message.Data)
                Debug.Log("PlayFab data element: " + pair.Key + "," + pair.Value);
        }
        if (p_Message.Message.Notification != null) {
            Debug.Log("PlayFab: Received a notification:");
            m_LastMsg += "TITLE: " + p_Message.Message.Notification.Title + "\n";
            m_LastMsg += "BODY: " + p_Message.Message.Notification.Body + "\n";
        }
    }

    public void f_RegisterForPush() {
        if (string.IsNullOrEmpty(LoginManager_Manager.m_Instance.m_LoginData.PlayFabId)|| string.IsNullOrEmpty(m_PushToken)) {
            return;
        }

#if UNITY_ANDROID
        var t_Req = new AndroidDevicePushNotificationRegistrationRequest {
            DeviceToken = m_PushToken,
        };
        PlayFabClientAPI.AndroidDevicePushNotificationRegistration(t_Req, f_OnAdroindPFReg,PlayFab_Error.m_Instance.f_OnPlayFabError);
#endif
    }

    public void f_OnAdroindPFReg(AndroidDevicePushNotificationRegistrationResult p_Result) {
        Debug.Log("PF Registration Succes");
    }
}
