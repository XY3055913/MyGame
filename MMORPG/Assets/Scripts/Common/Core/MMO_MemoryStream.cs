using System;
using System.IO;
using System.Text;

/// <summary>
/// 数据转换（byte short int long float decimal bool string ）
/// </summary>
public class MMO_MemoryStream : MemoryStream
{

    public MMO_MemoryStream() 
    {
    
    
    
    }

    public MMO_MemoryStream(byte[] buffer):base(buffer)
    { 
    
    
    
    }




    #region Short


    /// <summary>
    /// 从流中读取一个short数据
    /// </summary>
    /// <returns></returns>
    public short ReadShort() 
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);//0偏移，2读两位
        return BitConverter.ToInt16(arr, 0);   
    }

    /// <summary>
    /// 把一个short写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteShort(short value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    
    }

    #endregion

    #region UShort 无符号的


    /// <summary>
    /// 从流中读取一个ushort数据
    /// </summary>
    /// <returns></returns>
    public ushort ReadUShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);//0偏移，2读两位
        return BitConverter.ToUInt16(arr, 0);
    }

    /// <summary>
    /// 把一个ushort写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteUShort(ushort value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion


    #region Int


    /// <summary>
    /// 从流中读取一个int数据
    /// </summary>
    /// <returns></returns>
    public int ReadInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);//0偏移，2读两位
        return BitConverter.ToInt32(arr, 0);
    }

    /// <summary>
    /// 把一个int写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteInt(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion

    #region UInt 无符号的


    /// <summary>
    /// 从流中读取一个uint数据
    /// </summary>
    /// <returns></returns>
    public uint ReadUInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);//0偏移，2读两位
        return BitConverter.ToUInt32(arr, 0);
    }

    /// <summary>
    /// 把一个uint写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteUInt(uint value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion


    #region Long


    /// <summary>
    /// 从流中读取一个long数据
    /// </summary>
    /// <returns></returns>
    public long ReadLong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);//0偏移，8读八位
        return BitConverter.ToInt64(arr, 0);
    }

    /// <summary>
    /// 把一个long写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteLong(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion

    #region UInt 无符号的


    /// <summary>
    /// 从流中读取一个uint数据
    /// </summary>
    /// <returns></returns>
    public ulong ReadULong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);//0偏移，8读八位
        return BitConverter.ToUInt64(arr, 0);
    }

    /// <summary>
    /// 把一个uLong写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteULong(uint value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion

    #region Float


    /// <summary>
    /// 从流中读取一个float数据
    /// </summary>
    /// <returns></returns>
    public float ReadFloat()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);//0偏移，4读四位
        return BitConverter.ToSingle(arr, 0);
    }

    /// <summary>
    /// 把一个float写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteFloat(float value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion

    #region Double 

    /// <summary>
    /// 从流中读取一个double数据
    /// </summary>
    /// <returns></returns>
    public double ReadDoublr()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);//0偏移，8读八位
        return BitConverter.ToDouble(arr, 0);
    }

    /// <summary>
    /// 把一个double写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteDoublr(double value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);

    }

    #endregion

    #region Bool 

    /// <summary>
    /// 从流中读取一个bool数据
    /// </summary>
    /// <returns></returns>
    public bool ReadBool()
    {
        return base.ReadByte() == 1;//1为true，0为false；
    }

    /// <summary>
    /// 把一个bool写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteBool(bool value)
    {
        base.WriteByte((byte)(value == true ? 1 : 0));      
    }

    #endregion

    #region UTF8String
    /// <summary>
    /// 从流中读取一个string数组
    /// </summary>
    /// <returns></returns>
    public string ReadUTF8String() 
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];
        base.Read(arr, 0, len);
        return Encoding.UTF8.GetString(arr);//转换成UTF8
    
    }

    /// <summary>
    /// 把string写入流
    /// </summary>
    /// <param name="str"></param>
    public void WriteUTF8String(string str)
    {
        byte[] arr = Encoding.UTF8.GetBytes(str);//把UTF8专场字节数组
        if (arr.Length>65535)//字节数组太大，报异常
        {
            throw new InvalidCastException("字符串超出范围");
        }
        this.WriteUShort((ushort)arr.Length);//字符串的长度
        base.Write(arr, 0, arr.Length);//写入内容

    }

    #endregion


}
