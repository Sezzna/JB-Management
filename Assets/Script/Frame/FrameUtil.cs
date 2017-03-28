// /*******************************************************
//  * Copyright (C) 2015 Aden
//  * 
//  * This file is part of Gambling Game.
//  * 
//  * Gambling Game can not be copied and/or distributed without the express
//  * permission of Aden.
//
//  * Author : Sezzna.
//  * Description : 特殊的功能工具,这个类基本算是应用层的接口类,比如加载Prefab 这里来关心是在Bundle里面还是Resource里面但是又不会涉及到bundle的load基本上就是个中转工具函数类;
//  * How to use :  能够在这里添加使用的函数就尽量不再去框架代码中添加了;
//  *******************************************************/




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class FrameUtil{
	//开始更新资源;
	//public static void StartUpdateResource(string netResourceListFileName){
	//	AssetBundleLoader.Instance.StartUpdateResource(netResourceListFileName);
	//}

	////资源是否更新完成;
	//public static bool IsUpdateResourceComplete(){
	//	return AssetBundleLoader.Instance.GetIsReady();
	//}

	////开始加载资源(UI以及各种资源都在这里面加载);
	//public static void StartLoadResource(){
	//	ResourceLoader.Instance.StartLoad();
	//}

	////是否加载资源完成;
	//public static bool IsLoadResourceComplete(){
	//	return ResourceLoader.Instance.GetIsLoadResourceComplete();
	//}

	////清除所有加载的包,缓存;
	//public static void ClearLoadedResource(){
	//	ResourceLoader.Instance.Clear();
	//}

	////加载关卡,也就是加载场景(第二个参数如果为false 也就是说 这个场景是加入了 playerSetting中的,为true 的话就要去找到底是下载的 还是在本地的);
	//public static void LoadLevel(string sceneName, bool isUseResourceMgr){
	//	if(sceneName != m_CurrentSceneName){
	//		//两种加载方式;
	//		if(!isUseResourceMgr){
	//			//已废弃;
	//			//Application.LoadLevel(sceneName);
	//			SceneManager.LoadScene(sceneName);
	//		}
	//		else{
	//			//注意使用了这个加载的话,注意对应资源名字,这里面就爱在的是_copy的资源;
	//			ResourceManager.Instance.LoadLevel(sceneName);
	//		}
	//		m_CurrentSceneName = sceneName;
	//	}
	//}

	//加载Prefab;
	public static GameObject GetPrefab(string prefabName){
		GameObject result = null;
		//首先在本地的Resources下面找;
		result = Resources.Load(prefabName) as GameObject;
		if(result){
			
			return result;
		}
		else{
			//这里应该调用 AssetBundleLoader来处理,等功能写完来改;
			//直接从Bundle中读取;
			//if(m_IsUseAssetBundle){
			//	if(null != m_Bundle  && m_Bundle.Contains(prefabName)){
			//		return result = m_Bundle.LoadAsset(prefabName) as GameObject;
			//	}
			//}
			//在解Bundle的缓存中读取;
			//else{
			//	if(null != m_Cache && m_Cache.ContainsKey(prefabName)){
			//		return result = m_Cache[prefabName];
			//	}
			//}
		}

		if(null == result){
			Debug.LogError("---------------- FrameUtil.GetPrefab() Error prefab is null : " + prefabName + " ------------------");
		}
		return result;
	}


	//添加子对象
	public static GameObject AddChild(GameObject parent, GameObject prefab){
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		//注册撤销,我现在还没想好到底用不用,看效果再说;
		//UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");

		if(null != go && null != parent){
			Transform t = go.transform;
			t.SetParent(parent.transform, false);
			//t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		else{
			Debug.LogError("---------------- FrameUtil AddChild() Instantiate Error ! ------------------");
		}
		return go;
	}

	//添加子对象
	public static GameObject AddChild(GameObject parent, GameObject prefab, int index){
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		//注册撤销,我现在还没想好到底用不用,看效果再说;
		//UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
		
		if(null != go && null != parent){
			Transform t = go.transform;
			t.SetParent(parent.transform, false);
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
			t.SetSiblingIndex(index);
		}
		else{
			Debug.LogError("---------------- FrameUtil AddChild() Instantiate Error ! ------------------");
		}
		return go;
	}

	
	//添加Mask对象,默认把Mask放在当前需要Mask的Layer的上一Layer, 如果如要手动控制,那么就需要首先isControl 要为true, 而后指定Index的层数; 
	public static GameObject AddMask(GameObject parent, GameObject prefab, bool isControl = false, int index = 1){
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		//注册撤销,我现在还没想好到底用不用,看效果再说;
		//UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
		
		if(null != go && null != parent){
			Transform t = go.transform;
			t.SetParent(parent.transform, false);
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
			if(!isControl){
				go.transform.SetSiblingIndex(go.transform.GetSiblingIndex() - 1);
			}
			else{
				go.transform.SetSiblingIndex(index);
			}


		}
		else{
			Debug.LogError("---------------- FrameUtil AddMask() Instantiate Error ! ------------------");
		}
		return go;
	}



	//移除子对象(所有)(先写着带测试);
	public static void RemoveAllChild(Transform root){
		if(null == root){
			return;
		}

		int index = 0;
		while(index < root.transform.childCount){
			Transform childTr = root.transform.GetChild(index);

			if(null != childTr){
				GameObject.DestroyImmediate(childTr.gameObject);
			}
			else{
				++index;
			}
		}
	}

	//复制组建(将一个组建的内容属性,复制到另外一个对象身上);
	public static T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		var dst = destination.GetComponent(type) as T;
		if (!dst) dst = destination.AddComponent(type) as T;
		var fields = type.GetFields();
		foreach (var field in fields)
		{
			if (field.IsStatic) continue;
			field.SetValue(dst, field.GetValue(original));
		}
		var props = type.GetProperties();
		foreach (var prop in props)
		{
			if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
			prop.SetValue(dst, prop.GetValue(original, null), null);
		}
		return dst as T;
	}

    /// <summary>
    /// 在Other层弹出提示面板参数为提示面板要显示的字符串;
    /// </summary>
    /// <param name="str"></param>
    public static void PopNoticePanel(string str) {
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("NoticePanel")).GetComponent<NoticePanelLogic>().Init(str);
    }

    //--------------------------------------------------------------MEMBER------------------------------

    //是否使用AssetBundle;
    public static bool m_IsUseAssetBundle = false;
	//AssetBundle;
	private static AssetBundle m_Bundle = null;
	//资源缓存(从bundle中读取出来放里面的);
	private static Dictionary<string, GameObject> m_Cache = new Dictionary<string, GameObject>();
	//当前场景名;
	private static string m_CurrentSceneName;


}
