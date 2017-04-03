using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftOpItemLogic : MonoBehaviour{
    private void Awake(){
        m_CodeText = transform.FindChild("Code/Text").GetComponent<Text>();
        m_DescriptionText = transform.FindChild("Description/Text").GetComponent<Text>();
        m_QtyText = transform.FindChild("Qty/Text").GetComponent<Text>();
        m_CostText = transform.FindChild("Cost/Text").GetComponent<Text>();
        m_ExtraText = transform.FindChild("Extra/Text").GetComponent<Text>();

        m_QtyButton = transform.FindChild("Qty").GetComponent<Button>();
        m_QtyButton.onClick.AddListener(OnQtyClick);

        m_DeleteButton = transform.FindChild("Delete").GetComponent<Button>();
        m_DeleteButton.onClick.AddListener(OnDeleteClick);
    }

    void Init(string s) {
        
    }

    void OnQtyClick() {

    }

    void OnDeleteClick() {

    }



    private Button m_QtyButton;
    private Button m_DeleteButton;

    private Text m_CodeText;
    private Text m_DescriptionText;
    private Text m_QtyText;
    private Text m_CostText;
    private Text m_ExtraText;
}
