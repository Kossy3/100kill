using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToKansuji : MonoBehaviour
{
    public string to_kansuji(long number, string zero)
    {
        if (number == 0) 
        {
            return zero;
        }
        
        string[] kl = new string[] { "", "十", "百", "千" };
        string[] nl = new string[] { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        string str = "";

        int keta = 0;

        while (number > 0) 
        {
            int k = keta % 4;
            int n = (int)(number % 10);
                         
            if (k != 0 && n == 1) 
            {
                str = kl[k] + str;
            } 
            
            else if (n != 0) 
            {
                str = nl[n] + kl[k] + str;
            }
                
            keta++;
            number /= 10;
        }

        return str;
    }
}
