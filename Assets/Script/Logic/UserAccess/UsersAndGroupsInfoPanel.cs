using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsersAndGroupsInfoPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_UsersInfoButton = transform.FindChild("UsersInfoButton").GetComponent<Button>();
        m_UsersInfoButton.onClick.AddListener(OnUsersInfoButtonClick);

        m_GroupsInfoButton = transform.FindChild("GroupsInfoButton").GetComponent<Button>();
        m_GroupsInfoButton.onClick.AddListener(OnGroupsInfoButtonClick);

        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);

        MsgRegister.Instance.Register((short)MsgCode.S2C_UserAccess, OnUserAccess);
    }

    //------------------------------------------- Message Handle --------------------------------

    void OnUserAccess(string data)
    {
        //数据转存到ControlPlayer;
        ControlPlayer.Instance.m_UserAccessData = JsonUtility.FromJson<MsgJson.UserAccess>(data);
        //转存数据,将数据转换为客户端好使用的格式;
        ControlPlayer.Instance.DataFormatConversion(ControlPlayer.Instance.m_UserAccessData);

        if (m_CurrentChoseFunctionID == 1) {
            //加载UserAccessPanel;
            FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("UserAccessPanel"));
            //销毁Login面板;
            Destroy(gameObject);
        }
        else if (m_CurrentChoseFunctionID == 2) {
            //加载UserAccessPanel;
            FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("GroupsPanel"));
            //销毁Login面板;
            Destroy(gameObject);
        }
    }


    //------------------------------------------- Button Response --------------------------------

    void OnUsersInfoButtonClick() {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.UserAccess, form);

        m_CurrentChoseFunctionID = 1;
    }


    void OnGroupsInfoButtonClick()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));

        HttpManager.Instance.SendPostForm(ProjectConst.UserAccess, form);

        m_CurrentChoseFunctionID = 2;
    }


    void OnBackButtonClick()
    {
        //加载UsersAndGroupsInfoPanel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("MainPanel"));
        GameObject.Find("TitlePanel(Clone)").GetComponent<TitlePanelLogic>().ChangeModuleName("");
        //销毁Login面板;
        Destroy(gameObject);
    }

    //----------------------------------MEMBER----------------------------------
    // 功能 ID 1为用户信息, 2为组信息;
    private int m_CurrentChoseFunctionID = 0;

    private Button m_UsersInfoButton;
    private Button m_GroupsInfoButton;
    private Button m_BackButton;
}
