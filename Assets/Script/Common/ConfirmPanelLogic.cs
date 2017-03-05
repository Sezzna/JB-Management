using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmPanelLogic : MonoBehaviour {

    void Awake() {
        m_ConfirmButton = transform.FindChild("Confirm").GetComponent<Button>();


        m_CancelButton = transform.FindChild("Cancel").GetComponent<Button>();
        m_CancelButton.onClick.AddListener(OnCancelButtonClick);
    }

	void Start () {
        
    }

    public void Init(UnityAction callback) {
        m_ConfirmButton.onClick.AddListener(callback);
    }

    void OnCancelButtonClick()
    {
        Destroy(gameObject);
    }

    private Button m_ConfirmButton;
    private Button m_CancelButton;
}
