using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MsgJson {
    //登录;
    public class Login {
        public string state;
        public string token;
        public User[] user;
    }

    //用户信息;
    [Serializable]
    public class User {
        public string GroupName;
        public string firstname;
        public string lastname;
        public string email;
        public string phone;
    }

    //用户权限;
    [Serializable]
    public class UserFunction
    {
        public string state;
        public Funciont[] function;
    }

    //权限;
    [Serializable]
    public class Funciont
    {
        public string id;
        public string name;
    }

    //用户控制;
    [Serializable]
    public class UserAccess {
        public string state;
        public Users[] users;
        public Groups[] groups;
        public Funciont[] function;
        public UserGroupLink[] userGroupLink;
        public FunctionGroupLink[] functionGroupLink;
    }

    //保存用户信息界面消息结构;
    [Serializable]
    public class SaveUserInfo
    {
        public Users[] users;
        public GroupID[] addGroupList;
    }

    //组ID
    [Serializable]
    public class GroupID
    {
        public string id;
    }

    //保存组界面组功能信息结构;
    [Serializable]
    public class SaveGroupInfo {
        public Groups[] group;
        public FuncitonID[] addFunctionList;
        //public FuncitonID[] delFunctionList;
    }

    //功能id(我日 转JSON 好麻烦)
    [Serializable]
    public class FuncitonID
    {
        public string id;
    }

    //添加新组;
    [Serializable]
    public class AddNewGroup
    {
        public string state;
        public string id;
        public string name;
    }

    //添加新用户;
    [Serializable]
    public class AddNewUser
    {
        public string state;
        public string id;
        public string username;
        public string firstname;
        public string lastname;
        public string email;
        public string phone;
    }

    //删除组;
    [Serializable]
    public class DeleteGroup
    {
        public string state;
    }

    //删除用户;
    [Serializable]
    public class DeleteUser
    {
        public string state;
    }

    //保存组信息返回;
    [Serializable]
    public class SaveGroupInfoBack
    {
        public string state;
    }

    //保存用户信息返回;
    [Serializable]
    public class SaveUserInfoBack
    {
        public string state;
    }

    [Serializable]
    public class Users {
        public string id;
        public string username;
        public string email;
        public string firstname;
        public string lastname;
        public string phone;
    }

    [Serializable]
    public class Group
    {
        public string id;
        public string groupId;
    }

    [Serializable]
    public class Groups {
        public string id;
        public string name;
    }

    [Serializable]
    public class UserGroupLink
    {
        public string id;
        public string userId;
        public string groupId;
    }

    [Serializable]
    public class FunctionGroupLink
    {
        public string id;
        public string groupId;
        public string functionId;
    }

    //---------------------------------------------------------------------------- Model Management -----------------------------------------------------
    //模型样式;
    [Serializable]
    public class ModelRange {
        public Range[] range;
    }

    [Serializable]
    public class Range
    {
        public string id;
        public string description;
        public string brand;
    }

    //车型数据(尺寸数据);
    [Serializable]
    public class CarModels {
        public CarModel[] models;
    }

    [Serializable]
    public class CarModel
    {
        public string id;
        public string code;
        public string model_year;
        public string status;
    }

    //车型详细数据
    [Serializable]
    public class ModelsDetail {
        public ModelDetail[] models;
        public Size[] size; 
    }

    [Serializable]
    public class ModelDetail {
        public string id;
        public string range_id;
        public string model_year;
        public string version;
        public string status;
        public string code;
    }

    //AddModel面板的Size消息用结构体(1015消息)
    [Serializable]
    public class AddModelSize
    {
        public string state;
        public Size[] size;
    }

    //1017 消息结构体;
    [Serializable]
    public class AddSize
    {
        public string state;
        public string id;
        public string size;
        public string doorPosition;
        public string type;
        public string note;
    }

    [Serializable]
    public class Size
    {
        public string id;
        public string size;
        public string doorPosition;
        public string type;
        public string note;
    }

    //获得供货商消息结构;
    [Serializable]
    public class GetSupplier
    {
        public string state;
        public Supplier[] supplier;
    }

    [Serializable]
    public class Supplier {
        public string id;
        public string Name;
    }

    //供货商Items
    [Serializable]
    public class Items
    {
        public string state;
        public Item[] item;
    }

    //供货商Item
    [Serializable]
    public class Item {
        public string id;
        public string product_code;
        public string description;
        public string units;
        public string unit_price;
        public string discount;
        public string category_id;
    }
}