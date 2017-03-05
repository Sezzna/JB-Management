// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : UI 事件监听类;
//  * How to use :  Example usage: EventListener.Get(gameObject).onClick += MyClickFunction;
//  * 根据需要自行添加响应事件;
//  *******************************************************/




using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventListener : EventTrigger {
	//静态调用事件注册;
	static public EventListener Get (GameObject go)
	{
		EventListener listener = go.GetComponent<EventListener>();
		if (listener == null){
			listener = go.AddComponent<EventListener>();
		} 
		return listener;
	}


	public override void OnPointerDown(PointerEventData  eventData){if (onDown != null) onDown(gameObject, eventData);}
	public override void OnPointerClick(PointerEventData  eventData){if (onClick != null) onClick(gameObject, eventData);}
	public override void OnPointerUp(PointerEventData  eventData){if (onUp != null) onUp(gameObject, eventData);}

	public override void OnBeginDrag(PointerEventData  eventData){if (onBeginDrag != null) onBeginDrag(gameObject, eventData);}
	public override void OnDrag(PointerEventData  eventData){if (onDrag != null) onDrag(gameObject, eventData);}
	public override void OnEndDrag(PointerEventData  eventData){if (onEndDrag != null) onEndDrag(gameObject, eventData);}

	public override void OnScroll (PointerEventData eventData){ if (onScroll != null) onScroll(gameObject, eventData); }

	//public override void OnPointerPress(PointerEventData  eventData){if (onPress != null) onPress(gameObject);}


//	void OnSubmit ()				{ if (onSubmit != null) onSubmit(gameObject); }
//	void OnClick ()					{ if (onClick != null) onClick(gameObject); }
//	void OnDoubleClick ()			{ if (onDoubleClick != null) onDoubleClick(gameObject); }
//	void OnHover (bool isOver)		{ if (onHover != null) onHover(gameObject, isOver); }
//	void OnPress (bool isPressed)	{ if (onPress != null) onPress(gameObject, isPressed); }
//	void OnSelect (bool selected)	{ if (onSelect != null) onSelect(gameObject, selected); }
//	void OnScroll (float delta)		{ if (onScroll != null) onScroll(gameObject, delta); }
//	void OnDrag (Vector2 delta)		{ if (onDrag != null) onDrag(gameObject, delta); }
//	void OnDrop (GameObject go)		{ if (onDrop != null) onDrop(gameObject, go); }
//	void OnInput (string text)		{ if (onInput != null) onInput(gameObject, text); }
//	void OnKey (KeyCode key)		{ if (onKey != null) onKey(gameObject, key); }


	
	


//-------------------------------------------------------------MEMBER--------------------------------------

	public delegate void VoidDelegate(GameObject go, PointerEventData eventData);

	//Down;
	public VoidDelegate onDown;
	//Up
	public VoidDelegate onUp;
	//Click;
	public VoidDelegate onClick;

	public VoidDelegate onBeginDrag;
	public VoidDelegate onDrag;
	public VoidDelegate onEndDrag;

	//Scroll
	public VoidDelegate onScroll;

	//press(持续按下)
	//public VoidDelegate onPress;





//	NGUI;
//	public delegate void VoidDelegate (GameObject go);
//	public delegate void BoolDelegate (GameObject go, bool state);
//	public delegate void FloatDelegate (GameObject go, float delta);
//	public delegate void VectorDelegate (GameObject go, Vector2 delta);
//	public delegate void StringDelegate (GameObject go, string text);
//	public delegate void ObjectDelegate (GameObject go, GameObject draggedObject);
//	public delegate void KeyCodeDelegate (GameObject go, KeyCode key);
//	
//	public object m_Parameter;
//
//
//	public VoidDelegate onSubmit;
//	public VoidDelegate onClick;
//	public VoidDelegate onDoubleClick;
//
//	public BoolDelegate onHover;
//	public BoolDelegate onPress;
//	public BoolDelegate onSelect;
//
//	public FloatDelegate onScroll;
//
//	public VectorDelegate onDrag;
//
//	public ObjectDelegate onDrop;
//
//	public StringDelegate onInput;

//	public KeyCodeDelegate onKey;


}
