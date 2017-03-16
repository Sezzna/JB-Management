using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPartsSelectionPanelLogic : MonoBehaviour {
    void Awake(){
        m_SupplierList = transform.FindChild("SupplierList/Viewport/Content");
        m_ItemList = transform.FindChild("ItemList/Viewport/Content");

        m_ItemItem = Resources.Load("ItemItem") as GameObject;

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItem, OnGetItem);
    }


    // Use this for initialization
    void Start () {
        foreach (var v in ControlPlayer.Instance.m_GetSupplier.supplier) {
            FrameUtil.AddChild(m_SupplierList.gameObject, Resources.Load<GameObject>("SupplierItem")).GetComponent<SupplierItemLogic>().Init(v);
        }
	}

    void OnGetItem(string data) {
        foreach (Transform child in m_ItemList)
        {
            Destroy(child.gameObject);
        }

        MsgJson.Items items = JsonUtility.FromJson<MsgJson.Items>(data);

        foreach (var v in items.item) {
            FrameUtil.AddChild(m_ItemList.gameObject, m_ItemItem).GetComponent<ItemItemLogic>().Init(v);
        }
    }


    private Transform m_SupplierList;
    private Transform m_ItemList;

    private GameObject m_ItemItem;
}
