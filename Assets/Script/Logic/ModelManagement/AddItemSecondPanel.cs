using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemSecondPanel : MonoBehaviour {
    void Awake()
    {
        m_ViewToggle = transform.FindChild("Panel/View").GetComponent<Toggle>();
        m_ViewToggle.onValueChanged.AddListener(OnValueChanged);
        m_DisplayToggle = transform.FindChild("Panel/Display").GetComponent<Toggle>();

        m_Cancel = transform.FindChild("Panel/Cancel").GetComponent<Button>();
        m_Cancel.onClick.AddListener(OnCancelClick);
        m_Confirm = transform.FindChild("Panel/Confirm").GetComponent<Button>();
        m_Confirm.onClick.AddListener(OnConfirmClick);


        m_CategoryDropdown = transform.FindChild("Panel/Category/Dropdown").GetComponent<Dropdown>();
    }

	// Use this for initialization
	void Start () {
        UpdateTypeDropdownView();
    }

    //------------------------------------------------------ON BUTTON ----------------------------------------------
    void OnValueChanged(bool val) {
        if (val == true)
        {
            m_DisplayToggle.gameObject.SetActive(true);
        }
        else {
            m_DisplayToggle.gameObject.SetActive(false);
        }
    }

    void OnCancelClick() {
        Destroy(gameObject);
    }

    void OnConfirmClick() {

    }


    private void UpdateTypeDropdownView()
    {
        AddDropdownItemName();
        m_CategoryDropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_CategoryList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_CategoryList[i];
            m_CategoryDropdown.options.Add(tempData);
        }
        m_CategoryDropdown.captionText.text = m_CategoryList[0];
    }

    //设置DorpDown字段名字;
    private void AddDropdownItemName()
    {
        foreach (var v in ControlPlayer.Instance.m_ItemCategory.category) {
            m_CategoryList.Add(v.rank + " " + v.des);
        }
    }

    //---------------------------------- MEMBER ---------------------------------
    private Toggle m_ViewToggle;
    private Toggle m_DisplayToggle;

    private Dropdown m_CategoryDropdown;
    private List<string> m_CategoryList = new List<string>();

    private Button m_Cancel;
    private Button m_Confirm;
        
}
