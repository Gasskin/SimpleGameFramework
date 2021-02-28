namespace SimpleGameFramework.Core
{
    /// 用于记录管理器的优先级，每一个管理器的Proprity属性需要返回对应enum的HashCode
    public enum ManagerPriority
    {
        EventManager,
        ReferenceManager,
    }
}