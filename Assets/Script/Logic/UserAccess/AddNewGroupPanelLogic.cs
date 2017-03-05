using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddNewGroupPanelLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_Confirm = transform.FindChild("Confirm").GetComponent<Button>();
        m_Cancel = transform.FindChild("Cancel").GetComponent<Button>();

        m_Confirm.onClick.AddListener(OnConfirmButtonClick);
        m_Cancel.onClick.AddListener(OnCancelButtonClick);

        m_GroupNameInputField = transform.FindChild("GroupNameInputField").GetComponent<InputField>();
        m_GroupNameInputField.onEndEdit.AddListener(OnGroupNameEndEdit);

        
    }

    //---------------------------------------------------Button Response ------------------------------------

    void OnGroupNameEndEdit(string s) {
        m_GroupName = s;
    }

    void OnConfirmButtonClick() {
        if (m_GroupName != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("token", PlayerPrefs.GetString("token"));
            form.AddField("name", m_GroupName);
            HttpManager.Instance.SendPostForm(ProjectConst.AddNewGroup, form);
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnCancelButtonClick()
    {
        Destroy(gameObject);
    }

    //-------------------------------------------------MEMBER--------------------------------------------
    private Button m_Confirm;
    private Button m_Cancel;

    private string m_GroupName = "";

    private InputField m_GroupNameInputField;
}
