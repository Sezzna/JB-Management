using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftItemItemLogic : MonoBehaviour {
    private void Awake()
    {
        m_CodeText = transform.FindChild("Code/Text").GetComponent<Text>();
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
        m_QtyText.text = qty;
        m_CodeText.text = item.product_code;
        m_DesText.text = item.description;
        m_TotalText.text = item.unit_price;
    }

    void OnQtyClick() {

    }

    void OnDeleteClick() {
        Destroy(gameObject);
    }




    private Text m_CodeText;
    private Text m_DesText;
    private Button m_QtyButton;
    private Text m_QtyText;
    private Text m_TotalText;
    private Button m_DeleteButton;
}
