using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  资源管理类,封装所有读取操作 
/// </summary>
public class ResourceManager
{
    public static Dictionary<int, Sprite> dic;
    //类被加载时执行1次
    static ResourceManager()
    { 
        dic = new Dictionary<int, Sprite>();
        //通过Unity API 读取图集
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/2048Atlas");
        //向字典集合添加记录
        foreach (var item in spriteArray)
        {
            int intName = int.Parse(item.name);
            //key 为int类型，存储精灵表示的数字
            //value 为Sprite类型，存储精灵
            dic.Add(intName, item);
        }
    }
     
    //0   2   4   8     2048
    public static Sprite GetImage(int number)
    { 
        //从字典集合中返回指定精灵
        return dic[number]; 
        ////读取图集
        //Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/2048Atlas");
        ////遍历精灵
        //foreach (var item in spriteArray)
        //{//如果该精灵名称(表示的数字) 与参数相同
        //    if (item.name == number.ToString())
        //    {
        //        return item;
        //    } 
        //}
        //return null;
    } 
}
