﻿using UnityEngine;
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

    //当先选中的 供货商ID;
    public string m_CurrentSupplierID = "0";



    //-------------------------ModelMangement模块--------------------------
    //当前正在操作的界面;
    public string m_CurrentPanelName = "";

    //模块信息;
    public MsgJson.ModelRange m_ModelsRange;
    //车型信息(尺寸信息);
    public MsgJson.CarModels m_CarModels;
    //车型详细信息;
    public MsgJson.ModelsDetail m_ModelsDetail;
    //AddModel面板 1015消息结构;
    public MsgJson.AddModelSize m_AddModelSize;
    //当前选择的Range的Brand
    public string m_CurrentRangeBrand;
    //1010 供货商信息;
    public MsgJson.GetSupplier m_GetSupplier;
    //1019 ItemCategory
    public MsgJson.ItemCategory m_ItemCategorys;
    //1020 ItemStages
    public MsgJson.ItemStages m_ItemStages;

    //车辆版本号?
    public int m_Version = 1;

    //--------------------------------------------------------------------------------------------
    //选择加载到common parts selected panel 左边的item数据;
    public List<CommonItem> m_CommonItemList = new List<CommonItem>();

    //这个是 common parts selected panel 左边的Stage 数据;
    public List<StageDisplay> m_StageDisplayList = new List<StageDisplay>();

  

    //第二界面使用的全局数据
    public class StageDisplay {
        public string stegeId;
        public string itemId;
        public string rank;
    }


    public class CommonItem {
        public MsgJson.Item item;
        public string qty;
        public string categoryRank;
        public bool displayToCustomer;
    }

    //第三界面使用的全局数据;
    //这个是 special parts selected panel 左边的Stage 数据;
    public List<SpStageDisplay> m_SpStageDisplayList = new List<SpStageDisplay>();
    //special parts selected panel 需要的数据;
    public List<SpItem> m_SpItemList = new List<SpItem>();
    //当前选择的哪一个尺寸;
    public string m_CurrentChoiceSizeId;

    public class SpStageDisplay
    {
        public string stegeId;
        public string itemId;
        public string rank;
        public string sizeId;
    }

    //特殊部件结构体;
    public class SpItem {
        public MsgJson.Item item;
        public string qty;
        public string categoryRank;
        public bool displayToCustomer;
        public string sizeId;
    }

    //最后一个面板的全局数据;
    public List<OpItem> m_OpItemList = new List<OpItem>();
    public List<OpStageDisplay> m_OpStageDisplayList = new List<OpStageDisplay>();

    public List<string> m_NameList = new List<string>();

    public class OpStageDisplay
    {
        public string stegeId;
        public string itemId;
        public string rank;
        public string name;
        public string sizeId;
    }

    public class OpItem {
        public MsgJson.Item item;
        public string qty;
        public string categoryRank;
        public bool displayToCustomer;
        public string sizeId;
        public string name;
        public string standardOrOptional;
        public string Extra;
    }

    //保存 1022 消息数据;
    public MsgJson.SaveAddModelMsg m_SaveAddModelMsg;

    //-------------------------ModelMangement模块在每个界面 Next 以后 要保存的数据;--------------------------
    //AddModelPanel模块要保存的数据;
    public class AddModelPanelSaveData {
        public string m_Brand;
        public string m_Model;
        public string m_ModelCode;
        public string m_ModelYear;
        public string m_ChassisType;
        public List<MsgJson.Size> m_Size = new List<MsgJson.Size>();
    }

    public AddModelPanelSaveData m_AddModelPanelSaveData = new AddModelPanelSaveData();

    //根据AddModelPanel面板数据生成的名字,用于后续的界面;
    public string m_ModelName;

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


