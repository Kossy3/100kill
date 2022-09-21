using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private ToKansuji tokansuji;

    private Text text;

    private int minutes;
    private float seconds;

    private string time_str;

    public void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();

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

        if (minutes == 0)
        {
            time_str = tokansuji.to_kansuji((int)seconds, "-") + "秒";
        }

        else
        {
            time_str = tokansuji.to_kansuji(minutes, "-") + "分" + tokansuji.to_kansuji((int)seconds, "-") + "秒";
        }

        text.text = time_str;
    }
}
