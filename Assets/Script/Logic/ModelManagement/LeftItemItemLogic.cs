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

    }

    void OnDeleteClick() {
        Destroy(gameObject);
    }

    public MsgJson.Item m_Item;

    private Text m_CategroyText;
    private Text m_DesText;
    private Button m_QtyButton;
    private Text m_QtyText;
    private Text m_TotalText;
    private Button m_DeleteButton;
}
