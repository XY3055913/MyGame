using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountEntity 
{
   
    public int Id { get; set; }

    public string UserName { get; set; }

    public string PassWord { get; set; }

    public int Coin { get; set; }
    public int Diamond { get; set; }

    public DateTime Logindate { get; set; }

    public string Logintype { get; set; }

    public string Token { get; set; }
}
