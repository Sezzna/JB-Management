using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UserAccessPanelLogic : MonoBehaviour {

    void Awake() {
        m_UserItemList = transform.FindChild("UserList/Content");
        m_UserAccessUserItem = Resources.Load("UserAccessUserItem") as GameObject;

        m_FirstName = transform.FindChild("UserInfo/FirstNameInputField").GetComponent<InputField>();
        m_LastName = transform.FindChild("UserInfo/LastNameInputField").GetComponent<InputField>();
        m_Email = transform.FindChild("UserInfo/EmailInputField").GetComponent<InputField>(); ;
        m_Phone = transform.FindChild("UserInfo/PhoneInputField").GetComponent<InputField>();

        transform.FindChild("UserInfo/FirstNameInputField").GetComponent<InputField>().onEndEdit.AddListener(OnFirstNameEditEnd);
        transform.FindChild("UserInfo/LastNameInputField").GetComponent<InputField>().onEndEdit.AddListener(OnLastNameEditEnd);
        transform.FindChild("UserInfo/EmailInputField").GetComponent<InputField>().onEndEdit.AddListener(OnEmailNameEditEnd);
        transform.FindChild("UserInfo/PhoneInputField").GetComponent<InputField>().onEndEdit.AddListener(OnPhoneNameEditEnd);

        m_Group = transform.FindChild("Group");
        m_GroupToggle = Resources.Load("GroupToggle") as GameObject;

        m_SaveButton = transform.FindChild("Save").GetComponent<Button>();
        m_SaveButton.onClick.AddListener(OnSaveButtonClick);

        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);

        m_DeleteButton = transform.FindChild("Delete").GetComponent<Button>();
        m_DeleteButton.onClick.AddListener(OnDeleteButtonClick);

        MsgRegister.Instance.Register((short)MsgCode.S2C_AddNewUser, OnAddNewUser);
        MsgRegister.Instance.Register((short)MsgCode.S2C_DeleteUser, OnDeleteUser);
        MsgRegister.Instance.Register((short)MsgCode.S2C_SaveUserInfo, OnSaveUserInfo);
    }

    void Start () {
        //加入所有用户;
        foreach (var v in ControlPlayer.Instance.m_UserAccessData.users){
            FrameUtil.AddChild(m_UserItemList.gameObject, m_UserAccessUserItem).GetComponent<UserAccessUserItemLogic>().Init(v.id);
        }

        //加入新添用户按钮;
        FrameUtil.AddChild(m_UserItemList.gameObject, m_UserAccessUserItem).GetComponent<UserAccessUserItemLogic>().Init("Add New User", AddNewUser);

        //将第一个用户的信息显示在用户信息里面;
        m_FirstName.text = ControlPlayer.Instance.m_UserAccessData.users[0].firstname;
        m_LastName.text = ControlPlayer.Instance.m_UserAccessData.users[0].lastname;
        m_Email.text = ControlPlayer.Instance.m_UserAccessData.users[0].email;
        m_Phone.text = ControlPlayer.Instance.m_UserAccessData.users[0].phone;

        //第一个用户的ID;
        ControlPlayer.Instance.m_CurrentEditUserID = ControlPlayer.Instance.m_UserAccessData.users[0].id;

        //加入现在所有的组,并检查第一个用户有没有这些组功能;
        foreach (var i in ControlPlayer.Instance.m_UserAccessData.groups)
        {
            GameObject go = FrameUtil.AddChild(m_Group.gameObject, m_GroupToggle);
            go.GetComponent<GroupToggleItemLogic>().Init(i);

            m_UserAccessToggleDic[i.id] = go;
        }
        RefreshGroupDisplay(ControlPlayer.Instance.m_UserAccessData.users[0].id);
    }
    //----------------------------------------- Function ------------------------------------------------
    //刷新用户所拥有的功能组显示;
    public void RefreshGroupDisplay(string userID) {
        if (ControlPlayer.Instance.m_UserHaveGroupDic.ContainsKey(userID))
        {
            List<string> s =  ControlPlayer.Instance.m_UserHaveGroupDic[userID];

            for (int i = 0; i < s.Count; ++i)
            {
                m_UserAccessToggleDic[s[i]].GetComponent<Toggle>().isOn = true;
            }
        }
        else {
            foreach (var v in m_UserAccessToggleDic) {
                v.Value.GetComponent<Toggle>().isOn = false;
            }
        }
    }

    public void DeleteGroup(string userID, string GroupID) {
        if (m_DeleteGroupDic.ContainsKey(userID))
        {
            for (int i = 0 ; i < m_DeleteGroupDic[userID].Count; ++i) {
                if (m_DeleteGroupDic[userID][i] == GroupID) {
                    return;
                }
            }
            m_DeleteGroupDic[userID].Add(GroupID);
        }
        else
        {
            List<string> tmp = new List<string>();
            tmp.Add(GroupID);
            m_DeleteGroupDic.Add(userID, tmp);
        }
    }

    public void AddGroup(string userID, string GroupID)
    {
        if (m_AddGroupDic.ContainsKey(userID)) {
            for (int i = 0; i < m_AddGroupDic[userID].Count; ++i)
            {
                if (m_AddGroupDic[userID][i] == GroupID)
                {
                    return;
                }
            }

            m_AddGroupDic[userID].Add(GroupID);
        }
        else {
            List<string> tmp = new List<string>();
            tmp.Add(GroupID);
            m_AddGroupDic.Add(userID, tmp);
        }
    }

    //---------------------------------------- Call Back -------------------------------------------------

    void AddNewUser() {
        //加载新加组面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("AddNewUserPanel"));
    }

    void OnAddNewUser(string data) {
        //销毁加入新组面板;
        Destroy(GameObject.Find("AddNewUserPanel(Clone)"));

        MsgJson.AddNewUser tmp = JsonUtility.FromJson<MsgJson.AddNewUser>(data);

        if (tmp.state == "success")
        {
            GameObject go = FrameUtil.AddChild(m_UserItemList.gameObject, m_UserAccessUserItem, m_UserItemList.childCount - 1);
            

            ControlPlayer.Instance.m_CurrentEditUserID = tmp.id;

            //将新数据加入总管理;
            ControlPlayer.UserInfo userInfo = new ControlPlayer.UserInfo();
            userInfo.userID = tmp.id;
            userInfo.userName = tmp.username;
            userInfo.firstName = tmp.firstname;
            userInfo.lastName = tmp.lastname;
            userInfo.email = tmp.email;
            userInfo.phone = tmp.phone;

            ControlPlayer.Instance.m_UserInfoDic[tmp.id] = userInfo;

            //刷新该用户的组相关数据;
            RefreshGroupDisplay(userInfo.userID);

            //刷新显示;
            m_FirstName.text = userInfo.firstName;
            m_LastName.text = userInfo.lastName;
            m_Email.text = userInfo.email;
            m_Phone.text = userInfo.phone;

            go.GetComponent<UserAccessUserItemLogic>().Init(tmp.id);


        }
        else
        {
            Debug.LogError("--------------------------------- 添加新组失败 --------------------------------------");
        }
    }

    void OnFirstNameEditEnd(string s) {
        ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].firstName = s;

        if (m_SaveUserInfo.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        {
            m_SaveUserInfo[ControlPlayer.Instance.m_CurrentEditUserID].firstName = s;
        }
        else {
            ControlPlayer.UserInfo userInfo = new ControlPlayer.UserInfo();
            userInfo = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID];

            m_SaveUserInfo.Add(ControlPlayer.Instance.m_CurrentEditUserID, userInfo);
        }
    }

    void OnLastNameEditEnd(string s)
    {
        ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].lastName = s;

        if (m_SaveUserInfo.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        {
            m_SaveUserInfo[ControlPlayer.Instance.m_CurrentEditUserID].lastName = s;
        }
        else
        {
            ControlPlayer.UserInfo userInfo = new ControlPlayer.UserInfo();
            userInfo = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID];

            m_SaveUserInfo.Add(ControlPlayer.Instance.m_CurrentEditUserID, userInfo);
        }
    }

    void OnEmailNameEditEnd(string s)
    {
        ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].email = s;

        if (m_SaveUserInfo.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        {
            m_SaveUserInfo[ControlPlayer.Instance.m_CurrentEditUserID].email = s;
        }
        else
        {
            ControlPlayer.UserInfo userInfo = new ControlPlayer.UserInfo();
            userInfo = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID];

            m_SaveUserInfo.Add(ControlPlayer.Instance.m_CurrentEditUserID, userInfo);
        }
    }

    void OnPhoneNameEditEnd(string s)
    {
        ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].phone = s;

        if (m_SaveUserInfo.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        {
            m_SaveUserInfo[ControlPlayer.Instance.m_CurrentEditUserID].phone = s;
        }
        else
        {
            ControlPlayer.UserInfo userInfo = new ControlPlayer.UserInfo();
            userInfo = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID];

            m_SaveUserInfo.Add(ControlPlayer.Instance.m_CurrentEditUserID, userInfo);
        }
    }

    void OnSaveButtonClick() {
        MsgJson.SaveUserInfo saveUserInfo = new MsgJson.SaveUserInfo();

        MsgJson.Users user = new MsgJson.Users();
        user.id = ControlPlayer.Instance.m_CurrentEditUserID;
        user.username = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].userName;
        user.firstname = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].firstName;
        user.lastname = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].lastName;
        user.email = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].email;
        user.phone = ControlPlayer.Instance.m_UserInfoDic[ControlPlayer.Instance.m_CurrentEditUserID].phone;

        List<MsgJson.Users> uList = new List<MsgJson.Users>();
        uList.Add(user);

        saveUserInfo.users = uList.ToArray();

        List<MsgJson.GroupID> gList = new List<MsgJson.GroupID>();

        foreach(var v in m_UserAccessToggleDic) {
            if (v.Value.GetComponent<Toggle>().isOn) {
                MsgJson.GroupID id = new MsgJson.GroupID();
                id.id = v.Key;
                gList.Add(id);
            }
        }

        saveUserInfo.addGroupList = gList.ToArray();

        //转用户信息数据到Json格式;
        //List<MsgJson.Users> tmp = new List<MsgJson.Users>();

        //foreach (var v in m_SaveUserInfo) {
        //    MsgJson.Users user = new MsgJson.Users();
        //    user.id = v.Key;
        //    user.firstname = v.Value.firstName;
        //    user.lastname = v.Value.lastName;
        //    user.email = v.Value.email;
        //    user.phone = v.Value.phone;

        //    tmp.Add(user);
        //}

        //saveUserInfo.users = tmp.ToArray();

        ////转加入组数据到Json格式;
        //List<MsgJson.Group> tmpAddGroup = new List<MsgJson.Group>();

        //foreach (var v in m_AddGroupDic)
        //{
        //    foreach (var j in v.Value) {
        //        MsgJson.Group group = new MsgJson.Group();
        //        group.id = v.Key;
        //        group.groupId = j;
        //        tmpAddGroup.Add(group);
        //    }
        //}

        //saveUserInfo.addGroups = tmpAddGroup.ToArray();

        ////转删除组数据到Json格式;
        //List<MsgJson.Group> tmpDeleteGroup = new List<MsgJson.Group>();

        //foreach (var v in m_DeleteGroupDic)
        //{
        //    foreach (var j in v.Value)
        //    {
        //        MsgJson.Group group = new MsgJson.Group();
        //        group.id = v.Key;
        //        group.groupId = j;
        //        tmpDeleteGroup.Add(group);
        //    }
        //}

        //saveUserInfo.deleteGroups = tmpDeleteGroup.ToArray();


        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("msg", JsonUtility.ToJson(saveUserInfo));

        Debug.Log(JsonUtility.ToJson(saveUserInfo));

        HttpManager.Instance.SendPostForm(ProjectConst.SaveUserAccess, form);
    }

    void OnBackButtonClick() {
        
        //加载UsersAndGroupsInfoPanel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("UsersAndGroupsInfoPanel"));
        //销毁本面板;
        Destroy(gameObject);
    }

    void OnDeleteButtonClick() {
        //加载UsersAndGroupsInfoPanel;
        GameObject go = FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("ConfirmPanel"));

        go.GetComponent<ConfirmPanelLogic>().Init(OnDeleteUserCallBack);
    }

    void OnDeleteUserCallBack()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", ControlPlayer.Instance.m_CurrentEditUserID);
        HttpManager.Instance.SendPostForm(ProjectConst.DeleteUser, form);
    }

    void OnDeleteUser(string data) {
        //销毁确认面板;
        Destroy(GameObject.Find("ConfirmPanel(Clone)"));

        MsgJson.DeleteGroup tmp = JsonUtility.FromJson<MsgJson.DeleteGroup>(data);
        if (tmp.state == "success")
        {
            //删除界面上的用户;
            for (int i = 0; i < m_UserItemList.childCount; ++i)
            {
                if (m_UserItemList.GetChild(i).GetComponent<UserAccessUserItemLogic>().m_UserID == ControlPlayer.Instance.m_CurrentEditUserID)
                {
                    Destroy(m_UserItemList.GetChild(i).gameObject);

                    //将第一个用户的信息显示在用户信息里面;
                    m_FirstName.text = ControlPlayer.Instance.m_UserAccessData.users[0].firstname;
                    m_LastName.text = ControlPlayer.Instance.m_UserAccessData.users[0].lastname;
                    m_Email.text = ControlPlayer.Instance.m_UserAccessData.users[0].email;
                    m_Phone.text = ControlPlayer.Instance.m_UserAccessData.users[0].phone;

                    RefreshGroupDisplay(ControlPlayer.Instance.m_UserAccessData.users[0].id);
                }
            }
        }
        else
        {
            Debug.LogError("--------------------------------- 删除组失败 --------------------------------------");
        }
    }

    void OnSaveUserInfo(string data) {
        MsgJson.SaveUserInfoBack tmp = JsonUtility.FromJson<MsgJson.SaveUserInfoBack>(data);
        if (tmp.state == "success") {
            if (ControlPlayer.Instance.m_UserHaveGroupDic.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID)) {
                ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID].Clear();
            }
            else
            {
                ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID] = new List<string>();
            }

            foreach (var v in m_UserAccessToggleDic) {
                if (v.Value.GetComponent<Toggle>().isOn) {
                    ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID].Add(v.Key);
                }
            }   
        }
        else
        {
            Debug.LogError("--------------------------------- 保存用户修改信息失败 --------------------------------------");
        }
    }

    //------------------------------------------ MEMBER----------------------------------------------
    private Transform m_UserItemList;
    private GameObject m_UserAccessUserItem;

    private InputField m_FirstName;
    private InputField m_LastName;
    private InputField m_Email;
    private InputField m_Phone;

    private Transform m_Group;
    private GameObject m_GroupToggle;

    //key 组ID, value toggle对象;
    private Dictionary<string, GameObject> m_UserAccessToggleDic = new Dictionary<string, GameObject>();

    //保存用户数据按钮;
    private Button m_SaveButton;
    //返回按钮;
    private Button m_BackButton;
    //加用户按钮;

    //删除用户按钮;
    private Button m_DeleteButton;

    //改变的用户数据;
    private Dictionary<string, ControlPlayer.UserInfo> m_SaveUserInfo = new Dictionary<string, ControlPlayer.UserInfo>();
    //删除的组;
    private Dictionary<string, List<string>> m_DeleteGroupDic = new Dictionary<string, List<string>>();
    //增加的组;
    private Dictionary<string, List<string>> m_AddGroupDic = new Dictionary<string, List<string>>();
}
