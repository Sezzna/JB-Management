using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeItemQtyPanelLogic : MonoBehaviour {

    private void Awake()
    {
        m_QtyInputField = transform.FindChild("Panel/QTY/InputField").GetComponent<InputField>();
        m_ConfirmButton = transform.FindChild("Panel/Confirm").GetComponent<Button>();
        m_ConfirmButton.onClick.AddListener(OnConfirmClick);

        m_CancelButton = transform.FindChild("Panel/Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelClick);
    }



	// Use this for initialization
	void Start () {
		
	}

    public void Init(MsgJson.Item item, string qty) {
        m_Item = item;
        m_QtyInputField.placeholder.GetComponent<Text>().text = qty;
    }

    void OnConfirmClick() {
   
        if (ControlPlayer.Instance.m_CurrentPanelName == "SpecialPartsSelectionPanel")
        {
            foreach (var v in ControlPlayer.Instance.m_SpItemList)
            {
                if (v.item.id == m_Item.id)
                {
                    if (m_QtyInputField.text != "")
                    {
                        v.qty = m_QtyInputField.text;
                    }
                }
            }
            GameObject.Find("SpecialPartsSelectionPanel(Clone)").GetComponent<SpecialpartsSelectionPanel>().AddPartItem();
        }
        else {
            foreach (var v in ControlPlayer.Instance.m_CommonItemList)
            {
                if (v.item.id == m_Item.id)
                {
                    if (m_QtyInputField.text != "")
                    {
                        v.qty = m_QtyInputField.text;
                    }
                }
            }
            GameObject.Find("CommonPartsSelectionPanel(Clone)").GetComponent<CommonPartsSelectionPanelLogic>().AddPartItem();
        }
        Destroy(gameObject);
    }

    void OnCancelClick() {
        Destroy(gameObject);
    }

    private InputField m_QtyInputField;

    private Button m_ConfirmButton;
    private Button m_CancelButton;

    private MsgJson.Item m_Item;
}
