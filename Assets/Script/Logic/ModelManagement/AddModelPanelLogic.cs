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
        MsgRegister.Instance.Register((short)MsgCode.S2C_AddSize, OnAddSize); 
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSupplier, OnGetSupplier);
    }

    void Start()
    {
        //刷新多选框元素;
        UpdateChassisDropdownView();
        //加入所有的SizeItem
        foreach (var v in ControlPlayer.Instance.m_AddModelSize.size)
        {
            FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem).GetComponent<SizeItemLogic>().Init(v);
        }

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



    void OnNextButtonClick()
    {
        //存储信息;

        //发送获取供货商消息
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        HttpManager.Instance.SendPostForm(ProjectConst.GetSupplier, form);
    }

    //----------------------------------------------- MEG Handle --------------------------------------------------------------------
    //处理添加尺寸消息;
    void OnAddSize(string data)
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
}
