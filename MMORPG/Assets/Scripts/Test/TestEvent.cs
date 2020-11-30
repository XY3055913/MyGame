using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetWorkSocket.Instance.Connect("192.168.50.253", 1011);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Mail_Request_ListProto protoMail = new Mail_Request_ListProto();

            NetWorkSocket.Instance.SendMsg(protoMail.ToArray());
        }
    }
}
