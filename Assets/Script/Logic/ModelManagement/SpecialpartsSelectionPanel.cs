using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpecialpartsSelectionPanel : MonoBehaviour {
    private void Awake() {
        m_PartList = transform.FindChild("FrameScrollView/Viewport/Content/GameObject/PartList");
 

    }




    private Transform m_PartList;
}
