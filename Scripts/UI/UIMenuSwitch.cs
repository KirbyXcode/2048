using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;


public class UIMenuSwitch : MonoBehaviour,IPointerDownHandler
{
    private RectTransform menuRTF;
    private GameObject maskBG;

    private void Start()
    {
        menuRTF = GameObject.FindWithTag("Menu").GetComponent<RectTransform>();
        maskBG = GameObject.FindWithTag("Mask");
        maskBG.SetActive(false);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        menuRTF.DOLocalMove(new Vector3(0, 0, 0), 1);
        StartCoroutine("ShowMask");
    }

    private IEnumerator ShowMask()
    {
        yield return new WaitForSeconds(1);
        maskBG.SetActive(true);
    }
}
