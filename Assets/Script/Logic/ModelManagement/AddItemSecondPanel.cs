using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemSecondPanel : MonoBehaviour {
    void Awake()
    {
        m_ViewToggle = transform.FindChild("Panel/View").GetComponent<Toggle>();
        m_ViewToggle.onValueChanged.AddListener(OnValueChanged);
        m_DisplayToggle = transform.FindChild("Panel/Display").GetComponent<Toggle>();

        m_Cancel = transform.FindChild("Panel/Cancel").GetComponent<Button>();
        m_Cancel.onClick.AddListener(OnCancelClick);
        m_Confirm = transform.FindChild("Panel/Confirm").GetComponent<Button>();
        m_Confirm.onClick.AddListener(OnConfirmClick);


        m_CategoryDropdown = transform.FindChild("Panel/Category/Dropdown").GetComponent<Dropdown>();

        m_StagesGrounp = transform.FindChild("Panel/StageGroup");

        m_StageToggle = Resources.Load<GameObject>("StageToggle");

        m_Description = transform.FindChild("Panel/Title").GetComponent<Text>();

        MsgRegister.Instance.Register((short)MsgCode.S2C_GetItemDisplayStage, OnGetItemDisplayStage);
    }

	// Use this for initialization
	void Start () {
        UpdateTypeDropdownView();

        foreach (var v in ControlPlayer.Instance.m_ItemStages.stages){
            FrameUtil.AddChild(m_StagesGrounp.gameObject, m_StageToggle).GetComponent<ItemStageLogic>().Init(v);
        }
    }

    public void Init(MsgJson.Item item) {
        m_Item = item;
        m_Description.text = item.description;
        //获得ItemCategory信息
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("id", m_Item.id);
        //Debug.Log(m_Item.id);
        HttpManager.Instance.SendPostForm(ProjectConst.GetItemDisplayStage, form);
    }

    public void OnGetItemDisplayStage(string data) {
        MsgJson.ItemStageDisplay itemStageDisplay = JsonUtility.FromJson<MsgJson.ItemStageDisplay>(data);

        foreach (var v in itemStageDisplay.stages) {
            foreach (Transform child in m_StagesGrounp)
            {
                if (child.GetComponent<ItemStageLogic>().m_Id == v.item_stage_id) {
                    child.GetComponent<Toggle>().isOn = true;
                }
             }
        }

        
    }

    //------------------------------------------------------ON BUTTON ----------------------------------------------
    void OnValueChanged(bool val) {
        if (val == true)
        {
            m_DisplayToggle.gameObject.SetActive(true);
        }
        else {
            m_DisplayToggle.gameObject.SetActive(false);
        }
    }

    void OnCancelClick() {
        Destroy(gameObject);
    }

    private class stages {
        
    }

    void OnConfirmClick() {
        //发送 ItemCategoryStageUpdate
        WWWForm form = new WWWForm();
        form.AddField("token", PlayerPrefs.GetString("token"));
        form.AddField("item_id", m_Item.id);
        form.AddField("category_id", m_CategoryMap[m_CategoryDropdown.captionText.text]);

        Debug.Log(m_CategoryMap[m_CategoryDropdown.captionText.text]);
   
        List<MsgJson.UpdateItemStageId> stageId = new List<MsgJson.UpdateItemStageId>();


        foreach (Transform child in m_StagesGrounp) {
            if (child.GetComponent<Toggle>().isOn) {
                MsgJson.UpdateItemStageId id = new MsgJson.UpdateItemStageId();
                id.id = child.GetComponent<ItemStageLogic>().m_Id;
                stageId.Add(id);
            }
        }

        MsgJson.UpdateItemStage updateItemStage = new MsgJson.UpdateItemStage();
        updateItemStage.stages = stageId.ToArray();

        form.AddField("msg", JsonUtility.ToJson(updateItemStage));
      
        HttpManager.Instance.SendPostForm(ProjectConst.ItemCategoryStageUpdate, form);

        //刷新这个供货商下的所有item;
        WWWForm form1 = new WWWForm();
        form1.AddField("token", PlayerPrefs.GetString("token"));
        form1.AddField("id", ControlPlayer.Instance.m_CurrentSupplierID);

        HttpManager.Instance.SendPostForm(ProjectConst.GetItem, form1);

        //再保留数据;

        //最后删除面板;
        Destroy(gameObject);
    }


    private void UpdateTypeDropdownView()
    {
        AddDropdownItemName();
        m_CategoryDropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < m_CategoryList.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = m_CategoryList[i];
            m_CategoryDropdown.options.Add(tempData);
        }

        if (m_Item.category_id != "0")
        {
            //Debug.Log("------------------- " + m_Item.category_id);
            m_CategoryDropdown.captionText.text = m_CategoryIdKeyMap[m_Item.category_id];
           
        }
        else {
            m_CategoryDropdown.captionText.text = "Please Select";
        }
    }

    //设置DorpDown字段名字;
    private void AddDropdownItemName()
    {
        foreach (var v in ControlPlayer.Instance.m_ItemCategory.category) {
            string temp = v.rank + " " + v.des;
            m_CategoryList.Add(temp);
            m_CategoryMap[temp] = v.id;
            m_CategoryIdKeyMap[v.id] = temp;
      
        }
    }

    //---------------------------------- MEMBER ---------------------------------
    private Text m_Description;

    private Toggle m_ViewToggle;
    private Toggle m_DisplayToggle;

    private Dropdown m_CategoryDropdown;
    private List<string> m_CategoryList = new List<string>();

    private Button m_Cancel;
    private Button m_Confirm;

    private Transform m_StagesGrounp;
    private GameObject m_StageToggle;

    //Rank + 描述作为key, id 作为value 这样下拉框选择后才知道选的什么categroy id;
    private Dictionary<string, string> m_CategoryMap = new Dictionary<string, string>();

    //id 作为key , Rank + 描述作为value,  这样才能通过id 选取默认值;
    private Dictionary<string, string> m_CategoryIdKeyMap = new Dictionary<string, string>();

    private MsgJson.Item m_Item;
}
