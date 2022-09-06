using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private TimingManager timingmanager;
    private Image image;
    private Text text;

    private int count_num;

    public void Start()
    {
        timingmanager = GameObject.Find("TimingManager").GetComponent<TimingManager>();
        image = GetComponent<UnityEngine.UI.Image>();
        text = transform.Find("Text(CountDown)").gameObject.GetComponent<Text>();

        count_num = 0;

        StartCoroutine(count_down(0));
    }

    public IEnumerator count_down(int n)
    {
        if (image.enabled == false)
        {
            image.enabled = true;
        }

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }

        while (true)
        {
            if (count_num  == 0)
            {
                text.text = "三";
            }

            else if (count_num  == 1)
            {
                text.text = "二";
            }

            else if (count_num == 2)
            {
                text.text = "一";
            }

            else if (count_num == 3)
            {
                text.text = "";

                if (n == 0)
                {
                    timingmanager.start_game();
                }

                image.enabled = false;
                Time.timeScale = 1;

                break;
            }

            yield return new WaitForSecondsRealtime (1.0f);

            count_num ++;
        }
    }
}
