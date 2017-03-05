using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitlePanelLogic : MonoBehaviour {
    void Awake() {
        m_Logout.Set(transform.FindChild("Logout").gameObject).OnClick = OnLogoutClick;
        m_WelcomeName = transform.FindChild("Welcome");
        m_ModuleName = transform.FindChild("ModuleName").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    //初始化面板;
    void Init()
    {
        m_WelcomeName.GetComponent<Text>().text = "Welcome : " + ControlPlayer.Instance.m_FirstName;
    }

    //修改模块名;
    public void ChangeModuleName(string moduleName) {
        m_ModuleName.text = moduleName;
    }

    //登出按钮;
    void OnLogoutClick(GameObject sender, PointerEventData eventData)
    {
        //点击Logout后就不需要自动登录了;
        ControlPlayer.Instance.m_IsNeedAutoLogin = false;

        //加载LoginPanel;
        //FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("LoginPanel"));

        //清除token；
        PlayerPrefs.DeleteKey("token");

        //加载开始场景；
        SceneManager.LoadScene("StartScene");

        //销毁MainPanel;
        //Destroy(gameObject);
    }

    //---------------------------------------------------------MEMBER---------------------------------------------

    private ButtonExtension m_Logout = new ButtonExtension();

    private Transform m_WelcomeName;

    private Text m_ModuleName; 
}
