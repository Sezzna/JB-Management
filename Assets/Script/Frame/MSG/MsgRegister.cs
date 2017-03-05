// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : 消息注册类,将所有的消息处理函数注册到MAP中,然后在HttpManager里面;
//  * How to use :  
//  *******************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MsgRegister{

	private static MsgRegister s_Instance;

	public static MsgRegister Instance{
		get {
			if (s_Instance == null) {
				s_Instance = new MsgRegister();
			}

			return s_Instance;
		}
	}

	//注册;
	public void Register(short msgID, VoidDelegate handle){
		m_MsgMap[msgID.ToString()] = handle;
	}

    public void Register(short msgID, VoidDelegateString handle)
    {
        m_MsgMapString[msgID.ToString()] = handle;
    }

    //根据消息ID,回调消息处理函数(为了效率没有写容错代码);
    public void Handle(string msgID, byte[] data){
		m_MsgMap[msgID](data);
	}

    public void Handle(string msgID, string data)
    {
        m_MsgMapString[msgID](data);
    }

    //----------------------------------------MEMBER--------------------------------
    //消息处理回调指针;
    public delegate void VoidDelegate(byte[] data);

    public delegate void VoidDelegateString(string data);

    //消息处理map;
    Dictionary<string, VoidDelegate> m_MsgMap = new Dictionary<string, VoidDelegate>();
    Dictionary<string, VoidDelegateString> m_MsgMapString = new Dictionary<string, VoidDelegateString>();
}
