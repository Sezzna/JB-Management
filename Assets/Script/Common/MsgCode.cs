// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.


public enum MsgCode
{
    //版本验证;
    S2C_VersionVerification = 1000,

    //登录;
    S2C_UserLogin = 1001,

    //查看用户权限;
    S2C_CheckUserFunction = 1002,

    //用户权限控制;
    S2C_UserAccess = 1003,

    //添加新组;
    S2C_AddNewGroup= 1004,

    //添加新用户;
    S2C_AddNewUser = 1005,

    //删除组;
    S2C_DeleteGroup = 1006,

    //删除用户;
    S2C_DeleteUser = 1007,

    //保存用户信息修改;
    S2C_SaveUserInfo = 1008,

    //保存组功能修改;
    S2C_SaveGroupInfo = 1009,

    //----------------------------------------------- Model Management -------------------------------
    //得到模块类型信息；
    S2C_GetRange = 1012,
    //得到模块信息;
    S2C_GetModel = 1013,
    //得到车型详细信息
    S2C_GetModelDetail = 1014,
    //得到尺寸消息;
    S2C_GetSize = 1015,

    //添加Size 消息返回
    S2C_AddSize = 1016,

    //model management 最后一条 添加车型消息;
    S2C_AddMode = 1017,

    //获取供货商消息返回;
    S2C_GetSupplier = 1010,

    //得到供货商Item
    S2C_GetItem = 1011,

    //得到供货商ItemCategory消息;
    S2C_GetItemCategory = 1019,
    //得到Item Stages 消息
    S2C_GetItemStages = 1020,
    //得到 Item 预填信息;
    S2C_GetItemDisplayStage = 1021,

    //更新Item category和stage 消息;
    S2C_ItemCategoryStageUpdate = 1023
}