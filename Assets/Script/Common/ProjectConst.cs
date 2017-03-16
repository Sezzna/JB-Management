using UnityEngine;
using System.Collections;

//cyb0301221

public class ProjectConst {
    //登录;
    public static string LoginMsgUrl = "http://jbmanagement.com.au/app/login.php";
    //查看用户权限;
    public static string CheckUserFunction = "http://www.jbmanagement.com.au/app/checkUserFunction.php";

    //----------------------------------------- User Access --------------------------------------------------
    //用户控制;
    public static string UserAccess = "http://www.jbmanagement.com.au/app/getUserAccess.php";

    //保存修改用户访问结果;
    public static string SaveUserAccess = "http://www.jbmanagement.com.au/app/userInfoEdit.php";
    //新加用户;
    public static string AddNewUser = "http://www.jbmanagement.com.au/app/addUser.php";
    //删除用户;
    public static string DeleteUser = "http://www.jbmanagement.com.au/app/delUser.php";


    //新加组
    public static string AddNewGroup = "http://www.jbmanagement.com.au/app/addGroup.php";
    //删除组
    public static string DelNewGroup = "http://www.jbmanagement.com.au/app/delGroup.php";
    //保存修改组信息;
    public static string SaveGroupInfo = "http://www.jbmanagement.com.au/app/groupInfoEdit.php";

    //-------------------------------------------- Model Management -------------------------------------- 
    
    //得到模块种类数据;
    public static string GetRange = "http://www.jbmanagement.com.au/app/getRange.php";
    //得到模块数据;
    public static string GetModel = "http://www.jbmanagement.com.au/app/getModel.php";
    //得到车型详细信息;
    public static string GetModelDetil = "http://www.jbmanagement.com.au/app/getModelDetil.php";

    //获得尺寸数据
    public static string GetSize = "http://www.jbmanagement.com.au/app/getSize.php";
    //添加尺寸数据(添加车型)
    public static string AddSize = "http://www.jbmanagement.com.au/app/addSize.php";

    //获取供货商
    public static string GetSupplier = "http://www.jbmanagement.com.au/app/getSupplier.php";
    //获取一个供货商的Item 信息;
    public static string GetItem = "http://www.jbmanagement.com.au/app/getItem.php";
    //得到AddItem面板的 Item类别消息;
    public static string GetItemCategory = "http://www.jbmanagement.com.au/app/getItemCategory.php";
}

