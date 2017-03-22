using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTitleLogic : MonoBehaviour {
    private void Awake(){
        m_Text = transform.FindChild("Text").GetComponent<Text>();
    }

    public void Init(string text) {
        m_Text.text = text;
    }

    private Text m_Text;
}
