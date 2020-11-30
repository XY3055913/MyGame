//===================================================
//作    者：肖 源  
//创建时间：2020-11-26 09:39:41
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 服务器返回邮件列表
/// </summary>
public struct Mail_Get_ListProto : IProto
{
    public ushort ProtoCode { get { return 1006; } }

    public int Count; //数量
    public int MailID; //邮件编号
    public string MailName; //邮件名称

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(Count);
            ms.WriteInt(MailID);
            ms.WriteUTF8String(MailName);
            return ms.ToArray();
        }
    }

    public static Mail_Get_ListProto GetProto(byte[] buffer)
    {
        Mail_Get_ListProto proto = new Mail_Get_ListProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.Count = ms.ReadInt();
            proto.MailID = ms.ReadInt();
            proto.MailName = ms.ReadUTF8String();
        }
        return proto;
    }
}