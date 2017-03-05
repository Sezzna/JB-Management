using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UserAccessUserItemLogic : MonoBehaviour {

    void Awake()
    {
        m_UserName = transform.FindChild("Text");
        m_Button = GetComponent<Button>();

        m_FirstNmae = GameObject.Find("UserInfo/FirstNameInputField").GetComponent<InputField>();
        m_LastName = GameObject.Find("UserInfo/LastNameInputField").GetComponent<InputField>();
        m_Email = GameObject.Find("UserInfo/EmailInputField").GetComponent<InputField>(); ;
        m_Phone = GameObject.Find("UserInfo/PhoneInputField").GetComponent<InputField>();

        m_UserAccessPanel = GameObject.Find("UserAccessPanel(Clone)");
    }

    void Start() {
       
    }

    public void Init(string userID)
    {
        m_UserID = userID;
        ControlPlayer.UserInfo tmp = ControlPlayer.Instance.m_UserInfoDic[userID];


        m_UserName.GetComponent<Text>().text = tmp.userID + ". " + tmp.lastName +" "+ tmp.firstName;
        m_Button.onClick.AddListener(OnClick);
    }

    ////由外部给CallBack；
    public void Init(string title, UnityAction callBack)
    {
        m_UserName.GetComponent<Text>().text = title;
        m_Button.onClick.AddListener(callBack);
    }


    //--------------------------- Call Back ----------------------------------

    void OnClick() {
        //改变当前正在编辑的用户ID;
        ControlPlayer.Instance.m_CurrentEditUserID = m_UserID;   
             
        //刷新用户信息显示;
        m_FirstNmae.text = ControlPlayer.Instance.m_UserInfoDic[m_UserID].firstName;
        m_LastName.text = ControlPlayer.Instance.m_UserInfoDic[m_UserID].lastName;
        m_Email.text = ControlPlayer.Instance.m_UserInfoDic[m_UserID].email;
        m_Phone.text = ControlPlayer.Instance.m_UserInfoDic[m_UserID].phone;

        //刷新用户组显示;
        m_UserAccessPanel.GetComponent<UserAccessPanelLogic>().RefreshGroupDisplay(m_UserID);
    }

    //--------------------------- MEMBER ------------------------------------

    public string m_UserID;

    private Button m_Button;

    private Transform m_UserName;



    private InputField m_FirstNmae;
    private InputField m_LastName;
    private InputField m_Email;
    private InputField m_Phone;

    private GameObject m_UserAccessPanel;
}
