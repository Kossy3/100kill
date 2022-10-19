using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Database : MonoBehaviour
{
    [SerializeField]
    [Header("Debug_mode : trueで死亡時のシーン変更無効化")]
    public bool debug_mode;
    public int BPM;
    public int defeated_enemies;
    public int defeated_color_enemies;
    public List<int> defeated_color_number;
    public int Stages;
    public int HP;
    public float skill_gauge;
    public float playing_time;
    public bool finish;

    public int scene_number;

    public void Start()
    {
        BPM = 100;
        defeated_enemies = 0;
        defeated_color_enemies = 0;
        defeated_color_number = new List<int>();
        Stages = 0;
        HP = 5;
        skill_gauge = 0;
        playing_time = 0;

#if !UNITY_EDITOR  //開発環境以外でデバッグモードを強制解除
        debug_mode = false;
#endif 

        DontDestroyOnLoad(gameObject);
    }

    public void FixedUpdate()
    {
        if (HP != 0)
        {
            playing_time += Time.deltaTime;
        }
    }

    public int get_rised_BPM()
    {
        return BPM + 8;
    }

    public void rise_BPM()
    {
        BPM = get_rised_BPM();
    }

    public void add_defeat_color_number(int color_num)
    {
        defeated_color_number.Add(color_num);
    }

    public void charge_HP(int n)
    {
        HP += n;
        HP = Mathf.Clamp(HP, 0, 5);

        if (HP == 0 && !debug_mode && finish)
        {
            scene_number = 0;
            finish = false;
            SceneManager.LoadScene("Score");
        }
    }

    public void charge_skill_gauge(int n)
    {
        skill_gauge += n;
        skill_gauge = Mathf.Clamp(skill_gauge, 0, 4);
    }

    public void reset()
    {
        Destroy(this);
    }
}