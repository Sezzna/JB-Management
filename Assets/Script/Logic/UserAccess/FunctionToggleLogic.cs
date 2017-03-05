using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FunctionToggleLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_Toggle = transform.GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChange);

        m_Text = transform.FindChild("Label").GetComponent<Text>();

        m_Text.text = m_FunctionName;
    }

    void OnValueChange(bool isOn) {
        if (isOn) {
     
        }
        else {
            
        }
    }

    public void Init(MsgJson.Funciont functionInfo) {
        m_ID = functionInfo.id;
        m_FunctionName = functionInfo.name;
    }



    //------------------------------------------MEMBER--------------------------------------
    private string m_ID;
    private string m_FunctionName;

    private Toggle m_Toggle;
    private Text m_Text;
}
