using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftItemItemLogic : MonoBehaviour {
    private void Awake()
    {
        m_CategroyText = transform.FindChild("Categroy/Text").GetComponent<Text>();
        m_DesText = transform.FindChild("Description/Text").GetComponent<Text>();
        m_QtyText = transform.FindChild("Qty/Text").GetComponent<Text>();
        m_TotalText = transform.FindChild("Total/Text").GetComponent<Text>();

        m_QtyButton = transform.FindChild("Qty").GetComponent<Button>();
        m_QtyButton.onClick.AddListener(OnQtyClick);

        m_DeleteButton = transform.FindChild("Delete").GetComponent<Button>();
        m_DeleteButton.onClick.AddListener(OnDeleteClick);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(MsgJson.Item item, string qty) {
        m_Item = item;
        m_QtyText.text = qty;
        foreach (var v in ControlPlayer.Instance.m_ItemCategorys.category) {
            if (v.id == item.category_id){
                m_CategroyText.text = v.des;
                break;
            }
        }
        
        m_DesText.text = item.description;
        m_TotalText.text = item.unit_price;
    }

    void OnQtyClick() {
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("ChangeItemQtyPanel")).GetComponent<ChangeItemQtyPanelLogic>().Init(m_Item,m_QtyText.text);
    }

    void OnDeleteClick() {
        //删除;
        //删除之前的ItemStages
        int check = 1;
        while (check == 1)
        {
            check = 0;
            foreach (var i in ControlPlayer.Instance.m_StageDisplayList)
            {
                if (i.itemId == m_Item.id)
                {
                    ControlPlayer.Instance.m_StageDisplayList.Remove(i);
                    check = 1;
                    break;
                }
            }
        }

        //判断左边list里面有没有 这个item选项;
        foreach (var i in ControlPlayer.Instance.m_CommonItemList)
        {
            //如果有就Remove掉;
            if (i.item.id == m_Item.id)
            {
                ControlPlayer.Instance.m_CommonItemList.Remove(i);
                break;
            }
        }


        //调用CommonPartsSelectionPanel面板的AddPartItem函数刷新左边的Item
        GameObject.Find("CommonPartsSelectionPanel(Clone)").GetComponent<CommonPartsSelectionPanelLogic>().AddPartItem();
        //Destroy(gameObject);
    }

    public MsgJson.Item m_Item;

    private Text m_CategroyText;
    private Text m_DesText;
    private Button m_QtyButton;
    private Text m_QtyText;
    private Text m_TotalText;
    private Button m_DeleteButton;
}
