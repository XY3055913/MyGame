using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTestModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.Instance.AddEvenListener(ProtoCodeDef.Mail_Get_List, OnGetList);
        
    }

    private void OnGetList(byte[] buffer)
    {
        Mail_Get_ListProto proto = Mail_Get_ListProto.GetProto(buffer);
        Debug.Log("Count" + proto.Count);
        Debug.Log("MailID:" + proto.MailID);
        Debug.Log("MailName:" + proto.MailName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Mail_Get_List, OnGetList);
    }

}
