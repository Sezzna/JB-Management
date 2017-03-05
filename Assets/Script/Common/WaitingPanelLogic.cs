using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WaitingPanelLogic : MonoBehaviour {

	void Start () {
        m_WaitingImage = transform.FindChild("Image");
        DoLocalRotate();
	}
	
    void DoLocalRotate() {
        m_WaitingImage.DOLocalRotate(new Vector3(0, 0, -360), 1f , RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    //------------------------------------------MEMBER-----------------------------------
    private Transform m_WaitingImage;
}
