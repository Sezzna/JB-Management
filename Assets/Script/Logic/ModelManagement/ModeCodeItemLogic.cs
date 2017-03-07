using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModeCodeItemLogic : MonoBehaviour {

    void Awake()
    {
        m_Arrow = transform.FindChild("Arrow");

        m_ModelCodeButton = transform.FindChild("ModelCode").GetComponent<Button>();
        m_ModelCodeButton.onClick.AddListener(OnClick);

        m_YearButton = transform.FindChild("Year").GetComponent<Button>();
        m_YearButton.onClick.AddListener(OnClick);

        m_StatusButton = transform.FindChild("Status").GetComponent<Button>();
        m_StatusButton.onClick.AddListener(OnClick);
    
        m_ModelCodeText = transform.FindChild("ModelCode/Text").GetComponent<Text>();
        m_YearText = transform.FindChild("Year/Text").GetComponent<Text>(); ;
        m_StatusText = transform.FindChild("Status/Text").GetComponent<Text>(); ;

        
    }

    void Start()
    {

    }

    public void Init(MsgJson.CarModel model)
    {
        m_CarModel = model;

        m_ModelCodeText.text = model.code;
        m_YearText.text = model.model_year;
        m_StatusText.text = model.status;
    }

    void OnClick()
    {
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
        form.AddField("id", m_CarModel.id);

        HttpManager.Instance.SendPostForm(ProjectConst.GetModelDetil, form);
    }

    //---------------------------MEMBER------------------------------------

    private Button m_ModelCodeButton;
    private Button m_YearButton;
    private Button m_StatusButton;

    private Text m_ModelCodeText;
    private Text m_YearText;
    private Text m_StatusText;


    private Transform m_Arrow;

    //ModelsRange数据;
    private MsgJson.CarModel m_CarModel;

}

