using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoticePanelLogic : MonoBehaviour {

    void Awake() {
        m_Text = transform.FindChild("Text").GetComponent<Text>();

        m_Confirm = transform.FindChild("Confirm").GetComponent<Button>();
        m_Confirm.onClick.AddListener(OnConfirmButtonClick);

        m_Cancel = transform.FindChild("Cancel").GetComponent<Button>();
        m_Cancel.onClick.AddListener(OnCancelButtonClick);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(string s) {
        m_Text.text = s;
    }

    void OnConfirmButtonClick() {
        Destroy(gameObject);
    }

    void OnCancelButtonClick()
    {
        Destroy(gameObject);
    }

    private Text m_Text;
    private Button m_Confirm;
    private Button m_Cancel;


}
