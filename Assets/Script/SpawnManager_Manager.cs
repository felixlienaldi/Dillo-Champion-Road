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
        if (Player_GameObject.m_Instance.m_EnemyKilled < 12) {
            m_EnemyType = 0;
        }
        else if (Player_GameObject.m_Instance.m_EnemyKilled >= 12 && Player_GameObject.m_Instance.m_EnemyKilled < 30) {
            m_EnemyType = UnityEngine.Random.Range(0, 100) > 30 ? 0 : 1;
        }
        else {
            m_EnemyType = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
        }
        //m_EnemyType = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
        m_EnemyPosition = UnityEngine.Random.Range(0, 100) > 50 ? 0 : 1;
        m_EnemyIndex = f_GetActiveIndex();

        if(m_EnemyIndex < 0) {
            m_ListEnemy.Add(Instantiate(m_ListEnemyPrefabs[m_EnemyType]));
            m_EnemyIndex = m_ListEnemy.Count - 1;
        }

        m_ListEnemy[m_EnemyIndex].transform.SetParent(transform);
        m_ListEnemy[m_EnemyIndex].transform.position = m_SpawnPosition[m_EnemyPosition].position;
        f_AdditionalRequirement();
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
            m_ListEnemy[m_EnemyIndex].transform.localScale = new Vector3(m_ListEnemy[m_EnemyIndex].transform.localScale.x * -1, m_ListEnemy[m_EnemyIndex].transform.localScale.y, m_ListEnemy[m_EnemyIndex].transform.localScale.z);
        }
        m_ListEnemy[m_EnemyIndex].m_SpawnPositionIndex = m_EnemyPosition;
    }
}
