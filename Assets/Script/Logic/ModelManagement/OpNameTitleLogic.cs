using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpNameTitleLogic : MonoBehaviour {

    private void Awake()
    {
        m_Name = transform.FindChild("Name").GetComponent<Text>();
    }

    void Init(string s)
    {
        m_Name.text = s;
    }


    private Text m_Name;
}
