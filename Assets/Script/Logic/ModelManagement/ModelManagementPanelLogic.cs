using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelManagementPanelLogic : MonoBehaviour {
    void Awake() {
        m_ModelsList = transform.FindChild("ModelsList/Viewport/Content");
        m_ModelCodeList = transform.FindChild("ModelCodeList/Viewport/Content");

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
        m_StatusText = transform.FindChild("Status/Text").GetComponent<Text>();


        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModel, OnGetCarModel);
  
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSize, OnGetSize);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetModelDetail, OnGetModelDetail);
    }

	void Start () {
        //加入所有的Models Item
        foreach (var v in ControlPlayer.Instance.m_ModelsRange.range)
         {
            FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem).GetComponent<ModelItemLogic>().Init(v);
        }

       
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

            if (v.id == jsonData.modelDetail.range_id) {
                m_BrandText.text = v.brand;
            }
        }

        //m_BrandText = ControlPlayer.Instance.m_ModelsDetail.modelDetail.range_id
        //m_ModelYearText = ControlPlayer.Instance.m_ModelsDetail.model[0].model_year;

        //foreach (var v in ControlPlayer.Instance.m_ModelsDetail.size)
        //{
        //    FrameUtil.AddChild(m_ModelCodeList.gameObject, m_ModelDetailItem).GetComponent<ModeCodeItemLogic>().Init(v);
        //}
    }

    void OnGetSize(string data)
    {
        //加载AddModel Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelPanel"));
        //销毁Login面板;
        Destroy(gameObject);
    }

    //------------------------------------------------MEMBER----------------------------------------
    private Transform m_ModelsList;
    private Transform m_ModelCodeList;

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

    
}
