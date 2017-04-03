using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanelLogic : MonoBehaviour {
    void Awake() {

        m_MainMenuList = transform.FindChild("MainMenu/List");

        m_MainMenuItem = Resources.Load("MainMenuItem") as GameObject;

        MsgRegister.Instance.Register((short)MsgCode.S2C_CheckUserFunction, OnCheckUserFunction);
        MsgRegister.Instance.Register((short)MsgCode.S2C_GetRange, OnGetRange);
    }

    void Start() {
        CheckUserFunction();
    }

    //查看用户权限消息;
    void CheckUserFunction() {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.CheckUserFunction, form);
    }

    //------------------------------------------------------------- Button Response ------------------------------------

    //用户控制按钮;
    void OnUesrAccessClick(GameObject go, PointerEventData eventData)
    {
        //加载UsersAndGroupsInfoPanel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("UsersAndGroupsInfoPanel"));

        GameObject.Find("TitlePanel(Clone)").GetComponent<TitlePanelLogic>().ChangeModuleName("User Access");
        //销毁主面板;
        Destroy(gameObject);
    }

    //模块管理(Model Management);
    void OnModelManagementClick(GameObject go, PointerEventData eventData)
    {
        
        //TitlePanel改名;
        GameObject.Find("TitlePanel(Clone)").GetComponent<TitlePanelLogic>().ChangeModuleName("Model Management");
        //加载模块管理面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("ModelManagementPanel"));

        //销毁主面板;
        Destroy(gameObject);

        //m_CurrentChoseFunctionID = 1;
    }

    //销售;
    void OnSalesClick(GameObject go, PointerEventData eventData)
    {
        GameObject.Find("TitlePanel(Clone)").GetComponent<TitlePanelLogic>().ChangeModuleName("Model Management");
        Debug.Log("点击 Sales");
    }

 

    //-------------------------------------------------------------- MESSAGE HANDLE ---------------------------------
    void OnCheckUserFunction(string data) {
        MsgJson.UserFunction jsonData = JsonUtility.FromJson<MsgJson.UserFunction>(data);

        if (jsonData.state == "success")
        {
            foreach (var v in jsonData.function)
            {
                switch (v.id)
                {
                    case "1":
                        FrameUtil.AddChild(m_MainMenuList.gameObject, m_MainMenuItem).GetComponent<MainMenuItemLogic>().Init("User Access", OnUesrAccessClick);
                        break;
                    case "2":
                        FrameUtil.AddChild(m_MainMenuList.gameObject, m_MainMenuItem).GetComponent<MainMenuItemLogic>().Init("Sales", OnSalesClick);
                        break;
                    case "4":
                        FrameUtil.AddChild(m_MainMenuList.gameObject, m_MainMenuItem).GetComponent<MainMenuItemLogic>().Init("Model Management", OnModelManagementClick);
                        break;
                    default:
                        Debug.Log(v.name);
                        Debug.Log("-------------------------------- OnCheckUserFunction Switch Error ! ---------------------------------------------------");
                        break;
                }
            }
        }
        else {
            GameObject go = FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel"));
            go.GetComponent<NoticePanelLogic>().Init(jsonData.state);
        }
    }

    //得到模块类别消息处理;
    void OnGetRange(string data)
    {
        MsgJson.ModelRange jsonData = JsonUtility.FromJson<MsgJson.ModelRange>(data);

        //将收到的数据转存到ControlPlayer
        ControlPlayer.Instance.m_ModelsRange = jsonData;

        //foreach (var v in jsonData.range) {
        //    Debug.Log(v.id);
        //    Debug.Log(v.description);
        //    Debug.Log(v.brand);
        //}

        //在这里存储消息,然后在ModelManagement 面板;

        ////加入所有用户;
        //foreach (var v in ControlPlayer.Instance.m_UserAccessData.users)
        //{
        //    FrameUtil.AddChild(m_UserItemList.gameObject, m_UserAccessUserItem).GetComponent<UserAccessUserItemLogic>().Init(v.id);
        //}

      
        //加载面板;
        //FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel"));


        ////数据转存到controlplayer;
        //controlplayer.instance.m_useraccessdata = jsonutility.fromjson<msgjson.useraccess>(data);
        ////转存数据,将数据转换为客户端好使用的格式;
        //controlplayer.instance.dataformatconversion(controlplayer.instance.m_useraccessdata);

        //if (m_currentchosefunctionid == 1)
        //{
        //    //加载useraccesspanel;
        //    frameutil.addchild(gameobject.find("canvas/stack"), resources.load<gameobject>("useraccesspanel"));
        //    //销毁login面板;
        //    destroy(gameobject);
        //}
        //else if (m_currentchosefunctionid == 2)
        //{
        //    //加载useraccesspanel;
        //    frameutil.addchild(gameobject.find("canvas/stack"), resources.load<gameobject>("groupspanel"));
        //    //销毁login面板;
        //    destroy(gameobject);
        //}
    }


    //-------------------------------------------------------------各个主菜单功按钮响应--------------------------------------------------


    //-------------------------------------------------------------- MEMBER ------------------------------------------------------------



    private Transform m_MainMenuList;

    private GameObject m_MainMenuItem;
}
