using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using MEC;

public class Intro_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static Intro_Gameobject m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    public VideoPlayer m_Video;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        m_Video.loopPointReached += f_OnVideoFinish;
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_OnVideoFinish(VideoPlayer p_Vp) {
        Timing.RunCoroutine(ie_LoadAsync());
    }

    IEnumerator<float> ie_LoadAsync() {
        AsyncOperation t_Async = SceneManager.LoadSceneAsync("DCR_Composite");

        while(t_Async.isDone){
            yield return Timing.WaitForOneFrame;
        }
    }
}
