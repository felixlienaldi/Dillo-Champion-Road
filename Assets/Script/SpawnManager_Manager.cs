using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static SpawnManager_Manager m_Instance;
    //===== STRUCT =====

    //===== PUBLIC =====
    [Header("Enemy Information")]
    public List<Enemy_GameObject> m_ListEnemyPrefabs;
    public List<Transform> m_SpawnPosition;
    public Transform m_Parents;
    [Header("List Active Enemy")]
    public List<Enemy_GameObject> m_ListEnemy;
    //===== PRIVATES =====
    int m_EnemyType;
    int m_EnemyPosition;
    int m_EnemyIndex;
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
    public Enemy_GameObject f_Spawn() {
        
        if (!Player_GameObject.m_Instance.m_IsCrimson) {
            if (Player_GameObject.m_Instance.m_EnemyKilled < 21) {
                m_EnemyType = 0;
            }
            else if (Player_GameObject.m_Instance.m_EnemyKilled >= 21 && Player_GameObject.m_Instance.m_EnemyKilled < 41) {
                m_EnemyType = UnityEngine.Random.Range(0, 100) > 10 ? 0 : 1;
            }
            else {
                m_EnemyType = UnityEngine.Random.Range(0, 100) > 20 ? 0 : 1;
            }
        }
        else {
            if (Player_GameObject.m_Instance.m_EnemyKilled < 21) {
                m_EnemyType = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
            }
            else if (Player_GameObject.m_Instance.m_EnemyKilled >= 21 && Player_GameObject.m_Instance.m_EnemyKilled < 41) {
                m_EnemyType = UnityEngine.Random.Range(0, 100) > 65 ? 0 : 1;
            }
            else {
                m_EnemyType = UnityEngine.Random.Range(0, 100) > 80 ? 0 : 1;
            }
        }

        //m_EnemyType = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
        m_EnemyPosition = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
        m_EnemyIndex = f_GetActiveIndex();

        if(m_EnemyIndex < 0) {
            m_ListEnemy.Add(Instantiate(m_ListEnemyPrefabs[m_EnemyType]));
            m_EnemyIndex = m_ListEnemy.Count - 1;
        }

        m_ListEnemy[m_EnemyIndex].transform.SetParent(m_Parents);
        m_ListEnemy[m_EnemyIndex].transform.position = m_SpawnPosition[m_EnemyPosition].position;
        f_AdditionalRequirement();
        m_ListEnemy[m_EnemyIndex].f_Init();
        m_ListEnemy[m_EnemyIndex].gameObject.SetActive(true);

        return m_ListEnemy[m_EnemyIndex];
    }

    public int f_GetActiveIndex() {
        for(int i = 0; i < m_ListEnemy.Count; i++) {
            if((int)m_ListEnemy[i].m_Type == m_EnemyType && !m_ListEnemy[i].gameObject.activeSelf) {
                return i;
            }
        }

        return -1;
    }

    public void f_AdditionalRequirement() {
        if ((m_ListEnemy[m_EnemyIndex].transform.position.x < Player_GameObject.m_Instance.transform.position.x && m_ListEnemy[m_EnemyIndex].transform.localScale.x < 0)
            || (m_ListEnemy[m_EnemyIndex].transform.position.x >= Player_GameObject.m_Instance.transform.position.x && m_ListEnemy[m_EnemyIndex].transform.localScale.x > 0)) {

        }
        m_ListEnemy[m_EnemyIndex].m_SpawnPositionIndex = m_EnemyPosition;
    }
}
