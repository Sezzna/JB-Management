using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemGroupNamePanelLogic : MonoBehaviour {
    private void Awake()
    {
        m_NameInputField = transform.FindChild("Panel/Name/InputField").GetComponent<InputField>();
        m_ConfirmButton = transform.FindChild("Panel/Confirm").GetComponent<Button>();
        m_ConfirmButton.onClick.AddListener(OnConfirmClick);

        m_CancelButton = transform.FindChild("Panel/Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelClick);
    }

    void OnConfirmClick()
    {
        //name字段不能为空;
        if (m_NameInputField.text == "") {
            FrameUtil.PopNoticePanel("Name Cannot Be Empty !");
            return;
        }
        //查看Name是否重复;
        if (ControlPlayer.Instance.m_NameList.Contains(m_NameInputField.text)) {
            FrameUtil.PopNoticePanel("Name Already Exists !");
            return;
        }
        //添加新的名字进List;
        ControlPlayer.Instance.m_NameList.Add(m_NameInputField.text);
        //重新刷新AddItemGroupSecondPanel面板NameList列表;
        GameObject.Find("AddItemGroupSecondPanel(Clone)").GetComponent<AddItemGroupSecondPanel>().UpdateNameDropdownView();
        Destroy(gameObject);
    }

    void OnCancelClick()
    {
        Destroy(gameObject);
    }

    private InputField m_NameInputField;

    private Button m_ConfirmButton;
    private Button m_CancelButton;

    private MsgJson.Item m_Item;
}
