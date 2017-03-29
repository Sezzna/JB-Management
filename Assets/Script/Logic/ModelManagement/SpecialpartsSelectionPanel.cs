using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpecialpartsSelectionPanel : MonoBehaviour {
    private void Awake() {
        ControlPlayer.Instance.m_CurrentPanelName = "SpecialpartsSelectionPanel";
        //给标题名字;
        m_ModelName = transform.FindChild("CommonParts/JB").GetComponent<Text>();
            m_ModelName.text = ControlPlayer.Instance.m_ModelName;

            m_LeftSizeList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/SizeList/Viewport/Content");
            m_SupplierList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/SupplierList/Viewport/Content");
            m_ItemList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/ItemList/Viewport/Content");
            m_PartList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/PartList/Viewport/Content");

            m_CancelButton = transform.FindChild("Cancel").GetComponent<Button>();
            m_CancelButton.onClick.AddListener(OnCancelClick);

            m_NextButton = transform.FindChild("Next").GetComponent<Button>();
            m_NextButton.onClick.AddListener(OnNextClick);

            m_TotalMoneyText = transform.FindChild("Money").GetComponent<Text>();

            m_LeftSizeItem = Resources.Load("LeftSizeItem") as GameObject;
            m_ItemItem = Resources.Load("ItemItem") as GameObject;
            m_LeftSpecialItemItem = Resources.Load("LeftSpecialItemItem") as GameObject;
            m_StageTatil = Resources.Load("StageTitle") as GameObject;


            MsgRegister.Instance.Register((short)MsgCode.S2C_GetItem, OnGetItem);
    }

    // Use this for initialization
    void Start()
    {
        //加入第一界面选择的Size;
        foreach (var v in ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size) {
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

        ////获得ItemCategory信息
        //WWWForm form = new WWWForm();
        //form.AddField("token", PlayerPrefs.GetString("token"));
        //HttpManager.Instance.SendPostForm(ProjectConst.GetItemCategory, form);
        ////获得Stages信息;
        //WWWForm form1 = new WWWForm();
        //form1.AddField("token", PlayerPrefs.GetString("token"));
        //HttpManager.Instance.SendPostForm(ProjectConst.GetItemStages, form1);
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
                            FrameUtil.AddChild(m_PartList.gameObject, m_LeftSpecialItemItem).GetComponent<LeftSpecialItemItemLogic>().Init(x.item, x.qty);
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
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("ModelManagementPanel"));
        Destroy(gameObject);
    }

    void OnNextClick()
    {

    }





    //-------------------------------------------- MEMBER ----------------------------------------------
    private Text m_ModelName;

    private GameObject m_LeftSizeItem;
    private GameObject m_StageTatil;
    private GameObject m_LeftSpecialItemItem;

    private Transform m_LeftSizeList;
    private Transform m_PartList;
    private Transform m_SupplierList;
    private Transform m_ItemList;

    private GameObject m_ItemItem;

    private Button m_CancelButton;
    private Button m_NextButton;

    private Text m_TotalMoneyText;
    private string m_CurrentChoiceSizeId;
}
