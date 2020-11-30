//===================================================
//作    者：肖 源  
//创建时间：2020-11-26 09:39:41
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 从服务器返回道具列表
/// </summary>
public struct Mail_Ret_ListProto : IProto
{
    public ushort ProtoCode { get { return 2001; } }

    public int ItemCount; //道具数量
    public List<ProducItem> ItemList; //道具集合

    /// <summary>
    /// 道具集合
    /// </summary>
    public struct ProducItem
    {
        public int ItemId; //道具编号
        public string ItemName; //道具名称
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(ItemCount);
            for (int i = 0; i < ItemCount; i++)
            {
                ms.WriteInt(ItemList[i].ItemId);
                ms.WriteUTF8String(ItemList[i].ItemName);
            }
            return ms.ToArray();
        }
    }

    public static Mail_Ret_ListProto GetProto(byte[] buffer)
    {
        Mail_Ret_ListProto proto = new Mail_Ret_ListProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.ItemCount = ms.ReadInt();
            proto.ItemList = new List<ProducItem>();
            for (int i = 0; i < proto.ItemCount; i++)
            {
                ProducItem _Item = new ProducItem();
                _Item.ItemId = ms.ReadInt();
                _Item.ItemName = ms.ReadUTF8String();
                proto.ItemList.Add(_Item);
            }
        }
        return proto;
    }
}