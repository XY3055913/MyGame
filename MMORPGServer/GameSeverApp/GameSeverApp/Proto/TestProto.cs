//===================================================
//作    者：肖 源  
//创建时间：2020-11-23 16:47:45
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 测试协议
/// </summary>
public struct TestProto : IProto
{
    //协议编号
    public ushort ProtoCode { get { return 1004; } }


    //协议数据
    public int Id; //编号
    public string Name; //名称
    public int Type; //类型
    public float Price; //价格
    /// <summary>
    /// 转成byte数组
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(Id);
            ms.WriteUTF8String(Name);
            ms.WriteInt(Type);
            ms.WriteFloat(Price);

         
            return ms.ToArray();
        }
    }

    /// <summary>
    /// 还原成对象
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static TestProto GetProto(byte[] buffer)
    {
        TestProto proto = new TestProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.Id = ms.ReadInt();
            proto.Name = ms.ReadUTF8String();
            proto.Type = ms.ReadInt();
            proto.Price = ms.ReadFloat();
        }
        return proto;
    }
}
