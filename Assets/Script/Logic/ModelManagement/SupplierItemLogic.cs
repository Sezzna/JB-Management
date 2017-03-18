using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplierItemLogic : MonoBehaviour {

    void Awake()
    {
        m_Text = transform.FindChild("Text").GetComponent<Text>();
        m_Button = transform.GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
        m_Arrow = transform.FindChild("Arrow");
    }

    void OnClick() {
        foreach (Transform child in transform.parent)
        {
            child.FindChild("Arrow").gameObject.SetActive(false);
        }

        if (!m_Arrow.gameObject.activeSelf)
        {
            m_Arrow.gameObject.SetActive(true);
        }

        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", m_id);

        HttpManager.Instance.SendPostForm(ProjectConst.GetItem, form);

        ControlPlayer.Instance.m_CurrentSupplierID = m_id;
    }

    public void Init(MsgJson.Supplier supplier){
        m_id = supplier.id;
        m_SupplierName = supplier.Name;
        m_Text.text = m_SupplierName;
    }

    private Text m_Text;

    private Button m_Button;

    private string m_id;
    private string m_SupplierName;
    private Transform m_Arrow;
}
