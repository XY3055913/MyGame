
/// <summary>
/// 抽象数据实体的基类
/// </summary>
public abstract class AbstractEntity // abstrac 不能被实例化，只能被继承
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id
    {
        get; set;
    }
}
