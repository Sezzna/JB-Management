using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlPlayer {
    private static ControlPlayer s_Instance;

    public static ControlPlayer Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new ControlPlayer();
            }

            return s_Instance;
        }
    }

    //----------------------------------------------------------- Function --------------------------------------------------
    //用户控制数据转换;
    public void DataFormatConversion(MsgJson.UserAccess data) {
        //先清空数据;
        m_UserInfoDic.Clear();
        m_UserHaveGroupDic.Clear();
        m_GroupFunctionDic.Clear();

        //用户所拥有的功能组;
        foreach (var i in data.userGroupLink) {
            if (m_UserHaveGroupDic.ContainsKey(i.userId))
            {
                m_UserHaveGroupDic[i.userId].Add(i.groupId);
            }
            else {
                List<string> tmpGroupIdList = new List<string>();
                tmpGroupIdList.Add(i.groupId);
                m_UserHaveGroupDic[i.userId] = tmpGroupIdList;
            }
        }

        //所有用户;
        foreach (var v in data.users) {
            UserInfo tmp = new UserInfo();

            tmp.userID = v.id;
            tmp.userName = v.username;
            tmp.firstName = v.firstname;
            tmp.lastName = v.lastname;
            tmp.email = v.email;
            tmp.phone = v.phone;

            m_UserInfoDic[v.id] = tmp;
        }

        //所有组各自拥有的功能;
        foreach (var v in data.functionGroupLink) {
            if (m_GroupFunctionDic.ContainsKey(v.groupId))
            {
                m_GroupFunctionDic[v.groupId].Add(v.functionId);
            }
            else {
                List<string> tmp = new List<string>();
                tmp.Add(v.functionId);
                m_GroupFunctionDic[v.groupId] = tmp;
            }
        }
    }

    //----------------------------------------------------------- MEMBER --------------------------------------------------
    public bool m_IsNeedAutoLogin = true;

    public string m_GroupName;
    public string m_FirstName;
    public string m_LastName;
    public string m_Email;
    public string m_Phone;

    //用户控制模块数据;
    public MsgJson.UserAccess m_UserAccessData;

    //用户信息 key 为 UserID;
    public Dictionary<string, UserInfo> m_UserInfoDic = new Dictionary<string, UserInfo>();

    //用户所拥有的功能组key 用户id , value 组id List;
    public Dictionary<string, List<string>> m_UserHaveGroupDic = new Dictionary<string, List<string>>();

    //组所拥有的功能Dic key 为 组ID,value 为 功能 id;
    public Dictionary<string, List<string>> m_GroupFunctionDic = new Dictionary<string, List<string>>();

    //当前正在编辑的用户ID;
    public string m_CurrentEditUserID = "1";

    //-------------------------ModelMangement模块--------------------------

    //模块信息;
    public MsgJson.ModelRange m_ModelsRange;
    //车型信息(尺寸信息);
    public MsgJson.CarModels m_CarModels;
    //车型详细信息;
    public MsgJson.ModelsDetail m_ModelsDetail;
    //当前选择的Range的Brand
    public string m_CurrentRangeBrand;

    //-------------------------------------------------------- Control Player 使用数据结构 --------------------------------------------------------
    public class UserInfo {
        public string userID;
        public string userName;
        public string firstName;
        public string lastName;
        public string email;
        public string phone;    
    }


}


