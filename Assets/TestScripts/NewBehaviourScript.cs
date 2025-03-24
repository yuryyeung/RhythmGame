using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int paid = 0;
    public int fee = 0;

    public int[] money = {500, 100, 50, 10, 5, 1}; 
    public List<int> amounts = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        Change();
    }

     public void Change()
    {
        int change = paid - fee;
        //int FH = change / 500;//500
        //change = change - FH * 500;

        //int H = change / 100;
        //change = change - H * 100;

        //int FT = change / 50;
        //change = change - FT * 50;

        //int T = change / 10;
        //change = change - T * 10;

        //int F = change / 5;
        //change = change - F * 5;

        //string output = "500Yen: " + FH + " | 100Yen:" + H + " | 50Yen: " + FT + " | 10Yen: " + T + " | 5Yen: " + F + " | 1Yen: " + change;

        string output = "";
        for (int coin = 0; coin < money.Length; coin++)
        {
            amounts.Add(change / money[coin]);
            change = change - change / money[coin] * money[coin];
        }

       

        for(int i=0; i<amounts.Count; i++) 
        {
            output += money[i] + "Yen: " + amounts[i] + " | ";
        }

        Debug.Log(output);
    }
}
