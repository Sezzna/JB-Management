using UnityEngine;
using UnityEngine.UI;

public class GroupToggleItemLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_Toggle = transform.GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChange);
        m_Text = transform.FindChild("Label").GetComponent<Text>();

        m_Text.text = m_GroupName;

        m_UserAccessPanel = GameObject.Find("UserAccessPanel(Clone)");
    }

    public void Init(MsgJson.Groups groupInfo) {
        m_GroupID = groupInfo.id;
        m_GroupName = groupInfo.name;
        
    }

    void OnValueChange(bool isOn) {
        //if (isOn)
        //{
        //    if (ControlPlayer.Instance.m_UserHaveGroupDic.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        //    {
        //        ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID].Add(m_GroupID);
        //    }
        //    else {
        //        List<string> tmp = new List<string>();
        //        tmp.Add(m_GroupID);
        //        ControlPlayer.Instance.m_UserHaveGroupDic.Add(ControlPlayer.Instance.m_CurrentEditUserID, tmp);
        //    }
        //    m_UserAccessPanel.GetComponent<UserAccessPanelLogic>().AddGroup(ControlPlayer.Instance.m_CurrentEditUserID, m_GroupID);
        //}
        //else {
        //    if (ControlPlayer.Instance.m_UserHaveGroupDic.ContainsKey(ControlPlayer.Instance.m_CurrentEditUserID))
        //    {
        //        for (int i = 0 ; i < ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID].Count ; ++i) {
        //            if (ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID][i] == m_GroupID) {
        //                ControlPlayer.Instance.m_UserHaveGroupDic[ControlPlayer.Instance.m_CurrentEditUserID].RemoveAt(i);

        //                m_UserAccessPanel.GetComponent<UserAccessPanelLogic>().DeleteGroup(ControlPlayer.Instance.m_CurrentEditUserID, m_GroupID);
        //            }
        //        }
        //    }
        //}

        //m_IsOn = isOn;
    }

    //------------------------------------------------MEMBER--------------------------------------------------------
    private GameObject m_UserAccessPanel;

    private string m_GroupID;
    private string m_GroupName;
    private bool m_IsOn;

    private Toggle m_Toggle;
    private Text m_Text;


}
