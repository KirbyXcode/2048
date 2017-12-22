using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 数字精灵，定义方块的行为。
/// </summary>
public class NumberSprite : MonoBehaviour
{
    private Image img;
    //创建脚本对象立即执行Awake
    //创建脚本对象，脚本启用，才(至少延迟1帧)执行Start
    private void Awake()
    {
        img = GetComponent<Image>(); 
    }
    /// <summary>
    /// 设置当前方格中的精灵
    /// </summary>
    /// <param name="number">精灵表示的数字</param>
    public void SetSprite(int number)
    { 
        img.sprite = ResourceManager.GetImage(number);
    }

    //移动

    //生成效果
    public void GenerateEffect()
    {
        //从小  -->  大
        //transform.localScale = Vector3.zero;
        //iTween.ScaleTo(gameObject, Vector3.one, 0.3f);

        iTween.ScaleFrom(gameObject, Vector3.zero, 0.3f);
    }

    //合并效果
    public void MergeEffect()
    {
        iTween.ScaleFrom(gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.4f);
    }
}
