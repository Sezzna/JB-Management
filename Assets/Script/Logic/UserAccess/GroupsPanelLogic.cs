using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GroupsPanelLogic : MonoBehaviour {

    void Awake() {
        //GroupItem Prefab;
        m_GroupItem = Resources.Load("GroupsGroupItem") as GameObject;
        //Function toggle Prefab;
        m_FunctionToggle = Resources.Load("FunctionToggle") as GameObject;

        MsgRegister.Instance.Register((short)MsgCode.S2C_AddNewGroup, OnAddNewGroups);
        MsgRegister.Instance.Register((short)MsgCode.S2C_DeleteGroup, OnDeleteGroups);
        MsgRegister.Instance.Register((short)MsgCode.S2C_SaveGroupInfo, OnSaveGroupsInfo);
    }

	// Use this for initialization
	void Start () {
        m_SaveButton = transform.FindChild("Save").GetComponent<Button>();
        m_SaveButton.onClick.AddListener(OnSaveButtonClick);

        m_BackButton = transform.FindChild("Back").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnBackButtonClick);

        m_DeleteButton = transform.FindChild("Delete").GetComponent<Button>();
        m_DeleteButton.onClick.AddListener(OnDeleteButtonClick);

        m_GroupNameInputField = transform.FindChild("GroupNameInputField").GetComponent<InputField>();

        m_FunctionArea = transform.FindChild("FunctionArea");

        //组列表	
        m_GroupItemList = transform.FindChild("GroupList/Content");

        //加入所有的组Item;
        foreach (var v in ControlPlayer.Instance.m_UserAccessData.groups)
        {
            GameObject go = FrameUtil.AddChild(m_GroupItemList.gameObject, m_GroupItem);
            go.GetComponent<GroupsGroupItemLogic>().Init(v);
        }
        //加入 增加新组按钮;
        FrameUtil.AddChild(m_GroupItemList.gameObject, m_GroupItem).GetComponent<GroupsGroupItemLogic>().Init("Add New Group", OnAddGroupButtonClick);

        //加入所有功能Toggle
        foreach (var i in ControlPlayer.Instance.m_UserAccessData.function) {
            GameObject go = FrameUtil.AddChild(m_FunctionArea.gameObject, m_FunctionToggle);
            go.GetComponent<FunctionToggleLogic>().Init(i);

            m_FunctionToggleDic[i.id] = go;
        }

        //刷新,判断那些Toggle要打勾;
        RefreshDisplay(ControlPlayer.Instance.m_UserAccessData.groups[0].id, ControlPlayer.Instance.m_UserAccessData.groups[0].name);

        //当前操作的组ID;
        m_CurrentChoseGroupID = ControlPlayer.Instance.m_UserAccessData.groups[0].id;
    }

    //刷新显示面板部分,组名字,这个组有哪些 function;
    public void RefreshDisplay(string groupID, string groupName) {
        m_CurrentChoseGroupID = groupID;
        m_GroupNameInputField.text = groupName;

        if (!ControlPlayer.Instance.m_GroupFunctionDic.ContainsKey(groupID) || ControlPlayer.Instance.m_GroupFunctionDic[groupID].Count == 0)
        {
            foreach (var j in m_FunctionToggleDic)
            {
                j.Value.GetComponent<Toggle>().isOn = false;
            }
        }
        else {
            foreach (var v in m_FunctionToggleDic)
            {
                foreach (var i in ControlPlayer.Instance.m_GroupFunctionDic[groupID])
                {
                    if (v.Key == i)
                    {
                        v.Value.GetComponent<Toggle>().isOn = true;
                        break;
                    }
                    else
                    {
                        v.Value.GetComponent<Toggle>().isOn = false;
                    }
                }
            }
        }
    }


    //-------------------------------------------- Message Response ------------------------------------------------

    //加入新组;
    void OnAddNewGroups(string data) {
        //销毁加入新组面板;
        Destroy(GameObject.Find("AddNewGroupPanel(Clone)"));

        MsgJson.AddNewGroup tmp = JsonUtility.FromJson<MsgJson.AddNewGroup>(data);
        
        if (tmp.state == "success")
        {
            GameObject go = FrameUtil.AddChild(m_GroupItemList.gameObject, m_GroupItem, m_GroupItemList.childCount - 1);
            go.GetComponent<GroupsGroupItemLogic>().Init(tmp);
            //将新数据加入总管理;
            ControlPlayer.Instance.m_GroupFunctionDic[tmp.id] = new List<string>();
            RefreshDisplay(tmp.id, tmp.name);
        }
        else
        {
            Debug.LogError("--------------------------------- 添加新组失败 --------------------------------------");
        }
    }

    //删除组消息响应;
    void OnDeleteGroups(string data)
    {
        //销毁确认面板;
        Destroy(GameObject.Find("ConfirmPanel(Clone)"));

        MsgJson.DeleteGroup tmp = JsonUtility.FromJson<MsgJson.DeleteGroup>(data);
        if (tmp.state == "success")
        {
            //删除界面上的功能;
            for (int i = 0 ; i < m_GroupItemList.childCount ; ++i) {
                if (m_GroupItemList.GetChild(i).GetComponent<GroupsGroupItemLogic>().m_GroupID == m_CurrentChoseGroupID) {
                    Destroy(m_GroupItemList.GetChild(i).gameObject);

                    m_CurrentChoseGroupID = m_GroupItemList.GetChild(0).GetComponent<GroupsGroupItemLogic>().m_GroupID;

                    RefreshDisplay(m_GroupItemList.GetChild(0).GetComponent<GroupsGroupItemLogic>().m_GroupID, m_GroupItemList.GetChild(0).GetComponent<GroupsGroupItemLogic>().m_GroupName);
                }
            }
        }
        else
        {
            Debug.LogError("--------------------------------- 删除组失败 --------------------------------------");
        }
    }

    //处理保存组信息返回;
    void OnSaveGroupsInfo(string data) {
        MsgJson.SaveGroupInfoBack tmp = JsonUtility.FromJson<MsgJson.SaveGroupInfoBack>(data);
        if (tmp.state == "success")
        {
            if (ControlPlayer.Instance.m_GroupFunctionDic.ContainsKey(m_CurrentChoseGroupID))
            {
                ControlPlayer.Instance.m_GroupFunctionDic[m_CurrentChoseGroupID].Clear();
            }
            else {
                ControlPlayer.Instance.m_GroupFunctionDic[m_CurrentChoseGroupID] = new List<string>();
            }

            foreach (var v in m_FunctionToggleDic)
            {
                if (v.Value.GetComponent<Toggle>().isOn)
                {
                    ControlPlayer.Instance.m_GroupFunctionDic[m_CurrentChoseGroupID].Add(v.Key);
                }
            }
        }
        else
        {
            Debug.LogError("--------------------------------- 保存组信息失败 --------------------------------------");
        }
    }


    //----------------------------------------------- Button Response --------------------------------------
    //新加组;
    void OnAddGroupButtonClick() {
        //加载新加组面板;
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("AddNewGroupPanel"));
    }
    
    //存储改变后的功能数据;
    void OnSaveButtonClick() {
        MsgJson.SaveGroupInfo tmp = new MsgJson.SaveGroupInfo();

        MsgJson.Groups m = new MsgJson.Groups();
        List<MsgJson.Groups> ml = new List<MsgJson.Groups>();

        m.id = m_CurrentChoseGroupID; 
        m.name = m_GroupNameInputField.text; ;

        ml.Add(m);

        tmp.group = ml.ToArray();

        List<MsgJson.FuncitonID> addList = new List<MsgJson.FuncitonID>();
       

        foreach (var v in m_FunctionToggleDic) {
            if (v.Value.GetComponent<Toggle>().isOn)
            {
                MsgJson.FuncitonID i = new MsgJson.FuncitonID();
                i.id = v.Key;
                addList.Add(i);
            }
        }

        tmp.addFunctionList = addList.ToArray();

        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("msg", JsonUtility.ToJson(tmp));
        Debug.Log(JsonUtility.ToJson(tmp));

        HttpManager.Instance.SendPostForm(ProjectConst.SaveGroupInfo, form);
    }

    void OnBackButtonClick()
    {
        //加载UsersAndGroupsInfoPanel;
        FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("UsersAndGroupsInfoPanel"));
        //销毁本面板;
        Destroy(gameObject);
    }

    void OnDeleteButtonClick() {
        //加载UsersAndGroupsInfoPanel;
        GameObject go = FrameUtil.AddChild(GameObject.Find("Canvas/Stack"), Resources.Load<GameObject>("ConfirmPanel"));

        go.GetComponent<ConfirmPanelLogic>().Init(DeleteGroupCallBack);
    }

    void DeleteGroupCallBack() {
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", m_CurrentChoseGroupID);
        HttpManager.Instance.SendPostForm(ProjectConst.DelNewGroup, form);
    }



    //----------------------------------------------- Member -----------------------------------------------

    private Button m_SaveButton;
    private Button m_BackButton;
    private Button m_DeleteButton;

    private Transform m_GroupItemList;

    private GameObject m_GroupItem;

    private InputField m_GroupNameInputField;

    private Transform m_FunctionArea;

    private GameObject m_FunctionToggle;

    public string m_CurrentChoseGroupID;

    //key 功能 ID , 功能 toggle 对象;
    private Dictionary<string, GameObject> m_FunctionToggleDic = new Dictionary<string, GameObject>();
}
