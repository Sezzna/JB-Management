using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AddModelPanelLogic : MonoBehaviour {

    void Awake()
    {
        m_ModelsList = transform.FindChild("ModelsList/Viewport/Content");
        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_ModelItem = Resources.Load("SizeItem") as GameObject;

        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);
        m_NextButton = transform.FindChild("Next").GetComponent<Button>();
        m_NextButton.onClick.AddListener(OnNextButtonClick);

        m_AddSizeButton = transform.FindChild("Size/Type/DoorPosition/Selected/Note/AddSize").GetComponent<Button>();
        m_AddSizeButton.onClick.AddListener(OnAddSizeButtonClick);

        m_ChassisDropdown = transform.FindChild("ChassisType/Dropdown").GetComponent<Dropdown>();

        m_BandInputField = transform.FindChild("Band/InputField").GetComponent<InputField>();
        m_ModeInputField = transform.FindChild("Model/InputField").GetComponent<InputField>();
        m_ModelYearInputField = transform.FindChild("ModelYear/InputField").GetComponent<InputField>();
        m_ModelCodeInputField = transform.FindChild("ModelCode/InputField").GetComponent<InputField>();

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSize, OnGetSize);
        MsgRegister.Instance.Register((short)MsgCode.S2C_AddSize, OnAddSize); 
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSupplier, OnGetSupplier);
    }

    void Start()
    {
        //现有的尺寸通过getSize.php获得，
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.GetSize, form);
        
       
        m_BandInputField.text = ControlPlayer.Instance.m_AddModelPanelSaveData.m_Brand;
        m_ModeInputField.text = ControlPlayer.Instance.m_AddModelPanelSaveData.m_Model;
        m_ModelYearInputField.text = ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear;
        m_ModelCodeInputField.text = ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelCode;
        m_ChassisDropdown.captionText.text = ControlPlayer.Instance.m_AddModelPanelSaveData.m_ChassisType;
    }


    //------------------------------------------ On Button Click ---------------------------------------------- 
    void OnAddSizeButtonClick()
    {
        //加载添加车型第二面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelSecondPanel"));
    }

    void OnBackButtonClick() {
        //加载模块管理面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("ModelManagementPanel"));
        Destroy(gameObject);
    }


    //处理 next 按钮后的事情;
    void OnNextButtonClick()
    {
        //清空之前的选择数据;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size.Clear();
        //存储信息;
        //判断InputField是否为空;
        if (m_BandInputField.text == "") {
            FrameUtil.PopNoticePanel("Band Cannot Be Empty !");
            return;
        }

        if (m_ModeInputField.text == "")
        {
            FrameUtil.PopNoticePanel("Model Cannot Be Empty !");
            return;
        }

        if (m_ModelYearInputField.text == "")
        {
            FrameUtil.PopNoticePanel("Model Year Cannot Be Empty !");
            return;
        }

        if (m_ModelCodeInputField.text == "")
        {
            FrameUtil.PopNoticePanel("Model Code Cannot Be Empty !");
            return;
        }

        //保存填写的数据到ControlPlayer
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Brand = m_BandInputField.text;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Model = m_ModeInputField.text;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear = m_ModelYearInputField.text;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelCode = m_ModelCodeInputField.text;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ChassisType = m_ChassisDropdown.captionText.text;

        ControlPlayer.Instance.m_ModelName = ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelCode + "MY" + ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear + "V" + ControlPlayer.Instance.m_Version;

        List<MsgJson.Size> m_SizeList = new List<MsgJson.Size>();

        bool isHaveData = false;

        //保存size;
        foreach (Transform v in m_ModelsList)
        {
            if (v.GetComponent<SizeItemLogic>().m_SelectedToggle.isOn == true)
            {
                isHaveData = true;
                m_SizeList.Add(v.GetComponent<SizeItemLogic>().m_Size);
            }
        }

        if (isHaveData == false) {
            FrameUtil.PopNoticePanel("Must Select At Least One Size");
            return;
        }

        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size = m_SizeList;

        //发送获取供货商消息
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        HttpManager.Instance.SendPostForm(ProjectConst.GetSupplier, form);
    }

    //----------------------------------------------- MEG Handle --------------------------------------------------------------------
    //得到AddModel面板的1015 Size消息;
    void OnGetSize(string data)
    {
        MsgJson.AddModelSize addModelSize = JsonUtility.FromJson<MsgJson.AddModelSize>(data);
        ControlPlayer.Instance.m_AddModelSize = addModelSize;

        //加入所有的SizeItem
        foreach (var v in ControlPlayer.Instance.m_AddModelSize.size)
        {
            GameObject tempSize = FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem);
            tempSize.GetComponent<SizeItemLogic>().Init(v);

            foreach (var i in ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size)
            {
                if (v.id == i.id)
                {
                    tempSize.transform.FindChild("Selected/Toggle").GetComponent<Toggle>().isOn = true;
                }
            }
        }

        //刷新多选框元素;
        UpdateChassisDropdownView();
    }

    //处理添加尺寸消息;
    public void OnAddSize(string data)
    {
        MsgJson.AddSize addSize = JsonUtility.FromJson<MsgJson.AddSize>(data);
        if (addSize.state == "success")
        {
            FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem).GetComponent<SizeItemLogic>().Init_2(
                addSize.size, 
                addSize.type, 
                addSize.doorPosition, 
                addSize.note);
        }
        else {
            Debug.LogError("Add Size Error");
        }
    }

    //处理得到供货商消息;
    void OnGetSupplier(string data) {
        ControlPlayer.Instance.m_GetSupplier = JsonUtility.FromJson<MsgJson.GetSupplier>(data);

        //Common Parts Selection Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("CommonPartsSelectionPanel"));

        Destroy(gameObject);
    }

    //--------------------------------------------------------

    //设置DorpDown字段名字;
    private void AddDoorPositionName()
    {
        string s1 = "On Road"; 
        string s2 = "Off Road";
        string s3 = "Semi Off Road";

        m_ChassisNameList.Add(s1);
        m_ChassisNameList.Add(s2);
        m_ChassisNameList.Add(s3);
    }

    private void UpdateChassisDropdownView()
    {
        AddDoorPositionName();
        m_ChassisDropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_ChassisNameList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_ChassisNameList[i];
            m_ChassisDropdown.options.Add(tempData);
        }
        m_ChassisDropdown.captionText.text = m_ChassisNameList[0];
    }


    //------------------------------------------------MEMBER----------------------------------------
    private Transform m_ModelsList;

    private GameObject m_ModelItem;

    private Button m_BackButton;
    private Button m_NextButton;

    private Button m_AddSizeButton;

    private Dropdown m_ChassisDropdown;

    private List<string> m_ChassisNameList = new List<string>();

    private InputField m_BandInputField;
    private InputField m_ModeInputField;
    private InputField m_ModelYearInputField;
    private InputField m_ModelCodeInputField;
    

}
