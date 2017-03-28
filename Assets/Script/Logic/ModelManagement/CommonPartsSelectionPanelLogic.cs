using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CommonPartsSelectionPanelLogic : MonoBehaviour {
    void Awake(){
        //给标题名字;
        m_ModelName = transform.FindChild("CommonParts/JB").GetComponent<Text>();
        m_ModelName.text = ControlPlayer.Instance.m_ModelName;

        m_SupplierList = transform.FindChild("SupplierList/Viewport/Content");
        m_ItemList = transform.FindChild("ItemList/Viewport/Content");
        m_PartList = transform.FindChild("PartList/Viewport/Content");

        m_CancelButton = transform.FindChild("Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelClick);

        m_NextButton = transform.FindChild("Next").GetComponent<Button>();
        m_NextButton.onClick.AddListener(OnNextClick);

        m_TotalMoneyText = transform.FindChild("Money").GetComponent<Text>();

        m_ItemItem = Resources.Load("ItemItem") as GameObject;
        m_LeftItemItem = Resources.Load("LeftItemItem") as GameObject;
        m_StageTatil = Resources.Load("StageTitle") as GameObject;

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItem, OnGetItem);
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItemCategory, OnGetItemCategory);
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItemStages, OnGetItemStages);
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItemDisplayStage, OnGetItemDisplayStage);
        MsgRegister.Instance.Register((short)MsgCode.S2C_ItemCategoryStageUpdate, OnItemCategoryStageUpdate);

        //清空左边边栏的项目;
        ControlPlayer.Instance.m_CommonItemList.Clear();
        //清空左边边栏的StageTitle;
        ControlPlayer.Instance.m_StageDisplayList.Clear(); 

}


    // Use this for initialization
    void Start () {
        foreach (var v in ControlPlayer.Instance.m_GetSupplier.supplier) {
            FrameUtil.AddChild(m_SupplierList.gameObject, Resources.Load<GameObject>("SupplierItem")).GetComponent<SupplierItemLogic>().Init(v);
        }

        //获得ItemCategory信息
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        HttpManager.Instance.SendPostForm(ProjectConst.GetItemCategory, form);
        //获得Stages信息;
        WWWForm form1 = new WWWForm();
        form1.AddField("token", PlayerPrefs.GetString("token"));
        HttpManager.Instance.SendPostForm(ProjectConst.GetItemStages, form1);
    }

    void OnGetItem(string data) {
        foreach (Transform child in m_ItemList){
            Destroy(child.gameObject);
        }

        MsgJson.Items items = JsonUtility.FromJson<MsgJson.Items>(data);

        foreach (var v in items.item) {
            FrameUtil.AddChild(m_ItemList.gameObject, m_ItemItem).GetComponent<ItemItemLogic>().Init(v);
        }
    }

    //添加左边的Item;
    public void AddPartItem() {
        //清空所有的stage和item
        foreach (Transform child in m_PartList) {
            Destroy(child.gameObject);
        }

        //首先循环所有的stage;
        foreach (var v in ControlPlayer.Instance.m_ItemStages.stages) {
            bool check = false;
            //看看stageDisplayList里面有没有当前循环到的stage;
            foreach (var i in ControlPlayer.Instance.m_StageDisplayList) {

                if (i.stegeId == v.id ) {
                    if (check == false) {
                        FrameUtil.AddChild(m_PartList.gameObject, m_StageTatil).GetComponent<StageTitleLogic>().Init(v.des);
                        check = true;
                    }

                    foreach (var x in ControlPlayer.Instance.m_CommonItemList)
                    {
                        if (x.item.id == i.itemId)
                        {
                            FrameUtil.AddChild(m_PartList.gameObject, m_LeftItemItem).GetComponent<LeftItemItemLogic>().Init(x.item, x.qty);
                        }
                    }
                }
            }
        }

        CalculateTotalAmount();
    }

    private void CalculateTotalAmount() {
        double money = 0;
        foreach (var v in ControlPlayer.Instance.m_CommonItemList) {
            double temp = double.Parse(v.item.unit_price);
            temp *= int.Parse(v.qty);
            money += temp;
        }

        m_TotalMoneyText.text ="$" + Math.Round(money, 2).ToString();
    }

    //-------------------------------------------- MessageHandle  ----------------------------------------------
    void OnGetItemCategory(string data) {
        MsgJson.ItemCategory itemCategory = JsonUtility.FromJson<MsgJson.ItemCategory>(data);
        ControlPlayer.Instance.m_ItemCategorys = itemCategory;
    }

    void OnGetItemStages(string data) {
        MsgJson.ItemStages itemStages = JsonUtility.FromJson<MsgJson.ItemStages>(data);
        ControlPlayer.Instance.m_ItemStages = itemStages;
    }

    void OnGetItemDisplayStage(string data) {

    }

    void OnItemCategoryStageUpdate(string data){

    }
    
    //-------------------------------------------- Button Click ----------------------------------------------
    void OnCancelClick() {
        //加载模块管理面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("ModelManagementPanel"));
        Destroy(gameObject);
    }

    void OnNextClick() {
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("SpecialPartsSelectionPanel"));
        Destroy(gameObject);
    }


    //-------------------------------------------- MEMBER ----------------------------------------------
    private Text m_ModelName;

    private GameObject m_StageTatil;
    private GameObject m_LeftItemItem;

    private Transform m_PartList;
    private Transform m_SupplierList;
    private Transform m_ItemList;
   
    private GameObject m_ItemItem;

    private Button m_CancelButton;
    private Button m_NextButton;

    private Text m_TotalMoneyText;

    //保存已经Add到左边的Item的Stage, key 为stageId, val 描述;
    private Dictionary<string, string> m_StagesMap = new Dictionary<string, string>();
    //保存已经加入左边的Item id
    private List<GameObject> m_LeftItemList = new List<GameObject>();
}
