//===================================================
//作    者：肖 源  
//创建时间：2020-11-26 09:39:41
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 获取邮件详情
/// </summary>
public struct Mail_Get_DetailProto : IProto
{
    public ushort ProtoCode { get { return 1005; } }

    public bool IsSuccess; //是否成功
    public string Name; //邮件名称
    public ushort ErrorCode; //错误编码

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteBool(IsSuccess);
            if(IsSuccess)
            {
                ms.WriteUTF8String(Name);
            }
            else
            {
                ms.WriteUShort(ErrorCode);
            }
            return ms.ToArray();
        }
    }

    public static Mail_Get_DetailProto GetProto(byte[] buffer)
    {
        Mail_Get_DetailProto proto = new Mail_Get_DetailProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.IsSuccess = ms.ReadBool();
            if(proto.IsSuccess)
            {
                proto.Name = ms.ReadUTF8String();
            }
            else
            {
                proto.ErrorCode = ms.ReadUShort();
            }
        }
        return proto;
    }
}