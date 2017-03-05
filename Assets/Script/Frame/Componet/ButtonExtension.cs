// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : 按钮扩展类,结合带各种默认带有DoTween的效果封装一个按钮,带有默认效果,可扩展,这个类的目的只是为了增加默认的按钮效果,可以在里面多写几个效果,然后动态的用枚举选择;
//  * How to use :  在要使用扩展按钮效果的地方,直接定义这个类,new出来,然后就想给事件回调一样给回调就行了;
//  *******************************************************/



using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonExtension : MonoBehaviour {
	//Button 效果模式;
	public enum EffectMode{
		A,
		B,
		C
	}





	//外部调用,设置按钮对象;
	public ButtonExtension Set(GameObject button){
		m_Button = button;
		EventListener.Get(m_Button).onDown = OnButtonDownDoShrink;
		EventListener.Get(m_Button).onUp = OnButtonUpDoOutBounce;
		EventListener.Get(m_Button).onClick = OnButtonClick;
		return this;
	}

	//多模式按钮效果选择,有需要的新增配套效果,直接在这里面组合就是了
	public void Set(GameObject button, EffectMode mode){
		m_Button = button;

		switch(mode){
		case EffectMode.A:
			EventListener.Get(m_Button).onDown = OnButtonDownDoShrink;
			EventListener.Get(m_Button).onUp = OnButtonUpDoOutBounce;
			EventListener.Get(m_Button).onClick = OnButtonClick;
			break;
		case EffectMode.B:

			break;
		case EffectMode.C:
			
			break;
		default:
			Debug.LogError("---------------- Select button effect mode Error ! ------------------");
			break;
		}


	}

	//按下响应;
	void OnButtonDownDoShrink(GameObject sender, PointerEventData  eventData){
		m_Button.transform.DOScale(0.9f, 0.1f);
		if(null != OnDown){
			OnDown(sender, eventData);
		}
	}

	//抬起响应;
	void OnButtonUpDoOutBounce(GameObject sender, PointerEventData  eventData){
		m_Button.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
		if(null != OnUp){
			OnUp(sender, eventData);
		}
	}

	//点击响应;
	void OnButtonClick(GameObject sender, PointerEventData  eventData){
		if(null != OnClick){
			OnClick(sender, eventData);
		}
	}

	//-----------------------------------------MEMBER------------------------------------------------

	//Button对象,由外部设置;
	public GameObject m_Button;
    //按钮点击是否有效(某些特殊场合使用,如牛牛的清零按钮);
    public bool m_IsEffective = true;
	//Button效果模式;
	EffectMode m_EffectMode;
	//事件回调指针;
	public delegate void VoidDelegate(GameObject go, PointerEventData  eventData);
	//按下;
	public VoidDelegate OnDown;
	//抬起;
	public VoidDelegate OnUp;
	//点击;
	public VoidDelegate OnClick;

}
