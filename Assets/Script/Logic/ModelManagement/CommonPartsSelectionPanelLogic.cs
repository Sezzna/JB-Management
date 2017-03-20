using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonPartsSelectionPanelLogic : MonoBehaviour {
    void Awake(){
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
    public void AddPartItem(MsgJson.Item item, string qty, List<string> stagesList) {
        foreach (var i in stagesList) {
            foreach (var v in ControlPlayer.Instance.m_ItemStages.stages)
            {
                if (i == v.id) {
                    m_StagesMap[i] = v.des;
                }
            }
        }
        //在这里决定是否插入新的stage title;
        FrameUtil.AddChild(m_PartList.gameObject, m_LeftItemItem).GetComponent<LeftItemItemLogic>().Init(item, qty);
    }

    //-------------------------------------------- MessageHandle  ----------------------------------------------
    void OnGetItemCategory(string data) {
        MsgJson.ItemCategory itemCategory = JsonUtility.FromJson<MsgJson.ItemCategory>(data);
        ControlPlayer.Instance.m_ItemCategory = itemCategory;
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

    }


    //-------------------------------------------- MEMBER ----------------------------------------------
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
}
