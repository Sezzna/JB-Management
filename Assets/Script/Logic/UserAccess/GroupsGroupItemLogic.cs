using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class GroupsGroupItemLogic : MonoBehaviour {

    void Awake() {
        m_Button = GetComponent<Button>();
      

        m_GroupNameText = transform.FindChild("Text").GetComponent<Text>();
        m_GroupPanel = GameObject.Find("GroupsPanel(Clone)");
    }
	
	void Start () {
        m_Button.onClick.AddListener(OnClick);
    }

    //内部Click响应初始化;
    public void Init(MsgJson.Groups group)
    {
        m_GroupID = group.id;
        m_GroupName = group.name;
        m_GroupNameText.text = m_GroupName;

        m_Button.onClick.AddListener(OnClick);
    }

    //新加组的初始化;
    public void Init(MsgJson.AddNewGroup newGroup)
    {
        m_GroupID = newGroup.id;
        m_GroupName = newGroup.name;
        m_GroupNameText.text = m_GroupName;

        m_Button.onClick.AddListener(OnClick);
    }

    //外部Click响应初始化;
    public void Init(string title, UnityAction callBack)
    {

        m_GroupName = title;
        m_GroupNameText.text = m_GroupName;
        m_Button.onClick.AddListener(callBack);
    }

    //--------------------------------------- Button Response ----------------------------

    void OnClick() {
        m_GroupPanel.GetComponent<GroupsPanelLogic>().RefreshDisplay(m_GroupID, m_GroupName);
        m_GroupPanel.GetComponent<GroupsPanelLogic>().m_CurrentChoseGroupID = m_GroupID;
    }

    //---------------------------------------  MEMBER -------------------------------------
    private GameObject m_GroupPanel;

    private Button m_Button;
    private Text m_GroupNameText;

    public string m_GroupID;
    public string m_GroupName;
}
