using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;


public class UIMainSwitch : MonoBehaviour,IPointerClickHandler
{
    private RectTransform mainRTF;
    private GameObject maskBG;
    void Start ()
    {
        mainRTF = GameObject.FindWithTag("Menu").GetComponent<RectTransform>();
        maskBG = GameObject.FindWithTag("Mask");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        maskBG.SetActive(false);
        Vector3 pos = new Vector3(768, 0, 0);
        if(mainRTF.localPosition.x == 0)
           mainRTF.DOLocalMove(pos, 0.5f);
    }   
}
