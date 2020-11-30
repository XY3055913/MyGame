using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽象数据管理基类
/// </summary>
/// <typeparam name="T">子类的类型</typeparam>
/// <typeparam name="P">操作数据类型的类</typeparam>
public abstract class AbstractDBModel<T,P> 
    where T : class,new() 
    where P : AbstractEntity
{
  
    protected List<P> m_List;//总账本
    protected Dictionary<int, P> m_Dic;//对应ID的账本

    public AbstractDBModel() 
    {
        m_List = new List<P>();
        m_Dic = new Dictionary<int, P>();
        LoadData();
    }


    #region 单例
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
               
            }
            return instance;
        }

    }

    #endregion


    #region 需要子类实现的属性或方法
    /// <summary>
    /// 数据文件名称
    /// </summary>
    protected abstract string FileName { get; }//跟子类要表的名字（或路径）

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected abstract P MakeEntity ( GameDataTableParser parse);// 子类的数据类型，自由子类知道，基类不知道的

    #endregion


    #region 加载数据
    /// <summary>
    /// 加载数据
    /// </summary>
    private void LoadData()
    {
        //1.读取文件  路径问题/先写死
        using (GameDataTableParser parse = new GameDataTableParser(string.Format(@"E:\TestU3DProject\MMORPG\www\Data\{0}", FileName)))
        {
            while (!parse.Eof)
            {
                //创建实体
                P p = MakeEntity(parse);
                m_List.Add(p);
                m_Dic[p.Id] = p;
                parse.Next();//没循环之后，转下一个
            }
        }
    }
    #endregion

    #region 获取集合
    /// <summary>
    /// 获取集合
    /// </summary>
    /// <returns></returns>
    public List<P> GetList()
    {
        return m_List;
    }
    #endregion

    #region Get 根据编号获取实体
    /// <summary>
    /// 根据编号获取实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public P GetId(int id)
    {
        if (m_Dic.ContainsKey(id))
        {
            return m_Dic[id];
        }
        return null;
    }
    #endregion

}
