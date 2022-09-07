using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Database : MonoBehaviour
{
    public int BPM = 100;
    public Color[] color_list = new Color[4];
    public int defeated_enemies;
    public List<int> defeated_color_number;
    public int Stages;
    public int HP = 5;
    public float skill_gauge;

    public void Start()
    {
        defeated_enemies = 0;
        Stages = 0;
        skill_gauge = 0;
    }

    public void rise_BPM()
    {
        BPM += 4;
    }

    public void add_defeat_color_number(int color_num)
    {
        defeated_color_number.Add(color_num);
    }

    public void charge_HP(int n)
    {
        HP += n;
        HP = Mathf.Clamp(HP, 0, 5);

        if (HP == 0)
        {
            SceneManager.LoadScene("Start");
        }
    }

    public void charge_skill_gauge(int n)
    {
        skill_gauge += n;
        skill_gauge = Mathf.Clamp(skill_gauge, 0, 1);
    }
}
