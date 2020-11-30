using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtil
{
    /// <summary>
    /// Int 扩展方法
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int tmp = 0;
        int.TryParse(str, out tmp);
        return tmp;
    
    }

    /// <summary>
    /// Float 扩展方法
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ToFloat(this string str)
    {
        float tmp = 0;
        float.TryParse(str, out tmp);
        return tmp;

    }







}
