using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddNewUserPanelLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_ConfirmButton = transform.FindChild("Confirm").GetComponent<Button>();
        m_ConfirmButton.onClick.AddListener(OnConfirmButtonClick);
        m_CancelButton = transform.FindChild("Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelButtonClick);

        m_UserNameInputField = transform.FindChild("UserName").GetComponent<InputField>();
        m_UserNameInputField.onEndEdit.AddListener(OnUserNameEndEdit);

        m_PasswordInputField = transform.FindChild("Password").GetComponent<InputField>();
        m_PasswordInputField.onEndEdit.AddListener(OnPasswordEndEdit);

        m_FirstNameInputField = transform.FindChild("FirstName").GetComponent<InputField>();
        m_FirstNameInputField.onEndEdit.AddListener(OnFirstNameEndEdit);

        m_LastNameInputField = transform.FindChild("LastName").GetComponent<InputField>();
        m_LastNameInputField.onEndEdit.AddListener(OnLastNameEndEdit);

        m_EmailInputField = transform.FindChild("Email").GetComponent<InputField>();
        m_LastNameInputField.onEndEdit.AddListener(OnEmailEndEdit);

        m_PhoneNameInputField = transform.FindChild("Phone").GetComponent<InputField>();
        m_LastNameInputField.onEndEdit.AddListener(OnPhoneEndEdit);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnConfirmButtonClick() {
        if (m_UserName != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("token", PlayerPrefs.GetString("token"));
            form.AddField("username", m_UserName);
            form.AddField("password", m_Password);
            form.AddField("firstname", m_FirstName);
            form.AddField("lastname", m_LastName);
            form.AddField("email", m_Email);
            form.AddField("phone", m_Phone);

            HttpManager.Instance.SendPostForm(ProjectConst.AddNewUser, form);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCancelButtonClick() {
        Destroy(gameObject);
    }

    void OnUserNameEndEdit(string s) {
        m_UserName = s;
    }

    void OnPasswordEndEdit(string s)
    {
        m_Password = s;
    }

    void OnFirstNameEndEdit(string s)
    {
        m_FirstName = s;
    }

    void OnLastNameEndEdit(string s)
    {
        m_LastName = s;
    }

    void OnEmailEndEdit(string s)
    {
        m_Email = s;
    }

    void OnPhoneEndEdit(string s)
    {
        m_Phone = s;
    }

    private InputField m_UserNameInputField;
    private InputField m_PasswordInputField;
    private InputField m_FirstNameInputField;
    private InputField m_LastNameInputField;
    private InputField m_EmailInputField;
    private InputField m_PhoneNameInputField;


    private Button m_ConfirmButton;
    private Button m_CancelButton;

    private string m_UserName = "";
    private string m_Password = "";
    private string m_FirstName = "";
    private string m_LastName = "";
    private string m_Email = "";
    private string m_Phone = "";
}
