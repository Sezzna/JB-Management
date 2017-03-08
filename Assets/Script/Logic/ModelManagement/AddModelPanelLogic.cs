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
        m_AddSizeButton = transform.FindChild("Size/Type/DoorPosition/Selected/Note/AddSize").GetComponent<Button>();
        m_AddSizeButton.onClick.AddListener(OnAddSizeButtonClick);

        m_ChassisDropdown = transform.FindChild("ChassisType/Dropdown").GetComponent<Dropdown>();
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

    private Button m_AddSizeButton;

    private Dropdown m_ChassisDropdown;

    private List<string> m_ChassisNameList = new List<string>();
}
