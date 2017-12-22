using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Console2048;
using UnityEngine.EventSystems;
using MoveDirection = Console2048.MoveDirection;

/// <summary>
///游戏控制器
/// </summary>
public class GameController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private GameCore core;
    public NumberSprite[,] spriteActions;
    private void Start()
    {
        core = new GameCore();
        spriteActions = new NumberSprite[4, 4];

        Init();

        GenerateNewNumber();
        GenerateNewNumber();
    }

    public void Init()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                spriteActions[r, c] = CreateSprite(r, c);
            }
        }
    }

    private NumberSprite CreateSprite(int r, int c)
    {
        //方式一：new GameObject 适合简单的UI
        // 缩放1   位置0
        //1.创建空物体
        GameObject spriteGO = new GameObject(r.ToString() + c.ToString());
        //2.添加UI组件   
        spriteGO.AddComponent<Image>();
        var action = spriteGO.AddComponent<NumberSprite>();
        action.SetSprite(0);
        //3.设置父级  缩放1   位置0
        spriteGO.transform.SetParent(transform, false);
        return action;
        //方式二：将UI制成预制件，在克隆，适合复杂的UI。 
    }

    //生成新数字
    public void GenerateNewNumber()
    {
        //通过核心算法类，生成新数字
        Location loc;
        int number;
        core.GenerateNumber(out loc, out number);
        //频繁查找某个位置的脚本对象，不如先将所有脚本对象引用存储在二维数组中
        //transform.Find("13").GetComponent<NumberSprite>().SetSprite(number);
        //通过二维数组获取方格的行为脚本
        spriteActions[loc.RIndex, loc.CIndex].SetSprite(number);
        spriteActions[loc.RIndex, loc.CIndex].GenerateEffect();
    }

    private void Update()
    {
        if (core.IsChange)
        {
            GenerateNewNumber();
            UpdateMap();
            if (core.IsOver())
            {
                //游戏结束
                GameOver();
            }
            core.IsChange = false; //表示地图变化完毕
        }
        KeyboardDetector();
    }

    private void UpdateMap()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                spriteActions[r, c].SetSprite(core.Map[r, c]);
            }
        }
    }

    //检测用户输入 
    //光标按下时执行，记录光标位置
    private Vector2 downPoint;
    private bool toggle =false;
    public void OnPointerDown(PointerEventData eventData)
    {
        downPoint = eventData.position;
        toggle = true;
    }
    //拖拽时执行，判断拖拽方向(上、下、左、右)
    public void OnDrag(PointerEventData eventData)
    {
        //如果开关关闭状态  退出方法
        if (!toggle) return;

        //触摸偏移量
        Vector2 touchOffset = eventData.position - downPoint;
        float absX = Mathf.Abs(touchOffset.x);
        float absY = Mathf.Abs(touchOffset.y); 
        MoveDirection? dir = null;
        //int? a = null; //值类型? 变量名 = null;  可空值类型
        //int b = a.Value;//获取可空值类型中的数据
        //水平移动
        if (absX > absY && absX > 100)
        {
            dir = touchOffset.x > 0 ?MoveDirection.Right : MoveDirection.Left;
        }
        //垂直移动
        if (absY > absX && absY > 100)
        {
            dir = touchOffset.y > 0 ? MoveDirection.Up : MoveDirection.Down;
        }
        if (dir != null)
        {
            core.Move(dir.Value);
            toggle = false;
        }
    }

    public void GameOver()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                spriteActions[r, c].SetSprite(0);
            }
        }
        core.emptyLOCList.Clear();
        spriteActions = null;
        spriteActions = new NumberSprite[4, 4];
        GenerateNewNumber();
        GenerateNewNumber();
    }
    
    private void KeyboardDetector()
    {
        if (Input.GetKeyDown(KeyCode.A))
            core.Move(MoveDirection.Left);
        if (Input.GetKeyDown(KeyCode.D))
            core.Move(MoveDirection.Right);
        if (Input.GetKeyDown(KeyCode.W))
            core.Move(MoveDirection.Up);
        if (Input.GetKeyDown(KeyCode.S))
            core.Move(MoveDirection.Down);
    }
}
