using UnityEngine;
using LitJson;
using System;

public class TestMemory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 数据表实体查询与Hppt连接测试
        /*
        //查整表
        List<JobEntity> lst = JobDBModel.Instance.GetList();
        for (int i = 0; i < lst.Count; i++)
        {
            Debug.Log("职业-" + l st[i].Name + ",描述:" + lst[i].Desc);
        }

        ProductEntity entity = ProductDBModel.Instance.GetId(6);
        if (entity != null)
        {
            Debug.Log("name=" + entity.Name);
        }
        */


        /*    
        if (!NetWorkHttp.Instance.IsBusy)//不忙的时候
        {
            NetWorkHttp.Instance.SendData("http://47.115.200.97:8080/api/account?id=2", GetCallBack);
        }
        */

        /*
        if (!NetWorkHttp.Instance.IsBusy)
        {
            //注册
            JsonData jsonData = new JsonData();
            jsonData["Type"] = 0;//0=注册 1=登录
            jsonData["UserName"] = "xx";
            jsonData["PassWord"] = "123";
            jsonData["Logintype"] = "Phone";
            NetWorkHttp.Instance.SendData("http://47.115.200.97:8080/api/account", PostCallBack, true, jsonData.ToJson());
        }
        */

        #endregion
        //1.连接服务器
         NetWorkSocket.Instance.Connect("192.168.50.253", 1011);


        //启动监听
        //GlobalInit.Instance.OnReceiveProto = OnReceiveProtoCallBack;





    }
    /// <summary>
    /// 委托回调
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="buffer"></param>
    private void OnReceiveProtoCallBack(ushort protoCode, byte[] buffer)
    {
        Debug.Log("protoCode="+protoCode);



        TestProto tmpprot = TestProto.GetProto(buffer);
        Debug.Log("tmpprotID="+tmpprot.Id);
        Debug.Log("tmpprotName=" + tmpprot.Name);
    }

    /*
private void GetCallBack(NetWorkHttp.CallBackArgs obj)
{

   if (obj.HasError)//如果返回为空
   {
       Debug.Log(obj.ErrorMsg);
   }
   else
   {
       //AccountEntity entity = LitJson.JsonMapper.ToObject<AccountEntity>(obj.Json);
       Debug.Log(obj.Json);
   }


}

private void PostCallBack(NetWorkHttp.CallBackArgs obj)
{

   if (obj.HasError)//如果返回为空
   {
       Debug.Log(obj.ErrorMsg);
   }
   else
   {
       Debug.Log(obj.Json);
   }


}
*/

    private void Send(string msg) 
    {
        //2.发消息
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUTF8String(msg);
            NetWorkSocket.Instance.SendMsg(ms.ToArray());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestProto proto = new TestProto();
            proto.Id = 100;
            proto.Name = "测试协议";
            proto.Type = 80;
            proto.Price = 99.5f;

            NetWorkSocket.Instance.SendMsg(proto.ToArray());



        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Send("你好，S");
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    for (int i = 0; i < 30; i++)
        //    {
        //        Send("你好循环...." + i);
        //    }
        //}
    }






}
