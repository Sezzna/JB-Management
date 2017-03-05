using UnityEngine;
using System.Collections;

public class Singletion<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static bool s_bEnableAutoCreate = true;
    protected static T s_pInstance;

    public static T Instance
    {
        get
        {
            if(s_pInstance == null)
            {
                s_pInstance = (T)GameObject.FindObjectOfType(typeof(T));
                if(s_pInstance == null && s_bEnableAutoCreate)
                {
                    GameObject singleGO = GameObject.Find("Singletion");
                    if(singleGO == null)
                    {
                        singleGO = new GameObject("Singletion");
                        DontDestroyOnLoad(singleGO);  //防止销毁自己
                    }
 
                    GameObject instanceObject = new GameObject(typeof(T).Name);
                    s_pInstance = instanceObject.AddComponent<T>();
                    instanceObject.transform.SetParent(singleGO.transform);
                }
                else if(s_pInstance == null)
                {
                    Debug.LogError("empty refrenced in this scene : "+ typeof(T).Name);
                }
            }
            return s_pInstance;
        }
    }

    public virtual void NewInstance()
    {

    }
}