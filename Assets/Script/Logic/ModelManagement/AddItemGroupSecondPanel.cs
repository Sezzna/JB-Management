﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemGroupSecondPanel : MonoBehaviour {
    void Awake()
    {
        m_Cancel = transform.FindChild("Panel/Cancel").GetComponent<Button>();
        m_Cancel.onClick.AddListener(OnCancelClick);
        m_Confirm = transform.FindChild("Panel/Confirm").GetComponent<Button>();
        m_Confirm.onClick.AddListener(OnConfirmClick);
        m_AddNameButton = transform.FindChild("Panel/AddName").GetComponent<Button>();
        m_AddNameButton.onClick.AddListener(OnAddNameClick);


        m_CategoryDropdown = transform.FindChild("Panel/Category/Dropdown").GetComponent<Dropdown>();
        m_NameDropdown = transform.FindChild("Panel/Name/Dropdown").GetComponent<Dropdown>();
        m_StdDropdown = transform.FindChild("Panel/StandOrOptional/Dropdown").GetComponent<Dropdown>() ;

        m_StagesGrounp = transform.FindChild("Panel/StageGroup");

        m_StageToggle = Resources.Load<GameObject>("StageToggle");

        m_Description = transform.FindChild("Panel/Title").GetComponent<Text>();

        m_QtyInputField = transform.FindChild("Panel/QTY/InputField").GetComponent<InputField>();
        m_QtyInputField.onEndEdit.AddListener(OnQtyEndEdit);

        m_ExtraInputfield = transform.FindChild("Panel/Extra/InputField").GetComponent<InputField>();
        m_ExtraInputfield.onEndEdit.AddListener(OnExtraEndEdit);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItemDisplayStage, OnGetItemDisplayStage);
    }

    // Use this for initialization
    void Start()
    {
        //跟新 CategoryDropdown 显示;
        UpdateCategoryDropdownView();
        //跟新 NameDropdown显示;
        UpdateNameDropdownView();
        //更新 StdDropdown显示；
        UpdateStdDropdownView();

        foreach (var v in ControlPlayer.Instance.m_ItemStages.stages)
        {
            FrameUtil.AddChild(m_StagesGrounp.gameObject, m_StageToggle).GetComponent<ItemStageLogic>().Init(v);
        }
    }

    public void Init(MsgJson.Item item)
    {
        m_Item = item;
        m_Description.text = item.description;
        //获得ItemCategory信息
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", m_Item.id);
        //Debug.Log(m_Item.id);
        HttpManager.Instance.SendPostForm(ProjectConst.GetItemDisplayStage, form);
    }

    public void OnGetItemDisplayStage(string data)
    {
        MsgJson.ItemStageDisplay itemStageDisplay = JsonUtility.FromJson<MsgJson.ItemStageDisplay>(data);

        foreach (var v in itemStageDisplay.stages)
        {
            foreach (Transform child in m_StagesGrounp)
            {
                if (child.GetComponent<ItemStageLogic>().m_Id == v.item_stage_id)
                {
                    child.GetComponent<Toggle>().isOn = true;
                }
            }
        }
    }

    //------------------------------------------------------ON BUTTON ----------------------------------------------


    void OnQtyEndEdit(string s)
    {
        m_QtyEndEdit = s;
    }

    void OnExtraEndEdit(string s)
    {
        m_ExtraEndEdit = s;
    }


    //添加名字加号按钮
    void OnAddNameClick() {
        //弹出面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("AddItemGroupNamePanel"));
    }

    void OnCancelClick()
    {
        Destroy(gameObject);
    }


    void OnConfirmClick()
    {
        //判断name是否选择;
        if (m_NameDropdown.captionText.text == "Please Add Name") {
            FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel")).GetComponent<NoticePanelLogic>().Init("Please add a name !");
            return;
        }

        //判断这个Item 是否已经存在Std的选项;
        if (m_StdDropdown.captionText.text == "Standard") {
            foreach (var v in ControlPlayer.Instance.m_OpItemList)
            {
                if (v.sizeId == ControlPlayer.Instance.m_CurrentChoiceSizeId && v.name == m_NameDropdown.captionText.text && v.standardOrOptional == m_StdDropdown.captionText.text) {
                    FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel")).GetComponent<NoticePanelLogic>().Init("There is already a standard option !");
                    return;
                }
            }
        }
        

        //判断categroy 是否选择;
        if (m_CategoryDropdown.captionText.text == "Please Select")
        {
            FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel")).GetComponent<NoticePanelLogic>().Init("categroy must be selected");
            return;
        }
        //查看是否选择了stage;
        bool flag = false;
        foreach (Transform child in m_StagesGrounp)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                flag = true;
                break;
            }
        }
        //判断是否选择了stage
        if (flag == false)
        {
            FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel")).GetComponent<NoticePanelLogic>().Init("Stage must be selected");
            return;
        }

        //发送 ItemCategroyStageUpdate 更新这个Item的Categroy 和 stage信息;
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("item_id", m_Item.id);
        form.AddField("category_id", m_CategroyMap[m_CategoryDropdown.captionText.text]);

        //如果Categroy改变过,那么新的m_item里面 就要更新Categroy
        if (m_CategroyMap[m_CategoryDropdown.captionText.text] != m_Item.category_id)
        {
            m_Item.category_id = m_CategroyMap[m_CategoryDropdown.captionText.text];
        }

        //Debug.Log(m_CategoryMap[m_CategoryDropdown.captionText.text]);

        //更新 新的Item 选择的 stage,这个是为了转json 数据存在的;
        List<MsgJson.UpdateItemStageId> stageId = new List<MsgJson.UpdateItemStageId>();

        foreach (Transform child in m_StagesGrounp)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                MsgJson.UpdateItemStageId id = new MsgJson.UpdateItemStageId();
                id.id = child.GetComponent<ItemStageLogic>().m_Id;
                stageId.Add(id);

                //保存这个item 的stage信息传给CommonPartsSelectPanel;
                m_ItemStagesList.Add(id.id);
            }
        }

        MsgJson.UpdateItemStage updateItemStage = new MsgJson.UpdateItemStage();
        updateItemStage.stages = stageId.ToArray();

        form.AddField("msg", JsonUtility.ToJson(updateItemStage));

        //发送 更新这个item的信息;
        HttpManager.Instance.SendPostForm(ProjectConst.ItemCategoryStageUpdate, form);

        //刷新这个供货商下的所有item;
        WWWForm form1 = new WWWForm();
        form1.AddField("token", PlayerPrefs.GetString("token"));
        form1.AddField("id", ControlPlayer.Instance.m_CurrentSupplierID);

        HttpManager.Instance.SendPostForm(ProjectConst.GetItem, form1);

        //再保留数据;
        //删除之前的ItemStages
        int check = 1;
        while (check == 1)
        {
            check = 0;
            foreach (var i in ControlPlayer.Instance.m_OpStageDisplayList)
            {
                if (i.itemId == m_Item.id)
                {
                    ControlPlayer.Instance.m_OpStageDisplayList.Remove(i);
                    check = 1;
                    break;
                }
            }
        }

        //保留stage
        foreach (var v in m_ItemStagesList)
        {
            ControlPlayer.OpStageDisplay opStageDisplay = new ControlPlayer.OpStageDisplay();
            opStageDisplay.itemId = m_Item.id;
            opStageDisplay.stegeId = v;
            opStageDisplay.rank = m_CategoryRankMap[m_CategoryDropdown.captionText.text];
            opStageDisplay.sizeId = ControlPlayer.Instance.m_CurrentChoiceSizeId;
            opStageDisplay.name = m_NameDropdown.captionText.text;

            int added = 0;
            if (ControlPlayer.Instance.m_OpStageDisplayList.Count > 0)
            {
                for (int x = 0; x < ControlPlayer.Instance.m_OpStageDisplayList.Count; x++)
                {
                    if (System.Convert.ToInt32(ControlPlayer.Instance.m_OpStageDisplayList[x].rank) >= System.Convert.ToInt32(opStageDisplay.rank))
                    {
                        if (x == 0)
                        {
                            ControlPlayer.Instance.m_OpStageDisplayList.Insert(0, opStageDisplay);
                            added = 1;
                            break;
                        }
                        else
                        {
                            ControlPlayer.Instance.m_OpStageDisplayList.Insert(x, opStageDisplay);
                            added = 1;
                            break;
                        }
                    }
                }
                if (added == 0)
                {
                    ControlPlayer.Instance.m_OpStageDisplayList.Add(opStageDisplay);
                }
            }
            else
            {
                ControlPlayer.Instance.m_OpStageDisplayList.Add(opStageDisplay);
            }
        }

        //保留item 这个才是 正真要显示在左边的内容;
        ControlPlayer.OpItem opItem = new ControlPlayer.OpItem();

        opItem.item = m_Item;
        opItem.qty = m_QtyEndEdit;
        opItem.categoryRank = m_CategoryRankMap[m_CategoryDropdown.captionText.text];
        opItem.displayToCustomer = true;
        opItem.sizeId = ControlPlayer.Instance.m_CurrentChoiceSizeId;
        opItem.name = m_NameDropdown.captionText.text;
        opItem.standardOrOptional = m_StdDropdown.captionText.text;
        opItem.Extra = m_ExtraEndEdit;


        //判断左边list里面有没有 这个item选项;
        foreach (var i in ControlPlayer.Instance.m_OpItemList)
        {
            //如果有就Remove掉;
            if (i.item.id == opItem.item.id)
            {
                ControlPlayer.Instance.m_OpItemList.Remove(i);
                break;
            }
        }

        //添加这个item;     
        ControlPlayer.Instance.m_OpItemList.Add(opItem);



        //添加到左测面板.
        GameObject.Find("LastPartsSelectionPanel(Clone)").GetComponent<LastPartsSelectionPanelLogic>().AddPartItem();

        //最后删除面板;
        Destroy(gameObject);
    }

    private void UpdateCategoryDropdownView()
    {
        m_CategoryDropdown.options.Clear();
        AddCategoryDropdownItemName();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_CategoryList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_CategoryList[i];
            m_CategoryDropdown.options.Add(tempData);
        }

        if (m_Item.category_id != "0")
        {
            for (int i = 0; i < m_CategoryDropdown.options.Count; ++i)
            {
                if (m_CategoryDropdown.options[i].text == m_CategoryIdKeyMap[m_Item.category_id])
                {
                    m_CategoryDropdown.value = i;
                    m_CategoryDropdown.captionText.text = m_CategoryIdKeyMap[m_Item.category_id];
                    break;
                }
            }
        }
        else
        {
            m_CategoryDropdown.captionText.text = "Please Select";
        }
    }

    public  void UpdateNameDropdownView() {
        //清空;
        m_NameDropdown.options.Clear();
        Dropdown.OptionData tempData;
    
        for (int i = 0; i < ControlPlayer.Instance.m_NameList.Count; ++i)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = ControlPlayer.Instance.m_NameList[i];
            m_NameDropdown.options.Add(tempData);
        }

        //如果没有值,那么显示一个请添加名字;
        if (ControlPlayer.Instance.m_NameList.Count == 0)
        {
            m_NameDropdown.captionText.text = "Please Add Name";
        }
        else {
            m_NameDropdown.value = 0;
            m_NameDropdown.captionText.text = ControlPlayer.Instance.m_NameList[0];
        }
    }

    public void UpdateStdDropdownView()
    {
        //清空;
        m_StdDropdown.options.Clear();
        Dropdown.OptionData tempData;
        Dropdown.OptionData tempData_1;

        //std 下拉框只有两个选项；
        tempData = new Dropdown.OptionData();
        tempData.text = "Standard";
        m_StdDropdown.options.Add(tempData);

        tempData_1 = new Dropdown.OptionData();
        tempData_1.text = "Optional";
        m_StdDropdown.options.Add(tempData_1);

        //默认是标准；
        m_StdDropdown.value = 0;
        m_StdDropdown.captionText.text = "Standard";
    }

    //设置CategoryDorpDown字段名字;
    private void AddCategoryDropdownItemName()
    {
        foreach (var v in ControlPlayer.Instance.m_ItemCategorys.category)
        {
            string temp = v.rank + " " + v.des;
            m_CategoryList.Add(temp);
            m_CategroyMap[temp] = v.id;
            m_CategoryRankMap[temp] = v.rank;
            m_CategoryIdKeyMap[v.id] = temp;
        }
    }


    //---------------------------------- MEMBER ---------------------------------
    private Text m_Description;

    private Dropdown m_CategoryDropdown;
    private Dropdown m_NameDropdown;
    private Dropdown m_StdDropdown;

    private List<string> m_CategoryList = new List<string>();

    private Button m_AddNameButton;
    private Button m_Cancel;
    private Button m_Confirm;

    private Transform m_StagesGrounp;
    private GameObject m_StageToggle;

    //Rank + 描述作为key, id 作为value 这样下拉框选择后才知道选的什么categroy id;
    private Dictionary<string, string> m_CategroyMap = new Dictionary<string, string>();

    //id 作为key , Rank + 描述作为value,  这样才能通过id 选取默认值;
    private Dictionary<string, string> m_CategoryIdKeyMap = new Dictionary<string, string>();
    //Rank + 描述作为key, Rank 作为value 这样下拉框选择后才知道categroy 的rank;
    private Dictionary<string, string> m_CategoryRankMap = new Dictionary<string, string>();


    private MsgJson.Item m_Item;

    private InputField m_QtyInputField;
    private InputField m_ExtraInputfield;

    //数量默认1
    private string m_QtyEndEdit = "1";
    private string m_ExtraEndEdit = "0";

    private List<string> m_ItemStagesList = new List<string>();

}
