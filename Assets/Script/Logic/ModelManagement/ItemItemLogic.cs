using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemItemLogic : MonoBehaviour {
    private void Awake()
    {
        m_Code = transform.FindChild("Code/Text").GetComponent<Text>();
        m_Description = transform.FindChild("Description/Text").GetComponent<Text>();
        m_Units = transform.FindChild("Units/Text").GetComponent<Text>();
        m_Price = transform.FindChild("Price/Text").GetComponent<Text>();
        m_Add = transform.FindChild("Add/Text").GetComponent<Button>();
    }


    // Use this for initialization
    void Start () {
		
	}

    public void Init(MsgJson.Item item)
    {
        m_Id = item.id;
        m_Code.text = item.product_code;
        m_Description.text = item.description;
        m_Units.text = item.units;
        m_Price.text = "$" + item.unit_price;
        m_Discount = item.discount;
        m_Category_id = item.category_id;
    }


    
    private Text m_Code;
    private Text m_Description;
    private Text m_Units;
    private Text m_Price;
    private Button m_Add;

    private string m_Id;
    private string m_Discount;
    private string m_Category_id;
}
