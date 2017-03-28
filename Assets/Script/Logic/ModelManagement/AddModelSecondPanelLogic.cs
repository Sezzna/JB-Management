using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AddModelSecondPanelLogic : MonoBehaviour {

    void Awake() {
        m_Confirm = transform.FindChild("Confirm").GetComponent<Button>();
        m_Confirm.onClick.AddListener(OnConfirmButtonClick);

        m_Cancel = transform.FindChild("Cancel").GetComponent<Button>();
        m_Cancel.onClick.AddListener(OnCancelButtonClick);

        m_SizeInputField = transform.FindChild("Size/InputField").GetComponent<InputField>();
     
        m_NoteInputField = transform.FindChild("Note/InputField").GetComponent<InputField>();

        m_TypeDropDown = transform.FindChild("Type/Dropdown").GetComponent<Dropdown>();
        m_DoorPositionDropDown = transform.FindChild("DoorPosition/Dropdown_1").GetComponent<Dropdown>();
    }
	// Use this for initialization
	void Start () {
        //更新DoopDown界面;
        UpdateTypeDropdownView();
        UpdateDoorPositionDropdownView();
    }

    void OnConfirmButtonClick() {
        //检查尺寸字符
        if (m_SizeInputField.text == "") {
            FrameUtil.PopNoticePanel("Size  Cannot Be Empty !");
            return;
        }

        //现有的尺寸通过getSize.php获得，
        WWWForm form = new WWWForm();

        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("size", m_SizeInputField.text);
        form.AddField("type", m_TypeDropDown.captionText.text);
        form.AddField("doorPosition",m_DoorPositionDropDown.captionText.text);
        form.AddField("note", m_NoteInputField.text);

        HttpManager.Instance.SendPostForm(ProjectConst.AddSize, form);

        Destroy(gameObject);
    }

    void OnCancelButtonClick()
    {
        Destroy(gameObject);
    }

    private void UpdateTypeDropdownView()
    {
        AddTypeName();
        m_TypeDropDown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_TypeNameList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_TypeNameList[i];
            m_TypeDropDown.options.Add(tempData);
        }
        m_TypeDropDown.captionText.text = m_TypeNameList[0];
    }

    private void UpdateDoorPositionDropdownView()
    {
        AddDoorPositionName();
        m_DoorPositionDropDown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_DoorPositionNameList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_DoorPositionNameList[i];
            m_DoorPositionDropDown.options.Add(tempData);
        }
        m_DoorPositionDropDown.captionText.text = m_DoorPositionNameList[0];
    }

    //设置DorpDown字段名字;
    private void AddTypeName()
    {
        string s1 = "Normal";
        string s2 = "Family";

        m_TypeNameList.Add(s1);
        m_TypeNameList.Add(s2);
    }

    //设置DorpDown字段名字;
    private void AddDoorPositionName() {
        string s1 = "Front";
        string s2 = "Rear";

        m_DoorPositionNameList.Add(s1);
        m_DoorPositionNameList.Add(s2);
    }


    private Button m_Confirm;
    private Button m_Cancel;

    private InputField m_SizeInputField;
    private Dropdown m_TypeDropDown;
    private Dropdown m_DoorPositionDropDown;
    private InputField m_NoteInputField;

    
    private List<string> m_TypeNameList = new List<string>();

    
    private List<string> m_DoorPositionNameList = new List<string>();
}
