using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class UISlider : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [Tooltip("拖拽速度")]
    public float dragSpeed = 100;

    [Tooltip("拖拽速度上限")]
    public float maxDragSpeed = 5;

    [Tooltip("拖拽距离上限")]
    public float maxDragDistance = 300;

    private Vector2 beginDragPoint; //开始拖拽位置
    public int currentPageIndex;
    private Transform[] pagesTF;

    private Vector3 startMovePos; //开始移动位置
    private Vector3 endMovePos; //结束移动位置

    public AnimationCurve curve;
    private float x = 1;//游戏开始时不做移动
    public float duration = 0.2f;

    private void Start()
    {
        //查找所有子物体（页面）
        pagesTF = new Transform[transform.childCount];
        for (int i = 0; i < pagesTF.Length; i++)
        {
            pagesTF[i] = transform.GetChild(i);
        }
    }

    //根据拖拽方向移动面吧
    public void OnDrag(PointerEventData eventData)
    {   
        //光标本帧位置-上帧位置
        float x = eventData.delta.x * dragSpeed * Time.deltaTime;
        transform.Translate(x, 0, 0);
    }

    //结束拖拽是，根据拖拽距离归位、拖拽速度归位
    //归本页、上页、下页(通过索引控制)
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragOffset = eventData.position - beginDragPoint;
        //如果拖拽速度够快，或者距离够远
        if (Mathf.Abs(eventData.delta.x) > maxDragSpeed || Mathf.Abs(dragOffset.x) > maxDragDistance) 
        {
            //切换页面
            if (dragOffset.x > 0)
                currentPageIndex--;
            else
                currentPageIndex++;
            //if (currentPageIndex < 0)
            //    currentPageIndex = 0;
            //if (currentPageIndex > pagesTF.Length - 1) 
            //    currentPageIndex = pagesTF.Length - 1;
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pagesTF.Length - 1);
            x = 0;
        }
        //移动:呈现pagesTF[currentPageIndex]页面
        //公式:父物体位置(画布位置) - 需要呈现页面位置 + 滑动面板位置
        endMovePos = transform.parent.position - pagesTF[currentPageIndex].position + transform.position;
        startMovePos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //开始拖拽时记录点击位置
        beginDragPoint = eventData.position;
    }

    private void Movement()
    {
        if(x < 1)
        {
            x += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startMovePos, endMovePos, curve.Evaluate(x));
        }
    }

    private void Update()
    {
        Movement();
    }
}
