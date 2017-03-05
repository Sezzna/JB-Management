
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPanelLogic : MonoBehaviour {
    void Awake()
    {
        m_LoginButton.Set(transform.FindChild("LoginButton").gameObject).OnClick = OnLoginButtonClick;
        m_AccountInputField = transform.FindChild("AccountInputField").GetComponent<InputField>();
        m_PasswordInputField = transform.FindChild("PasswordInputField").GetComponent<InputField>();

     
        //注册登录消息处理回调;
        MsgRegister.Instance.Register((short)MsgCode.S2C_UserLogin, OnLogin);
    }

    void Start() {
        //如果本机存有Token 就直接到主场景跳过登陆场景;
        if (PlayerPrefs.GetString("token") != "" && ControlPlayer.Instance.m_IsNeedAutoLogin)
        {
            //用token为参数发送登录消息;
            WWWForm form = new WWWForm();
            form.AddField("token", PlayerPrefs.GetString("token"));
            form.AddField("time", Time.time.ToString());

            //发送消息；
            HttpManager.Instance.SendPostForm(ProjectConst.LoginMsgUrl, form);
        }
    }

    //------------------------------------------------------------- Button Response ------------------------------------
    void OnLoginButtonClick(GameObject sender, PointerEventData eventData) {

        WWWForm form = new WWWForm();
        form.AddField("myusername", m_AccountInputField.GetComponent<InputField>().text);
        form.AddField("mypassword", m_PasswordInputField.GetComponent<InputField>().text);
        form.AddField("time", Time.time.ToString());

        //发送消息；
        HttpManager.Instance.SendPostForm(ProjectConst.LoginMsgUrl, form);
    }

    //-------------------------------------------------------------- MESSAGE HANDLE ---------------------------------
    void OnLogin(string data) {
        MsgJson.Login jsonData = JsonUtility.FromJson<MsgJson.Login>(data);

        //登录成功;
        if (jsonData.state == "success") {
            //保留token;
            PlayerPrefs.SetString("token", jsonData.token);
            //保留ControlPlayer信息;
            ControlPlayer.Instance.m_GroupName = jsonData.user[0].GroupName;
            ControlPlayer.Instance.m_FirstName = jsonData.user[0].firstname;
            ControlPlayer.Instance.m_LastName = jsonData.user[0].lastname;
            ControlPlayer.Instance.m_Email = jsonData.user[0].email;
            ControlPlayer.Instance.m_Phone = jsonData.user[0].phone;

            //加载主面板;
            FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("MainPanel"));
            //加入TitlePanel；
            FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("TitlePanel"));
            //销毁Login面板;
            Destroy(gameObject);
        }
        //登录失败;
        else if (jsonData.state == "fail") {
            //弹出提示面板;
            Debug.Log("------------------------------------------- 登录失败 !!!-------------------------------------------------");
            GameObject go = FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel"));
            go.GetComponent<NoticePanelLogic>().Init("Login Fail");
        }
    }




    //-------------------------------------------------------------- MEMBER ------------------------------------------------------------

    private ButtonExtension m_LoginButton = new ButtonExtension();

    private InputField m_AccountInputField;
    private InputField m_PasswordInputField;

    public bool m_IsNeedAutoLogin;

}
