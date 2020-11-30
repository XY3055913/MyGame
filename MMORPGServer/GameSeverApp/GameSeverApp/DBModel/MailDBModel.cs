using GameServerApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSeverApp.DBModel
{
    /// <summary>
    /// 邮件管理
    /// </summary>
    class MailDBModel:Singleton<MailDBModel>
    {   //初始化,监听消息
        public void Init()
        {
            EventDispatcher.Instance.AddEvenListener(ProtoCodeDef.Mail_Request_List, OnRequestList);
        }

        private void OnRequestList(Role role, byte[] buffer)
        {
            Console.WriteLine("客户端请求邮件列表");

            Mail_Get_ListProto proto = new Mail_Get_ListProto();
            proto.Count = 30;
            proto.MailID = 102;
            proto.MailName = "名称";
            role.client_Socket.SendMsg(proto.ToArray());

        }
    }
}
