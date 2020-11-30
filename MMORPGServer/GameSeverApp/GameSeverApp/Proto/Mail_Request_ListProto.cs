//===================================================
//作    者：肖 源  
//创建时间：2020-11-26 09:39:41
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 请求邮件列表
/// </summary>
public struct Mail_Request_ListProto : IProto
{
    public ushort ProtoCode { get { return 1007; } }


    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            return ms.ToArray();
        }
    }

    public static Mail_Request_ListProto GetProto(byte[] buffer)
    {
        Mail_Request_ListProto proto = new Mail_Request_ListProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
        }
        return proto;
    }
}