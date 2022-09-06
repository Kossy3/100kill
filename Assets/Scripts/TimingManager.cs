using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    private Database database;
    private RhythmGenerator rhythmgenerator;
    private Player player;
    private Enemy enemy;

    public Enemy enemy1;
    //public Enemy enemy2;
    //public Enemy enemy3;
    //public Enemy enemy4;

    private int[] rhythm;

    private List<Enemy> spawn_enemy;

    private float keyinput_time;
    private List<float> spawn_time;

    private int rhythm_num;
    private int spawn_num;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        player = GameObject.Find("tatie").GetComponent<Player>();

        spawn_num = 0;
        spawn_enemy = new List<Enemy>();
        spawn_time = new List<float>();

        keyinput_time = 0f;
        

        StartCoroutine("start_game");
    }

    public void FixedUpdate()
    {
        if (spawn_time.Count > 1 && spawn_num < 64)
        {
            if (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.1f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.1f)
            {
                spawn_enemy[spawn_num].good();
                spawn_num ++;
            }

            else if (Time.time > spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f ||
            (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.2f))
            {
                spawn_enemy[spawn_num].miss();
                spawn_num ++;
            }
        }
    }

    public IEnumerator start_game()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine("EnemyGenerator");
    }

    public void getkey(int KeyID)
    {
        keyinput_time = Time.time;

        if (KeyID == 1)
        {
            player.jump();
            Debug.Log(KeyID);
        }

        else if (KeyID == 2)
        {
            player.skill();
            Debug.Log(KeyID);
        }

        else if (KeyID == 3)
        {
            player.sliding();
            Debug.Log(KeyID);
        }

        else if (KeyID == 4)
        {
            player.slash();
            Debug.Log(KeyID);
        }
    }

    public IEnumerator EnemyGenerator()
    {
        if (rhythm_num == 0 || rhythm_num == 64)
        {
            rhythm_num = 0;
            rhythm = rhythmgenerator.generate_8bar_rhythm();

            spawn_num = 0;
            spawn_enemy = new List<Enemy>();
            spawn_time = new List<float>();

            database.charge_skill_gauge(1);
            database.rise_BPM();
        }

        while (rhythm_num < 64)
        {
            if (rhythm[rhythm_num] != 0)
            {
                enemy = Instantiate(enemy1, new Vector3(10, 0, 0), Quaternion.identity);
                spawn_enemy.Add(enemy);
                spawn_time.Add(Time.time);
            }

            rhythm_num ++;

            yield return new WaitForSecondsRealtime((60 / (float)database.BPM) / 2);

            if ( rhythm_num == 64)
            {
                StartCoroutine("EnemyGenerator");
                break;
            }
        }
    }
}