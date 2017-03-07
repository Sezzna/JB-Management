using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelItemLogic : MonoBehaviour {

    void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Title = transform.FindChild("Text").GetComponent<Text>();
        m_Arrow = transform.FindChild("Arrow");
    }

    void Start()
    {
       

    }

    public void Init(MsgJson.Range range)
    {
        m_Range = range;
        m_Title.text = m_Range.description;
        m_Button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //显示点击的箭头,其他都关闭;
        foreach (Transform child in transform.parent)
        {
            child.FindChild("Arrow").gameObject.SetActive(false);
        }

        if (!m_Arrow.gameObject.activeSelf) {
            m_Arrow.gameObject.SetActive(true);
        }

        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", m_Range.id);

        HttpManager.Instance.SendPostForm(ProjectConst.GetModel, form);
    }

    //---------------------------MEMBER------------------------------------

    private Button m_Button;
    private Text m_Title;
    private Transform m_Arrow;

    //ModelsRange数据;
    private MsgJson.Range m_Range;
    
    
}
