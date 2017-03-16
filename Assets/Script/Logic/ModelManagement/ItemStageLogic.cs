using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStageLogic : MonoBehaviour {
    private void Awake()
    {
        m_Text = transform.FindChild("Label").GetComponent<Text>();
    }


    public void Init(MsgJson.Stages stage){
        m_Id = stage.id;
        m_Des = stage.des;
        m_Rank = stage.rank;

        m_Text.text = stage.des;    
    }

    private Text m_Text;

    private string m_Id;
    private string m_Des;
    private string m_Rank;
}
