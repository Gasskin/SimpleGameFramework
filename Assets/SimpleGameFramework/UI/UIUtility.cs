using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// UI工具类
/// </summary>
public static class UIUtility
{
    /// <summary>
    /// 根据路径获取某个child的某个组件 
    /// </summary>
    private static T Find<T>(Transform trans, string path)
    {
        var child = trans.Find(path);
        return child.GetComponent<T>();
    }


    /// <summary>
    /// 根据路径给按钮添加一个OnClick方法
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="action">方法</param>
    public static void RegisterButton(this Transform trans, string path, UnityAction action)
    {
        var btn = Find<Button>(trans, path);
        btn.onClick.AddListener(action);
    }

    /// <summary>
    /// 根据路径获取一个Text组件
    /// </summary>
    /// <param name="path">路径</param>
    public static Text FindText(this Transform trans, string path)
    {
        return Find<Text>(trans, path);
    }
}
