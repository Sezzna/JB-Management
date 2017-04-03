using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelManagementPanelLogic : MonoBehaviour {
    void Awake() {
        m_ModelsList = transform.FindChild("ModelsList/Viewport/Content");
        m_ModelCodeList = transform.FindChild("ModelCodeList/Viewport/Content");
        m_SizeList = transform.FindChild("SizeList/Viewport/Content");

        m_ModelItem = Resources.Load("ModelItem") as GameObject;
        m_ModelCodeItem = Resources.Load("ModelCodeItem") as GameObject;
        m_ModelDetailItem = Resources.Load("ModelDetailItem") as GameObject;

        m_AddModelButton = transform.FindChild("Models/AddModel").GetComponent<Button>();
        m_AddModelButton.onClick.AddListener(OnAddModelClick);

        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);

        m_BrandText = transform.FindChild("Brand/Text").GetComponent<Text>();
        m_ModelText = transform.FindChild("Model/Text").GetComponent<Text>();
        m_ModelYearText = transform.FindChild("ModelYear/Text").GetComponent<Text>();
        m_VersionText = transform.FindChild("Version/Text").GetComponent<Text>();
        m_StatusText = transform.FindChild("DetailStatus/Text").GetComponent<Text>();

        m_MakeChangesButton = transform.FindChild("MakeChanges").GetComponent<Button>();
        m_MakeChangesButton.onClick.AddListener(OnMakeChangesClick);
       

        m_MakeItInactiveButton = transform.FindChild("MakeItInactive").GetComponent<Button>();
        m_MakeItInactiveButton.onClick.AddListener(OnMakeItInactiveClick);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetRange, OnGetRange);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModel, OnGetCarModel);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModelDetail, OnGetModelDetail);

        

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModelPartDetail, OnGetModelPartDetail);

        //清空全局数据;
        ControlPlayer.Instance.m_ModelsDetail = null;
    }

	void Start () {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.GetRange, form);
    }

    //------------------------------------------------ On Button Click ------------------------------------------
    void OnBackButtonClick()
    {
        //加载模块管理面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("MainPanel"));
        Destroy(gameObject);
    }

    void OnAddModelClick() {
        //加载AddModel Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelPanel"));
        Destroy(gameObject);
    }
    //------------------------------------------------MESSAGE RESPONSE------------------------------------------
    //得到模块类别消息处理 1012;
    void OnGetRange(string data)
    {
        MsgJson.ModelRange jsonData = JsonUtility.FromJson<MsgJson.ModelRange>(data);

        //将收到的数据转存到ControlPlayer
        ControlPlayer.Instance.m_ModelsRange = jsonData;

        //加入所有的Models Item
        foreach (var v in ControlPlayer.Instance.m_ModelsRange.range)
        {
            FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem).GetComponent<ModelItemLogic>().Init(v);
        }
    }

    //处理1013 获得车型消息;
    void OnGetCarModel(string data)
    {
        MsgJson.CarModels jsonData = JsonUtility.FromJson<MsgJson.CarModels>(data);
        //将收到的数据转存到ControlPlayer
        ControlPlayer.Instance.m_CarModels = jsonData;

        foreach (Transform child in m_ModelCodeList)
        {
            Destroy(child.gameObject);
        }

        //加入所有的Models Code Item
         foreach (var v in ControlPlayer.Instance.m_CarModels.models)
        {
            FrameUtil.AddChild(m_ModelCodeList.gameObject, m_ModelCodeItem).GetComponent<ModeCodeItemLogic>().Init(v);
        }
    }

    //处理1014车型详细信息消息;
    void OnGetModelDetail(string data) {
        MsgJson.ModelsDetail jsonData = JsonUtility.FromJson<MsgJson.ModelsDetail>(data);

        ControlPlayer.Instance.m_ModelsDetail = jsonData;

        foreach (var v in ControlPlayer.Instance.m_ModelsRange.range) {

            if (v.id == jsonData.models[0].range_id) {
                m_BrandText.text = v.brand;
                m_ModelText.text = v.description;
            }
        }

        m_ModelYearText.text = ControlPlayer.Instance.m_ModelsDetail.models[0].model_year;
        m_VersionText.text = ControlPlayer.Instance.m_ModelsDetail.models[0].version;
        m_StatusText.text = ControlPlayer.Instance.m_ModelsDetail.models[0].status;

        //添加新Size之前删除老的;
        foreach (Transform child in m_SizeList)
        {
            Destroy(child.gameObject);
        }

        foreach (var v in ControlPlayer.Instance.m_ModelsDetail.size)
        {
            FrameUtil.AddChild(m_SizeList.gameObject, m_ModelDetailItem).GetComponent<ModelDetailItemLogic>().Init(v);
        }
    }



    //得到 之前添加的所有车型数据,在转换到 那3个全局数据中 com sp op;
    void OnGetModelPartDetail(string data) {
        ControlPlayer.Instance.m_SaveAddModelMsg = JsonUtility.FromJson<MsgJson.SaveAddModelMsg>(data);

        //转换数据;
        ControlPlayer.Instance.m_AddModelPanelSaveData = new ControlPlayer.AddModelPanelSaveData();
        //转model;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Brand = ControlPlayer.Instance.m_SaveAddModelMsg.models[0].brand;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_Model = ControlPlayer.Instance.m_SaveAddModelMsg.models[0].name;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelCode = ControlPlayer.Instance.m_SaveAddModelMsg.models[0].code;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ModelYear = ControlPlayer.Instance.m_SaveAddModelMsg.models[0].model_year;
        ControlPlayer.Instance.m_AddModelPanelSaveData.m_ChassisType = ControlPlayer.Instance.m_SaveAddModelMsg.models[0].chassis_type;
        ControlPlayer.Instance.m_Version = int.Parse(ControlPlayer.Instance.m_SaveAddModelMsg.models[0].version);

        //转size;
        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.size) {
            MsgJson.Size tempSize = new MsgJson.Size();
            tempSize.id = v.size_id;
            tempSize.size = v.size;
            tempSize.doorPosition = v.doorPosition;
            tempSize.type = v.type;
            tempSize.note = v.note;
            ControlPlayer.Instance.m_AddModelPanelSaveData.m_Size.Add(tempSize);
        }

        //转 part_com

        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.part_com) {
            ControlPlayer.CommonItem comItem = new ControlPlayer.CommonItem();
            
            comItem.item.id = v.item_id;
            comItem.item.product_code = v.product_code;
            comItem.item.description = v.description;
            //comItem.item.units
            comItem.item.unit_price = v.unit_price;
            comItem.item.discount = v.discount;
            //comItem.item.category_id 
            comItem.qty = v.qty;
            if (v.show == "Yes")
            {
                comItem.displayToCustomer = true;
            }
            else {
                comItem.displayToCustomer = false;
            }
            comItem.categoryRank = v.rank;

            ControlPlayer.Instance.m_CommonItemList.Add(comItem);
        }

        //转 part_sp
        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.part_sp)
        {
            ControlPlayer.SpItem spItem = new ControlPlayer.SpItem();
            spItem.item.id = v.item_id;
            spItem.item.product_code = v.product_code;
            spItem.item.description = v.description;
            //comItem.item.units
            spItem.item.unit_price = v.unit_price;
            spItem.item.discount = v.discount;
            //comItem.item.category_id \

            spItem.qty = v.qty;
            spItem.categoryRank = v.rank;
            if (v.show == "Yes")
            {
                spItem.displayToCustomer = true;
            }
            else
            {
                spItem.displayToCustomer = false;
            }
            spItem.sizeId = v.size_id;

            ControlPlayer.Instance.m_SpItemList.Add(spItem);
        }


        //转 part_Op
        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.part_op)
        {
            ControlPlayer.OpItem opItem = new ControlPlayer.OpItem();
            opItem.item.id = v.item_id;
            opItem.item.product_code = v.product_code;
            opItem.item.description = v.description;
            //comItem.item.units
            opItem.item.unit_price = v.unit_price;
            opItem.item.discount = v.discount;
            //comItem.item.category_id \

            opItem.qty = v.qty;
            opItem.categoryRank = v.rank;
            if (v.show == "Yes")
            {
                opItem.displayToCustomer = true;
            }
            else
            {
                opItem.displayToCustomer = false;
            }
            opItem.sizeId = v.size_id;
            opItem.name = v.option_name;
            opItem.standardOrOptional = v.stand;
            opItem.Extra = v.extra;

            ControlPlayer.Instance.m_OpItemList.Add(opItem);
        }

        //转stagedisplay;
        ControlPlayer.Instance.m_StageDisplayList.Clear();
        ControlPlayer.Instance.m_SpStageDisplayList.Clear();
        ControlPlayer.Instance.m_OpStageDisplayList.Clear();

        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.StageDisplay) {
            ControlPlayer.StageDisplay stageDisplay = new ControlPlayer.StageDisplay();
            stageDisplay.itemId = v.itemId;
            stageDisplay.rank = v.rank;
            stageDisplay.stegeId = v.stegeId;
            ControlPlayer.Instance.m_StageDisplayList.Add(stageDisplay);
        }
        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.SpStageDisplay)
        {
            ControlPlayer.SpStageDisplay stageDisplay = new ControlPlayer.SpStageDisplay();
            stageDisplay.itemId = v.itemId;
            stageDisplay.rank = v.rank;
            stageDisplay.stegeId = v.stegeId;
            stageDisplay.sizeId = v.sizeId;
            ControlPlayer.Instance.m_SpStageDisplayList.Add(stageDisplay);
        }
        foreach (var v in ControlPlayer.Instance.m_SaveAddModelMsg.OpStageDisplay)
        {
            ControlPlayer.OpStageDisplay stageDisplay = new ControlPlayer.OpStageDisplay();
            stageDisplay.itemId = v.itemId;
            stageDisplay.rank = v.rank;
            stageDisplay.stegeId = v.stegeId;
            stageDisplay.sizeId = v.sizeId;
            stageDisplay.name = v.name;
            ControlPlayer.Instance.m_OpStageDisplayList.Add(stageDisplay);
        }
        //转 itemstage
        ControlPlayer.Instance.m_ItemStages = new MsgJson.ItemStages();
        ControlPlayer.Instance.m_ItemStages.stages = ControlPlayer.Instance.m_SaveAddModelMsg.ItemStagesList;

        ControlPlayer.Instance.m_Version += 1;

        //加载AddModel Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelPanel"));
    }

    //---------------------------------------------Button -------------------------------------------
    void OnMakeChangesClick()
    {
        if (ControlPlayer.Instance.m_ModelsDetail == null) {
            FrameUtil.PopNoticePanel("Please select a model !");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("id", ControlPlayer.Instance.m_ModelsDetail.models[0].id);
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.GetModelPartDetail, form);
    }

    void OnMakeItInactiveClick() {
        
    }



    //------------------------------------------------MEMBER----------------------------------------
    private Transform m_ModelsList;
    private Transform m_ModelCodeList;
    private Transform m_SizeList;

    private GameObject m_ModelItem;
    private GameObject m_ModelCodeItem;
    private GameObject m_ModelDetailItem;


    private Button m_AddModelButton;

    private Button m_BackButton;

    //Deital 详细界面显示数据;
    private Text m_BrandText;
    private Text m_ModelText;
    private Text m_ModelYearText;
    private Text m_VersionText;
    private Text m_StatusText;

    private Button m_MakeItInactiveButton;
    private Button m_MakeChangesButton;
    
}
