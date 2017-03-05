// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : WWW 类封装,用于发送WWW消息,和接收消息回调处理函数, 需要配合MsgRegister使用;
//  * How to use :  
//  *******************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class HttpManager {

	private static HttpManager s_Instance;

	public static HttpManager Instance{
		get {
			if (s_Instance == null) {
				s_Instance = new HttpManager();
			}

			return s_Instance;
		}
	}



	//发送(为了配合protobuf 参数为二进制流, 第三个参数为消息ID);
	public void SendPost(short msgID, byte[] data){
		StartCoroutineHelper.Instance.StartCoroutine(SendPostCoroutine(msgID.ToString(), data));
	}

    //Form方式;
    public void SendPostForm(string url, WWWForm form) {
        StartCoroutineHelper.Instance.StartCoroutine(SendPostFormCoroutine(url, form));
    }

	//参数为 url ,data 数据, 消息ID的string形式;
	private IEnumerator SendPostCoroutine(string MsgID, byte[] data){
		//创建消息头;
		Dictionary<string, string> headers = new Dictionary<string, string>();
		//消息头为消息ID;
		headers.Add("MsgID", MsgID);
		
		WWW www_instance = new WWW(m_Url, data, headers);  

		yield return www_instance;  

		if (www_instance.error != null) {
			//这里应该是做超时处理,要通知主逻辑弹出通知面板;
			//后面来做;
			Debug.LogError("------------------------ Send WWW Post Error : " + www_instance.error + "------------------------");  
		} else {
            //在消息注册机里面找对应的消息处理函数;
			MsgRegister.Instance.Handle(www_instance.responseHeaders["MSGID"], www_instance.bytes);
		}
	}

    //参数为 url ,data 数据, 消息ID的string形式;
    private IEnumerator SendPostFormCoroutine(string url, WWWForm form)
    {
        WWW www_instance = new WWW(url, form);

        //开始转菊花图;
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("WaitingPanel"));

        yield return www_instance;

        if (www_instance.error != null)
        {
            //这里应该是做超时处理,要通知主逻辑弹出通知面板;
            Object.DestroyImmediate(GameObject.Find("WaitingPanel(Clone)"));
            //后面来做;
            Debug.LogError("------------------------ Send WWW Post Error : " + www_instance.error + "------------------------");
            GameObject go = FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel"));
            go.GetComponent<NoticePanelLogic>().Init("Send WWW Post Error: " + www_instance.error);
        }
        else
        {
            Object.DestroyImmediate(GameObject.Find("WaitingPanel(Clone)"));

            Debug.Log("----------------------------------------- 收到消息号 : " + www_instance.responseHeaders["code"] + "----------------------------------------");
            Debug.Log(www_instance.text);

            MsgRegister.Instance.Handle(www_instance.responseHeaders["code"], www_instance.text);
        }
    }




    //-------------------------------------------------MEMBER---------------------------------------------------------
    //JB Caravans （Form方式不需要这个url）;
    private string m_Url = "http://jbmanagement.com.au/";

}
