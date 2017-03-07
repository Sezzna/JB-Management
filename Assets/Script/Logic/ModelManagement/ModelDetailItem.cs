using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelDetailItem : MonoBehaviour {
    void Awake()
    {
        m_SizeText = transform.FindChild("Size/Text").GetComponent<Text>();
        m_TypeText = transform.FindChild("Type/Text").GetComponent<Text>();
        m_DoorPositionText = transform.FindChild("DoorPosition/Text").GetComponent<Text>();
        m_Selected = transform.FindChild("Selected/Text").GetComponent<Text>();
        m_NoteText = transform.FindChild("Note/Text").GetComponent<Text>();
    }


    // Use this for initialization
    void Start () {
		
	}

    public void Init(MsgJson.Size size) {
        m_SizeText.text = size.size;
        m_TypeText.text = size.type;
        m_DoorPositionText.text = size.doorPosition; 
        //这个数据应为都是Yes,所以这里写死,后面做完了再取消掉;
        m_Selected.text = "Yes";
        m_NoteText.text = size.note;
    }


    private Text m_SizeText;
    private Text m_TypeText;
    private Text m_DoorPositionText;
    private Text m_Selected;
    private Text m_NoteText;
}
