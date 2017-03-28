using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeItemLogic : MonoBehaviour {

    void Awake()
    {
        m_SizeText = transform.FindChild("Size/Text").GetComponent<Text>();
        m_TypeText = transform.FindChild("Type/Text").GetComponent<Text>();
        m_DoorPosition = transform.FindChild("DoorPosition/Text").GetComponent<Text>();
        m_NoteText = transform.FindChild("Note/Text").GetComponent<Text>();

        m_SelectedToggle = transform.FindChild("Selected/Toggle").GetComponent<Toggle>();
    }

    // Use this for initialization
    void Start () {
		
	}

    public void Init(MsgJson.Size size){
        m_Size = size;
        m_SizeText.text = size.size;
        m_TypeText.text = size.type;
        m_DoorPosition.text = size.doorPosition;
        m_NoteText.text = size.note;
    }

    public void Init_2(string size, string doorPosition, string type, string note) {
        m_SizeText.text = size;
        m_TypeText.text = type;
        m_DoorPosition.text = doorPosition;
        m_NoteText.text = note;
    }

    public MsgJson.Size m_Size;

    private InputField m_BandInputField;
    private InputField m_ModelInputField;
    private InputField m_ModelYearField;
   
    private Text m_SizeText;
    private Text m_TypeText;
    private Text m_DoorPosition;
    private Toggle m_SelectedToggle;
    private Text m_NoteText;
}
