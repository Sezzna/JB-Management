using UnityEngine;
using System.Collections;

public class UIManager : Singletion<UIManager> {


	void Start () {
        
    }

    //显示菊花图;
    public void ShowWaitingPanel() {

    }

    //关闭菊花图;
    public void CloseWaitingPanel() {
        
    }



    //------------------------------------------------MEMBER-----------------------------------

    private Transform m_Stack;

    private GameObject m_WaitingPanelGO;

    private Object m_WaitingPanelInstance;
}
