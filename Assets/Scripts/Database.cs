using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Database : MonoBehaviour
{
    public int BPM;
    public int defeated_enemies;
    public List<int> defeated_color_number;
    public int Stages;
    public int HP;
    public float skill_gauge;

    public void Start()
    {
        BPM = 100;
        defeated_enemies = 0;
        defeated_color_number = new List<int>();
        Stages = 0;
        HP = 5;
        skill_gauge = 0;

        DontDestroyOnLoad(gameObject);
    }

    public void rise_BPM()
    {
        BPM += 8;
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
            SceneManager.LoadScene("Score");
        }
    }

    public void charge_skill_gauge(int n)
    {
        skill_gauge += n;
        skill_gauge = Mathf.Clamp(skill_gauge, 0, 1);
    }

    public void reset()
    {
        BPM = 100;
        defeated_enemies = 0;
        defeated_color_number = new List<int>();
        Stages = 0;
        HP = 5;
        skill_gauge = 0;
    }
}
