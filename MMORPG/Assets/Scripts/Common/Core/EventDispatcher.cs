using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 观察者模式
/// </summary>
public class EventDispatcher : Singleton<EventDispatcher>
{
    //委托原型
    public delegate void OnActionHandler(byte[] buffer);

    //委托字典
    private Dictionary<ushort, List<OnActionHandler>> dic = new Dictionary<ushort, List<OnActionHandler>>();

    /// <summary>
    /// 添加进字典
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="actionHandler"></param>
    public void AddEvenListener(ushort protoCode,OnActionHandler handler) 
    {
        if (dic.ContainsKey(protoCode))
        {
            //往ID里添加委托
            dic[protoCode].Add(handler);

        }
        else
        {
            List<OnActionHandler> lisHandle = new List<OnActionHandler>();
            lisHandle.Add(handler);
            dic[protoCode] = lisHandle;

        }
    
    
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void RemoveEventListener(ushort protoCode, OnActionHandler handler) 
    {
        if (dic.ContainsKey(protoCode))
        {
            List<OnActionHandler> lstHandle = dic[protoCode];
            lstHandle.Remove(handler);
           
            if (lstHandle.Count==0)
            {
                //移出字典
                dic.Remove(protoCode);
            }



        }
    
    
    
    }

    /// <summary>
    /// 派发协议
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="param"></param>
    public void Dispath(ushort protoCode, byte[] buffer) 
    {
        if (dic.ContainsKey(protoCode))
        {
            List<OnActionHandler> lstHandle = dic[protoCode];//取出集合
            if (lstHandle!=null&&lstHandle.Count>0)
            {
                for (int i = 0; i < lstHandle.Count; i++)
                {
                    if (lstHandle[i]!=null)//集合不为空
                    {
                        lstHandle[i](buffer);//执行里面的委托
                    }
                }
            }
        
        
        
        }
    
    }

}
