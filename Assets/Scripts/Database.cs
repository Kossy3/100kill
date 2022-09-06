using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Database : MonoBehaviour
{
    public int BPM = 100;
    public List<Enemy> defeated_enemies;
    public int Stages;
    public int HP = 5;
    public float skill_gauge;

    public void Start()
    {
        skill_gauge = 0;
        Stages = 0;
    }

    public void rise_BPM()
    {
        BPM += 4;
    }

    public void add_defeat_enemy(Enemy enemy)
    {
        defeated_enemies.Add(enemy);
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
