using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ObjectPooler
{
    public class PoolingManager_Manager<T> : MonoBehaviour where T : MonoBehaviour
    {
        //=====================================================================
        //				      VARIABLES 
        //=====================================================================
        //===== SINGLETON =====
        //public static PoolingManager_Manager<T> m_Instance;
        //===== STRUCT =====

        //===== PUBLIC =====
        public List<T> m_Objects;
        public List<T> m_PoolingContainer;
        public Transform m_PoolingParent;
        public Transform m_SpawnTransform;
        //===== PRIVATES =====
        int t_Index;
        T t_Object;
        Vector3 t_DefaultScale = new Vector3(1, 1, 1);
        //=====================================================================
        //				MONOBEHAVIOUR METHOD 
        //=====================================================================
        void Awake() {
            //m_Instance = this;
        }
        //=====================================================================
        //				    OTHER METHOD
        //=====================================================================
        public int f_GetIndex() {
            for(int i = 0; i < m_PoolingContainer.Count; i++) {
                if (!m_PoolingContainer[i].gameObject.activeSelf && f_AdditionalValidation(i)) return i;
            }

            return -1;
        }

        public int f_GetIndex(T p_Object) {
            for (int i = 0; i < m_PoolingContainer.Count; i++) {
                if (!m_PoolingContainer[i].gameObject.activeSelf && f_AdditionalValidation(i, p_Object)) return i;
            }

            return -1;
        }

        public T f_SpawnObject() {
            t_Index = f_GetIndex();
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(m_Objects[f_GetType()]));
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1];
            }
            else t_Object = m_PoolingContainer[t_Index];
            t_Object.transform.SetParent(m_PoolingParent);

            t_Object.transform.localScale = t_DefaultScale;
           
            //if (m_SpawnTransform) {
            //    t_Object.transform.position = m_SpawnTransform.position;
            //    t_Object.transform.localScale = m_SpawnTransform.localScale;
            //}

           
            t_Object.gameObject.SetActive(true);
            return t_Object;
        }

        public GameObject f_SpawnObject(GameObject p_Object, Transform p_Parent) {
            t_Index = f_GetIndex(p_Object.GetComponent<T>());
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object).GetComponent<T>());
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1].GetComponent<T>();
            }
            else t_Object = m_PoolingContainer[t_Index].GetComponent<T>();

            t_Object.transform.SetParent(p_Parent);
            t_Object.transform.localScale = Vector3.one;

            t_Object.gameObject.SetActive(true);
            return t_Object.gameObject;
        }

        public GameObject f_SpawnObject(GameObject p_Object, Transform p_Parent, Vector3 p_Position, Quaternion p_Rotation) {
            t_Index = f_GetIndex(p_Object.GetComponent<T>());
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object).GetComponent<T>());
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1].GetComponent<T>();
            }
            else t_Object = m_PoolingContainer[t_Index].GetComponent<T>();

            t_Object.transform.SetParent(p_Parent);


            t_Object.transform.position = p_Position;
            t_Object.transform.rotation = p_Rotation;

            t_Object.transform.localScale = Vector3.one;

            t_Object.gameObject.SetActive(true);
            return t_Object.gameObject;
        }

        public GameObject f_SpawnObject(GameObject p_Object) {
            t_Index = f_GetIndex(p_Object.GetComponent<T>());
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object).GetComponent<T>());
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1];
            }
            else t_Object = m_PoolingContainer[t_Index];

            t_Object.transform.localScale = Vector3.one;

            t_Object.gameObject.SetActive(true);
            return t_Object.gameObject;
        }

        public T f_SpawnObject(T p_Object, Transform p_Parent) {
            t_Index = f_GetIndex(p_Object);
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object));
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1];
            }
            else t_Object = m_PoolingContainer[t_Index];

            t_Object.transform.SetParent(p_Parent);

            t_Object.transform.localScale = Vector3.one;
            t_Object.gameObject.SetActive(true);
            return t_Object;
        }

        public T f_SpawnObject(T p_Object, Transform p_Parent, Vector3 p_Position, Quaternion p_Rotation) {
            t_Index = f_GetIndex(p_Object);
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object));
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1];
            }
            else t_Object = m_PoolingContainer[t_Index];
            t_Object.transform.SetParent(p_Parent);


            t_Object.transform.localScale = Vector3.one;
            t_Object.transform.position = p_Position;
            t_Object.transform.rotation = p_Rotation;

            t_Object.gameObject.SetActive(true);
            return t_Object;
        }

        public T f_SpawnObject(T p_Object) {
            t_Index = f_GetIndex(p_Object);
            if (t_Index < 0) {
                m_PoolingContainer.Add(Instantiate(p_Object));
                t_Object = m_PoolingContainer[m_PoolingContainer.Count - 1];
            }
            else t_Object = m_PoolingContainer[t_Index];


            t_Object.transform.localScale = Vector3.one;

            t_Object.gameObject.SetActive(true);
            return t_Object;
        }


        public virtual bool f_AdditionalValidation(int p_Index) { return true; }

        public virtual bool f_AdditionalValidation(int p_Index,T p_Object) { return true; }

        public virtual int f_GetType() { return Random.Range(0, 100) < 50 ? 0 : m_Objects.Count-1; } 

    }

}
