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
    //得到尺寸消息;
    S2C_GetSize = 1015,
}