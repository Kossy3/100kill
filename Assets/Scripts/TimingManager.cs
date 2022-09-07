using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    private Database database;
    private RhythmGenerator rhythmgenerator;
    private Player player;
    private Enemy enemy;

    public Enemy enemy1_0;
    public Enemy enemy1_1;
    public Enemy enemy1_2;
    public Enemy enemy1_3;
    public Enemy stone;
    public Enemy upthing;

    private Enemy[] enemy_type;
    private Enemy[] color_enemy_type;

    private int[] rhythm;

    private List<Enemy> spawn_enemy;

    private float keyinput_time;
    private List<float> spawn_time;

    private int rhythm_num;
    private int spawn_num;

    private int input_key;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        player = GameObject.Find("Player").GetComponent<Player>();

        enemy_type = new Enemy[] {null, stone, null, upthing, enemy1_0};
        color_enemy_type = new Enemy[] {null, enemy1_1, enemy1_2, enemy1_3};

        spawn_enemy = new List<Enemy>();

        keyinput_time = 0f;
        spawn_time = new List<float>();

        spawn_num = 0;
    }

    public void FixedUpdate()
    {
        Debug.Log(input_key);

        if (spawn_time.Count > 1 && spawn_num < rhythm_num)
        {
            if (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.1f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.1f)
            {
                if(input_key == rhythm[spawn_num])
                {
                    spawn_enemy[spawn_num].good();
                    spawn_num ++;
                    input_key = 0;
                }

                else
                {
                    spawn_enemy[spawn_num].miss();
                    spawn_num ++;
                    input_key = 0;
                }
            }

            else if (Time.time > spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f ||
            (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.2f))
            {
                spawn_enemy[spawn_num].miss();
                spawn_num ++;
                input_key = 0;
                Debug.Log(false);
            }
        }
    }

    public void start_game()
    {
        StartCoroutine("EnemyGenerator");
    }

    public void getkey(int KeyID)
    {
        keyinput_time = Time.time;
        input_key = KeyID;

        if (KeyID == 1)
        {
            player.jump();
        }

        else if (KeyID == 2)
        {
            player.skill();
        }

        else if (KeyID == 3)
        {
            player.sliding();
        }

        else if (KeyID == 4)
        {
            player.slash();
        }
    }

    public IEnumerator EnemyGenerator()
    {
        if (rhythm_num == 0 || rhythm_num == 64)
        {
            rhythm = rhythmgenerator.generate_8bar_rhythm();

            database.charge_skill_gauge(1);
            database.rise_BPM();

            spawn_enemy = new List<Enemy>();
            spawn_time = new List<float>();

            rhythm_num = 0;
            spawn_num = 0;
        }

        while (rhythm_num < 64)
        {
            if (enemy_type[rhythm[rhythm_num]])
            {
                enemy = Instantiate(enemy_type[rhythm[rhythm_num]], new Vector3(10, -2, 0), Quaternion.identity);
                spawn_enemy.Add(enemy);
                spawn_time.Add(Time.time);

                /*if (rhythm[rhythm_num] == 4)
                {
                    //enemyにcolor_number[0]を指定
                }*/
            }

            else if (rhythm[rhythm_num] == 2)
            {
                int rnd = Random.Range(1, 3);
                enemy = Instantiate(color_enemy_type[rnd], new Vector3(10, -2, 0), Quaternion.identity);

                //enemyにcolor_number[rnd]を指定

                spawn_enemy.Add(enemy);
                spawn_time.Add(Time.time);
            }

            rhythm_num ++;

            yield return new WaitForSeconds((60 / (float)database.BPM) / 2);

            if ( rhythm_num == 64)
            {
                StartCoroutine("EnemyGenerator");
                break;
            }
        }
    }
}