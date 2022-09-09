using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;

    private int minutes;
    private float seconds;

    public void Start()
    {
        text = GetComponent<Text>();

        minutes = 0;
        seconds = 0f;
    }

    public void FixedUpdate()
    {
        seconds += Time.deltaTime;

        if (seconds >= 60f)
        {
            minutes ++;
            seconds -= 60f;
        }

        string time_str;

        if (minutes == 0)
        {
            time_str = toKansuji((int)seconds) + "秒";
        }

        else
        {
            time_str = toKansuji(minutes) + "分" + toKansuji((int)seconds) + "秒";
        }

        text.text = time_str;
    }

    private string toKansuji(long number) 
    {
        if (number == 0) 
        {
            return "-";
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
