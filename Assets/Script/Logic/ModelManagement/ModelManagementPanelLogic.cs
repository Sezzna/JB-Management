using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelManagementPanelLogic : MonoBehaviour {
    void Awake() {
        m_ModelsList = transform.FindChild("ModelsList/Viewport/Content");
        m_ModelItem = Resources.Load("ModelItem") as GameObject;
        m_AddModelButton = transform.FindChild("Models/AddModel").GetComponent<Button>();
        m_AddModelButton.onClick.AddListener(OnAddModelClick);
        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetSize, OnGetSize);
    }

	void Start () {
        //加入所有的Models Item
        foreach (var v in ControlPlayer.Instance.m_ModelsRange.range)
         {
            FrameUtil.AddChild(m_ModelsList.gameObject, m_ModelItem).GetComponent<ModelItemLogic>().Init(v);
        }

       
    }

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
    void OnGetSize(string data)
    {
        //这里要保存数据，现在没有数据；

        //加载AddModel Panel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("AddModelPanel"));
        //销毁Login面板;
        Destroy(gameObject);
    }


    //------------------------------------------------MEMBER----------------------------------------
    private Transform m_ModelsList;

    private GameObject m_ModelItem;

    private Button m_AddModelButton;

    private Button m_BackButton;
}
