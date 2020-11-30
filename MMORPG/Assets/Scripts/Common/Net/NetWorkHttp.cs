using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// Http 通讯管理
/// </summary>
public class NetWorkHttp : MonoBehaviour
{
    #region 单例
    private static NetWorkHttp instance;
    public static NetWorkHttp Instance 
    {
        get
        {
            if (instance==null)
            {
                GameObject obj = new GameObject("NetWorkHttp");
                
                DontDestroyOnLoad(obj);
                instance = obj.GetOrCreatComponent<NetWorkHttp>();
            }
            return instance;
        }     
    }
    #endregion

    //Web 请求回调
    private Action<CallBackArgs> m_CallBack;

    //Web 请求回调数据
    private CallBackArgs m_CallBackArgs;

    /// <summary>
    ///  是否繁忙
    /// </summary>
    private bool m_IsBusy = false;
    /// <summary>
    /// 是否繁忙，外部使用,但不能写
    /// </summary>
    public bool IsBusy 
    {
        get { return m_IsBusy; }
    
    }
    /// <summary>
    /// Web 请求回调数据
    /// </summary>
    public class CallBackArgs : EventArgs
    {
        /// <summary>
        /// 是否报错
        /// </summary>
        public bool HasError;
        /// <summary>
        /// Json 数据
        /// </summary>
        public string Json;
        /// <summary>
        /// 错误原因
        /// </summary>
        public string ErrorMsg;
    }
    private void Start()
    {
        m_CallBackArgs = new CallBackArgs();
    }
    //发送消息
    public void SendData( string url, Action<CallBackArgs> callBack, bool isPost=false, string json="")
    {
        if (m_IsBusy) return;
        m_IsBusy = true;
        m_CallBack = callBack;
        if (!isPost)
        {
            //WWW写法
            //GetUrl(url)

            //UnityWebRequest写法
            GetUrl(url);
        }
        else
        {
            //WWW写法
            //PostUrl(url, json);

            //UnityWebRequest写法
            PostUrl(url, json);
        }
    
    
    }
    #region  Get 请求
    /// <summary>
    ///  UnityWebRequest Get 请求
    /// </summary>
    /// <param name="url"></param>
    private void GetUrl(string url)
    {       
        StartCoroutine(Get(url));//启动协程
    }
    /// <summary>
    ///  Get 请求f服务器
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator Get(string url) 
    {
        //Get 请求
        UnityWebRequest data = UnityWebRequest.Get(url);
        //分段完成
        yield return data.SendWebRequest();//请求网络
        m_IsBusy = false;//当返回数据的时候
        if (string.IsNullOrEmpty(data.error))//没有错
        {
            if (data.downloadHandler.text=="null")
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.ErrorMsg = "未请求到数据";
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Json = data.downloadHandler.text;
                    m_CallBack(m_CallBackArgs);
                }
            }         
        }
        else
        {
            if (m_CallBack != null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorMsg = data.error;
                m_CallBack(m_CallBackArgs);
            }
        }
    }


    /*


    /// <summary>
    /// WWW Get 请求
    /// </summary>
    /// <param name="url"></param>
    private void GetUrl(string url) 
    {
        UnityWebRequest data = new UnityWebRequest(url);       
        StartCoroutine(Get(data));//启动协程

    }

    //协程
    private IEnumerator Get(UnityWebRequest data) 
    {
        yield return data;
        if (string.IsNullOrEmpty(data.error))//没有错
        {
            if (m_CallBack != null)
            {
                m_CallBackArgs.IsError = false;
                Debug.Log(data.downloadHandler.text);
                m_CallBackArgs.Json = data.downloadHandler.text;
                m_CallBack(m_CallBackArgs);


            }
        }
        else
        {
            if (m_CallBack!=null)
            {
                m_CallBackArgs.IsError = true;
                m_CallBackArgs.Error = data.error;
                m_CallBack(m_CallBackArgs);


            }
          
        }     
    }
    */
    #endregion

    #region Post 请求

    private void PostUrl(string url, string json)
    {
        //表单
        WWWForm form = new WWWForm();
        //给表单添加值
        form.AddField("", json);
        StartCoroutine(Request(url, form));
    }

    /// <summary>
    /// UnityWebRequest Post 请求服务器
    /// </summary>
    /// <param name="url">地址</param>
    /// <param name="wmpForm">表单，数据都在表单里</param>
    /// <returns></returns>
    IEnumerator Request(string url,WWWForm tmpForm) 
    {       
        UnityWebRequest data = UnityWebRequest.Post(url, tmpForm);
        yield return data.SendWebRequest();
        m_IsBusy = false;//当返回数据的时候
        if (string.IsNullOrEmpty(data.error))//没有错
        {
            if (data.downloadHandler.text == "null")
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.ErrorMsg = "未请求到数据";
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Json = data.downloadHandler.text;
                    m_CallBack(m_CallBackArgs);
                }
            }
        }
        else
        {
            if (m_CallBack != null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorMsg = data.error;
                m_CallBack(m_CallBackArgs);
            }
        }
    }

    #endregion
}
