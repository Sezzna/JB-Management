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

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSize, OnGetSize);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModelPartDetail, OnGetModelPartDetail);

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
        //现有的尺寸通过getSize.php获得，
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.GetSize, form);
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

    //得到AddModel面板的1015 Size消息;
    void OnGetSize(string data)
    {
        MsgJson.AddModelSize addModelSize = JsonUtility.FromJson<MsgJson.AddModelSize>(data);
        ControlPlayer.Instance.m_AddModelSize = addModelSize;
        //加载AddModel Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelPanel"));
        //销毁Login面板;
        Destroy(gameObject);
    }

    void OnGetModelPartDetail(string data) {
        ControlPlayer.Instance.m_ModelPartDetail = JsonUtility.FromJson<MsgJson.AddModelMsg>(data);


    }

    //---------------------------------------------Button -------------------------------------------
    void OnMakeChangesClick()
    {
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
