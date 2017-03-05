// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : UNITY 专用携程助手(有两种做法 一 : 伪单列模式挂在 GameManager上 二: 真单例模式 哈哈哈但是不能继承MonoBehaviour哦!~);
//  * How to use :  目前采用挂在GameManager上的做法哦,记住必须挂在StartScene的GameManager上,否则无法使用;
//  * 使用的时候还要注意,这种假单列的方式,使用的时候还要注意不能在被挂对象实例化之前使用,否则出错,应为正真的能Instance的时间是在Awake之后;
//  *******************************************************/



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartCoroutineHelper : MonoBehaviour{
	//Call类哦;
	private class Call{
		//构造哦,碉堡了(实际就是生成一个回调类,一个回调函数一个类,带状态哦!~);
		public Call(Callback callback){
			m_Callback = callback;
			m_State = EState.Wait;
		}
		
		//不解释(实时updata 看是不是需要移除);
		//注意一个回调从等待到执行完毕需要3帧的时间(在未知效率的情况下暂时这么写着,测试完了来优化);
		public bool Update(){
			Debug.Log("StartCoroutineHelper 终于用上你了 来注释掉我把!!");
			bool needRemove = false;
			
			switch(m_State){
			case EState.Wait :
				m_State = EState.Work;
				needRemove = false;
				break;
			case EState.Work :
				if(null != m_Callback){
					m_Callback();
				}
				m_State = EState.Over;
				break;
			case EState.Over : 
				needRemove = true;
				break;
			}
			
			return needRemove;
		}
		//状态枚举;
		private enum EState{
			Wait, 
			Work, 
			Over
		}
		
		//回调函数;
		private Callback m_Callback;
		//Call状态;
		private EState m_State;
	}

	//只能单列方式使用;
	private StartCoroutineHelper(){
		//注意:假单列在构造中不能写代码,会被执行两次的;
	}

	//注意这里必须是private(不解释);
	static private StartCoroutineHelper s_Instance = null;
	//不解释;
	static public StartCoroutineHelper Instance {
		get {
			if (s_Instance == null) {
				Debug.LogError("---------------- StartCoroutineHelper Instance not exist !!! ------------------");
			}
			return s_Instance;

		}
	}
	//不解释(又见假单列,优化的时候一定要弄了它,现在为了开发速度暂时忍了我);
	void Awake(){
		s_Instance = this;

		m_NextFrameCalls = new List<Call>();
		m_NeedRemove = new List<Call>();

        DontDestroyOnLoad(transform);
	}

	//加入要在框架帧中执行的回调;
	public void AddNextFrame(Callback callback){
		Call call = new Call(callback);
		m_NextFrameCalls.Add(call);
	}

	//注意哦,Late哦;
	void LateUpdate(){
		for(int i = 0 ; i < m_NextFrameCalls.Count ; ++i){
			Call call = m_NextFrameCalls[i];
			bool needRemove = call.Update();
			if(needRemove){
				m_NeedRemove.Add(call);
			}
		}

		for(int i = 0 ; i < m_NeedRemove.Count ; ++i){
			Call call = m_NeedRemove[i];
			m_NextFrameCalls.Remove(call);
		}

		m_NeedRemove.Clear();
	}
	
	//执行携程;
	public Coroutine ProcessWork(IEnumerator iterationResult){
		return StartCoroutine(iterationResult);
	}



	//-------------------------------------------------------MEMBER-----------------------------------------------
	//C# 特有的代理模式用作回调注意 Callback 是一个单词 哈哈哈哈!(其实这玩意就是一个函数指针);
	public delegate void Callback();

	//下一帧要调用的回调;
	private List<Call> m_NextFrameCalls;
	//已经调用完毕需要移除的回调;
	private List<Call> m_NeedRemove;
}
