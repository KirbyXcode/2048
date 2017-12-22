using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;


public class UIRestart : MonoBehaviour, IPointerClickHandler
{
    private RectTransform gameOverRTF;
    void Start()
    {
        gameOverRTF = GameObject.FindWithTag("GameOver").GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Tweener tweener = gameOverRTF.DOLocalMove(new Vector3(0, 1024, 0),0.8f);
        tweener.SetEase(Ease.OutBounce);
    }
}
