using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LastPartsSelectionPanelLogic : MonoBehaviour {
    private void Awake()
    {
        //暂定名字;
        ControlPlayer.Instance.m_CurrentPanelName = "LastPartsSelectionPanel";
        //给标题名字;
        m_ModelName = transform.FindChild("CommonParts/JB").GetComponent<Text>();
        m_ModelName.text = ControlPlayer.Instance.m_ModelName;

        m_LeftSizeList = transform.FindChild("GameObject/SizeList/Viewport/Content");
        m_SupplierList = transform.FindChild("GameObject/SupplierList/Viewport/Content");
        m_ItemList = transform.FindChild("GameObject/ItemList/Viewport/Content");
        m_PartList = transform.FindChild("GameObject/PartList/Viewport/Content");

        m_CancelButton = transform.FindChild("Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelClick);

        m_NextButton = transform.FindChild("Next").GetComponent<Button>();
        m_NextButton.onClick.AddListener(OnNextClick);

        m_TotalMoneyText = transform.FindChild("Money").GetComponent<Text>();

        m_LeftSizeItem = Resources.Load("LeftSizeItem") as GameObject;
        m_ItemItem = Resources.Load("ItemItem") as GameObject;
        m_LeftSpecialItemItem = Resources.Load("LeftSpecialItemItem") as GameObject;
        m_LeftSpecialColorItemItem = Resources.Load("LeftSpecialColorItemItem") as GameObject;
        m_StageTatil = Resources.Load("StageTitle") as GameObject;
        m_OpNameTitle = Resources.Load("OpNameTitle") as GameObject;
        m_LeftOpItem = Resources.Load("LeftOpItem") as GameObject;


        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItem, OnGetItem);
    }

    void Start()
    {
        //加入第一界面选择的Size;
        foreach (var v in ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size)
        {
            FrameUtil.AddChild(m_LeftSizeList.gameObject, m_LeftSizeItem).GetComponent<LeftSizeItemLogic>().Init(v);
        }
        //默认点选第一个;
        m_LeftSizeList.transform.GetChild(0).GetComponent<LeftSizeItemLogic>().OnClick();

        //重新加入上个界面选择的普通部件;
        //AddPartItem();

        //加入供货商;
        foreach (var v in ControlPlayer.Instance.m_GetSupplier.supplier)
        {
            FrameUtil.AddChild(m_SupplierList.gameObject, Resources.Load<GameObject>("SupplierItem")).GetComponent<SupplierItemLogic>().Init(v);
        }
    }

    //添加左边的Item;
    public void AddPartItem()
    {
        //清空所有的stage和item
        foreach (Transform child in m_PartList)
        {
            Destroy(child.gameObject);
        }

        //首先循环所有的stage;
        foreach (var v in ControlPlayer.Instance.m_ItemStages.stages)
        {
            bool check = false;
            //看看stageDisplayList里面有没有当前循环到的stage;
            foreach (var i in ControlPlayer.Instance.m_StageDisplayList)
            {
                if (i.stegeId == v.id)
                {
                    if (check == false)
                    {
                        FrameUtil.AddChild(m_PartList.gameObject, m_StageTatil).GetComponent<StageTitleLogic>().Init(v.des);
                        check = true;
                    }

                    foreach (var x in ControlPlayer.Instance.m_CommonItemList)
                    {
                        if (x.item.id == i.itemId)
                        {
                            FrameUtil.AddChild(m_PartList.gameObject, m_LeftSpecialItemItem).GetComponent<LeftSpecialItemItemLogic>().Init(x.item, x.qty);
                        }
                    }
                }
            }

            //添加特殊部件;
            foreach (var i in ControlPlayer.Instance.m_SpStageDisplayList)
            {
                if (i.stegeId == v.id && i.sizeId == ControlPlayer.Instance.m_CurrentChoiceSizeId)
                {
                    if (check == false)
                    {
                        FrameUtil.AddChild(m_PartList.gameObject, m_StageTatil).GetComponent<StageTitleLogic>().Init(v.des);
                        check = true;
                    }

                    foreach (var x in ControlPlayer.Instance.m_SpItemList)
                    {
                        if (x.item.id == i.itemId)
                        {
                            FrameUtil.AddChild(m_PartList.gameObject, m_LeftSpecialColorItemItem).GetComponent<LeftSpecialColorItemItemLogic>().Init(x.item, x.qty);
                        }
                    }
                }
            }
            //添加选择部件;
            
            foreach (var y in ControlPlayer.Instance.m_NameList)
            {
                bool title = false;
                foreach (var i in ControlPlayer.Instance.m_OpStageDisplayList)
                {
                    if (i.stegeId == v.id && i.sizeId == ControlPlayer.Instance.m_CurrentChoiceSizeId && i.name==y)
                    {
                        if (check == false)
                        {
                            FrameUtil.AddChild(m_PartList.gameObject, m_StageTatil).GetComponent<StageTitleLogic>().Init(v.des);
                            check = true;
                        }
                        if(title==false)
                        {
                            //输出 name的名字
                            FrameUtil.AddChild(m_PartList.gameObject, m_OpNameTitle).GetComponent<OpNameTitleLogic>().Init(y);
                            title = true;
                        }
                        foreach (var x in ControlPlayer.Instance.m_OpItemList)
                        {
                            if (x.item.id == i.itemId)
                            {
                                FrameUtil.AddChild(m_PartList.gameObject, m_LeftOpItem).GetComponent<LeftOpItemLogic>().Init(x.item,x.standardOrOptional, x.qty, x.item.unit_price, x.Extra);
                            }
                        }
                    }
                }
            }
        }

        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        double money = 0;
        foreach (var v in ControlPlayer.Instance.m_CommonItemList)
        {
            double temp = double.Parse(v.item.unit_price);
            temp *= int.Parse(v.qty);
            money += temp;
        }

        foreach (var i in ControlPlayer.Instance.m_SpItemList)
        {
            if (ControlPlayer.Instance.m_CurrentChoiceSizeId == i.sizeId)
            {
                double temp = double.Parse(i.item.unit_price);
                temp *= int.Parse(i.qty);
                money += temp;
            }
        }

        m_TotalMoneyText.text = "$" + Math.Round(money, 2).ToString();
    }

    void OnGetItem(string data)
    {
        foreach (Transform child in m_ItemList)
        {
            Destroy(child.gameObject);
        }

        MsgJson.Items items = JsonUtility.FromJson<MsgJson.Items>(data);

        foreach (var v in items.item)
        {
            FrameUtil.AddChild(m_ItemList.gameObject, m_ItemItem).GetComponent<ItemItemLogic>().Init(v);
        }
    }


    //-------------------------------------------- Button Click ----------------------------------------------
    void OnCancelClick()
    {
        //加载模块管理面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("SpecialPartsSelectionPanel")).GetComponent<SpecialpartsSelectionPanel>().AddPartItem();
        Destroy(gameObject);
    }

    void OnNextClick()
    {
        //发送获取供货商消息
        WWWForm form = new WWWForm();
        form.AddField("band", ControlPlayer.Instance.m_AddModelPanelSaveData.m_Band);
        form.AddField("model", ControlPlayer.Instance.m_AddModelPanelSaveData.m_Model);
        form.AddField("code", ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelCode);
        form.AddField("model_year", ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear);
        form.AddField("model_year", ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear);

        Msg msg = new Msg();

        //组size数据;
        List<Size> sizeList = new List<Size>();     
        foreach (var i in ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size) {
            Size size = new Size();
            size.id = i.id;
            sizeList.Add(size);
        }

        msg.size = sizeList.ToArray();

        //组part_com数据;
        List<Part_Com> partComList = new List<Part_Com>();
        foreach (var v in ControlPlayer.Instance.m_CommonItemList) {
            Part_Com partCom = new Part_Com();
            partCom.id = v.item.id;
            partCom.qty = v.qty;
            if (v.displayToCustomer == true) {
                partCom.show = "Yes";
            }
            else {
                partCom.show = "No";
            }

            partComList.Add(partCom);
        }

        msg.part_com = partComList.ToArray();

        //组 part_sp数据;
        List<Part_Sp> partSpList = new List<Part_Sp>();
        foreach (var v in ControlPlayer.Instance.m_SpItemList)
        {
            Part_Sp partSp = new Part_Sp();
            partSp.id = v.item.id;
            partSp.qty = v.qty;
            if (v.displayToCustomer == true)
            {
                partSp.show = "Yes";
            }
            else
            {
                partSp.show = "No";
            }

            partSp.size_id = v.sizeId;

            partSpList.Add(partSp);
        }

        msg.part_sp = partSpList.ToArray();

        //组 part_op数据;
        List<Part_Op> partOpList = new List<Part_Op>();
        foreach (var v in ControlPlayer.Instance.m_OpItemList)
        {
            Part_Op partOp = new Part_Op();
            partOp.id = v.item.id;
            partOp.name = v.name;
            partOp.qty = v.qty;
            if (v.displayToCustomer == true)
            {
                partOp.show = "Yes";
            }
            else
            {
                partOp.show = "No";
            }

            partOp.size_id = v.sizeId;

            if (v.standardOrOptional == "Standard")
            {
                partOp.stand = "Yes";
            }
            else
            {
                partOp.stand = "No";
            }

        }

        msg.part_op = partOpList.ToArray();



        form.AddField("msg", JsonUtility.ToJson(msg));
        form.AddField("chassis_type", ControlPlayer.Instance.m_AddModelPanelSaveData.m_ChassisType);
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.ModelMagenementSaveDate, form);
     }

    //最后一条消息用结构体 ModelMagenementSaveDate;
    struct Size {
        public string id;
    }

    struct Part_Com {
        public string id;
        public string qty;
        public string show; //yse or no
    }

    struct Part_Sp
    {
        public string id;
        public string qty;
        public string show; //yse or no
        public string size_id;
    }

    struct Part_Op {
        public string id;
        public string name;
        public string qty;
        public string show; //yse or no
        public string size_id;
        public string stand; //yse or no
    }

    struct Msg
    {
        public Size[] size;
        public Part_Com[] part_com;
        public Part_Sp[] part_sp;
        public Part_Op[] part_op;
    }

//-------------------------------------------- MEMBER ----------------------------------------------
private Text m_ModelName;
    private GameObject m_LeftSpecialColorItemItem;
    private GameObject m_LeftSizeItem;
    private GameObject m_StageTatil;
    private GameObject m_LeftSpecialItemItem;
    private GameObject m_OpNameTitle;
    private GameObject m_LeftOpItem;

    private Transform m_LeftSizeList;
    private Transform m_PartList;
    private Transform m_SupplierList;
    private Transform m_ItemList;

    private GameObject m_ItemItem;

    private Button m_CancelButton;
    private Button m_NextButton;

    private Text m_TotalMoneyText;
}
