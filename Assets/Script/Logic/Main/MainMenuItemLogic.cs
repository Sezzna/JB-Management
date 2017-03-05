using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuItemLogic : MonoBehaviour {

    void Awake() {
        m_Title = transform.FindChild("Text");
    }

	void Start () {
        m_Button.Set(transform.gameObject).OnClick = OnClick;
        
    }

    public void Init(string title, ButtonExtension.VoidDelegate onClick) {
        m_Title.GetComponent<Text>().text = title;
        OnClick = onClick;
    }

    //---------------------------MEMBER------------------------------------

    private ButtonExtension m_Button = new ButtonExtension();
    
    private ButtonExtension.VoidDelegate OnClick;

    private Transform m_Title;
}
