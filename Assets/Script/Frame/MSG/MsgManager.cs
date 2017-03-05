//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : 消息管理类,用于注册消息等功能,在客户端启动时Init(),由于这是框架功能,又涉及了具体的逻辑消息,使用框架时将Init里面的删除就好;
//  * How to use :  搭配MsgRegister使用
//  *******************************************************/


using UnityEngine;
using System.Collections;


public class MsgManager{
	
	private static MsgManager s_Instance;

	public static MsgManager Instance{
		get {
			if (s_Instance == null) {
				s_Instance = new MsgManager();
			}

			return s_Instance;
		}
	}

	//这个函数要在客户端启动的时候调用,将所有消息注册到MsgRegister里面用于处理消息;
	public void Init(){
		//防止重复注册消息;
		if(m_IsInitialized){
			return;
		}
		//登录消息;

        m_IsInitialized = true;
	}

	//--------------------------------------------MEMBER-----------------------------------------
	//是否执行初始化;
	private bool m_IsInitialized = false;
}
