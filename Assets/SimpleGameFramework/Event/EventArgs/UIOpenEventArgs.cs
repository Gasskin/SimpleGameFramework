using SimpleGameFramework.Event;


public struct UIEventData
{
    public string test1;
    public int test2;
}

public class UIOpenEventArgs : GlobalEventArgs
{
    /// UI的名字，所有的UI都会注册这个时间，则需要靠名称来确定当前到底打开了哪一个UI
    public string uiName;
    
    public string data;

    /// 这个ID是事件的ID，会根据事件ID找到相应的处理方法 
    public override int Id
    {
        get
        {
            return SGFEvents.OpenUI.GetHashCode();
        }
        set { }
    }

    public override void Clear()
    {
        Id = UIRegister.None.GetHashCode();
        uiName = string.Empty;
        data = string.Empty;
    }
}
