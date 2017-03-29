using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftSizeItemLogic : MonoBehaviour {
    void Awake()
    {
        m_Text = transform.FindChild("Text").GetComponent<Text>();
        m_Button = transform.GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);

        m_ButtonImage = transform.GetComponent<Image>();

        m_Arrow = transform.FindChild("Arrow");

        ////改变颜色;
        //ColorBlock cb = new ColorBlock();
        //cb.normalColor = new Color32(245, 245, 245, 255);
        //cb.highlightedColor = new Color32(63, 77, 99, 255);
        //cb.pressedColor = new Color32(245, 245, 245, 255);
        //cb.disabledColor = new Color32(245, 245, 245, 255);

        //cb.colorMultiplier = 1;

        //m_Button.colors = cb;
    }

    public void OnClick()
    {
        //将其他size颜色还原;
        foreach (Transform child in transform.parent) {
            child.GetComponent<LeftSizeItemLogic>().m_ButtonImage.color = new Color32(245, 245, 245, 255);
            child.GetComponent<LeftSizeItemLogic>().m_Text.color = new Color32(50, 50, 50, 255);
        }

        m_ButtonImage.color = new Color32(63, 77, 99, 255);
        m_Text.color = new Color32(245, 245, 245, 255);
        foreach (Transform child in transform.parent)
        {
            child.FindChild("Arrow").gameObject.SetActive(false);
        }

        if (!m_Arrow.gameObject.activeSelf)
        {
            m_Arrow.gameObject.SetActive(true);
        }

        ControlPlayer.Instance.m_CurrentChoiceSizeId = m_id;

        //添加到左测面板.调用 SpecialPartsSelectionPanel;
        GameObject.Find("SpecialPartsSelectionPanel(Clone)").GetComponent<SpecialpartsSelectionPanel>().AddPartItem();

        //WWWForm form = new WWWForm();
        //form.AddField("token", PlayerPrefs.GetString("token"));
        //form.AddField("id", m_id);

        //HttpManager.Instance.SendPostForm(ProjectConst.GetItem, form);

        //ControlPlayer.Instance.m_CurrentSupplierID = m_id;
    }

    public void Init(MsgJson.Size size)
    {
        m_id = size.id;
        m_SizeText = size.size;
        m_Text.text = m_SizeText;
    }

    public Text m_Text;

    private Button m_Button;
    public Image m_ButtonImage;

    private string m_id;
    private string m_SizeText;
    private Transform m_Arrow;
}
