using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewNesting : MonoBehaviour {

    private void Awake()
    {
        m_PartList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/PartList");
        m_FramScrollView = transform.FindChild("FrameScrollView"); ;

        EventListener.Get(m_PartList.gameObject).onScroll += OnDrag;
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnDrag(GameObject go, PointerEventData eventData)
    {
        //m_FramScrollView.GetComponent<ScrollRect>().OnDrag(eventData);
        m_FramScrollView.GetComponent<ScrollRect>().OnScroll(eventData);
        Debug.Log("来没有????");
    }


    private Transform m_PartList;
    private Transform m_FramScrollView;
}
