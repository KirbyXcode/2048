using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class UIExit : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
