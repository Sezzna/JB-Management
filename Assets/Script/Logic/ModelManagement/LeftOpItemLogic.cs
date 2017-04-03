using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftOpItemLogic : MonoBehaviour{
    private void Awake(){
        m_CodeText = transform.FindChild("Code/Text").GetComponent<Text>();
        m_DescriptionText = transform.FindChild("Description/Text").GetComponent<Text>();
        m_OptionText = transform.FindChild("Option/Text").GetComponent<Text>();
        m_QtyText = transform.FindChild("Qty/Text").GetComponent<Text>();
        m_CostText = transform.FindChild("Cost/Text").GetComponent<Text>();
        m_ExtraText = transform.FindChild("Extra/Text").GetComponent<Text>();

        m_QtyButton = transform.FindChild("Qty").GetComponent<Button>();
        m_QtyButton.onClick.AddListener(OnQtyClick);

        m_DeleteButton = transform.FindChild("Delete").GetComponent<Button>();
        m_DeleteButton.onClick.AddListener(OnDeleteClick);
    }

    public void Init(MsgJson.Item item, string option, string qty, string cost, string extra) {
        m_Item = item;
        m_CodeText.text = item.product_code;
        m_DescriptionText.text = item.description;
        m_OptionText.text = option;
        m_QtyText.text = qty;
        m_CostText.text = "$"+cost;
        m_ExtraText.text = extra;
    }

    void OnQtyClick() {
        FrameUtil.AddChild(GameObject.Find("Canvas/Other"), Resources.Load<GameObject>("ChangeItemQtyPanel")).GetComponent<ChangeItemQtyPanelLogic>().Init(m_Item, m_QtyText.text);
    }

    void OnDeleteClick() {

    }

    private Button m_QtyButton;
    private Button m_DeleteButton;

    private MsgJson.Item m_Item;

    private Text m_CodeText;
    private Text m_DescriptionText;
    private Text m_OptionText;
    private Text m_QtyText;
    private Text m_CostText;
    private Text m_ExtraText;
}
