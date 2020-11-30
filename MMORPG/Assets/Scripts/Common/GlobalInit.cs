using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInit : MonoBehaviour
{
    #region 单例
    private static GlobalInit instance;
    public static GlobalInit Instance
    {
        get
        {
            if (instance == null)
            {
                //GameObject obj = new GameObject("NetWorkSocket");

                //DontDestroyOnLoad(obj);
                //instance = obj.GetOrCreatComponent<NetWorkSocket>();
                instance = new GlobalInit();
            }
            return instance;
        }
    }
    #endregion

    //临时委托
    public delegate void OnReceiveProtoHandler(ushort protoCode,byte[] buffer);
    //定义委托
    public OnReceiveProtoHandler OnReceiveProto;

    private void Awake()
    {
        instance = this;
    }


    

}
