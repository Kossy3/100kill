using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TimingManager : MonoBehaviour
{
    private Database database;
    private RhythmGenerator rhythmgenerator;
    private MusicGenerator musicgenerator;
    private MusicPlayer musicplayer;
    private Player player;
    private BackGround background;
    private CameraShake shake;

    public Enemy enemy1_0;
    public Enemy enemy1;
    public Enemy enemy1_2;
    public Enemy enemy1_3;
    public Enemy stone;
    public Enemy upthing;

    public BackGround[] background_order = new BackGround[25];
    public BackGround Stage1_frist;
    public BackGround Stage4;

    private Enemy[] enemy_type;
    private Enemy[] color_type;

    private List<Enemy> spawn_enemy;
    private List<int> spawn_type;
    private List<float> spawn_time;

    private int spawn_num;
    private int background_num;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        musicgenerator = GameObject.Find("MusicGenerator").GetComponent<MusicGenerator>();
        musicplayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        player = GameObject.Find("Player").GetComponent<Player>();
        shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        enemy_type = new Enemy[] { null, stone, null, upthing, enemy1_0 };
        color_type = new Enemy[] { null, enemy1, enemy1_2, enemy1_3 };

        //frist_bar_rhythm = new int[] {0, 0, 0, 0, 0, 0};

        spawn_enemy = new List<Enemy>();
        spawn_type = new List<int>();
        spawn_time = new List<float>();
        spawn_num = 0;
        background_num = 0;

        start_game();
    }

    public void FixedUpdate()
    {
        if (spawn_enemy.Count > spawn_num )
        {
            float perfect_time = spawn_time[spawn_num];
            if (Time.time > perfect_time + 0.2f)
            {
                spawn_enemy[spawn_num].miss();
                //Debug.Log(perfect_time);
                spawn_num++;
            }
        }
    }

    public float get_perfect_time()
    {
        return 0;
    }

    public void start_game()
    {
        background = Instantiate(Stage1_frist, new Vector3(0.5f, 0, 0), Quaternion.identity);

        //score = musicgenerator.generate_8bar_music(frist_bar_rhythm);
        //musicplayer.play_music(score);

        StartCoroutine("enemy_generator");
    }

    public void getkey(int KeyID)
    {
        float keyinput_time = Time.time;

        if (KeyID == 2 && database.skill_gauge == 4)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().good();
                spawn_num ++;
            }

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            foreach (GameObject obstacle in obstacles)
            {
                obstacle.GetComponent<Enemy>().good();
                obstacle.GetComponent<Animator>().SetTrigger("obstacle");
                spawn_num ++;
            }
        }

        else if (spawn_enemy.Count > spawn_num)
        {
            float perfect_time = spawn_time[spawn_num];
            if (Math.Abs(perfect_time - keyinput_time) <= 0.1f )
            {
                if (KeyID == spawn_type[spawn_num] || (KeyID == 4 && spawn_type[spawn_num] == 2))
                {
                    spawn_enemy[spawn_num].good();
                }
                else
                {
                    spawn_enemy[spawn_num].miss();
                }
                spawn_num++;
            }

            else if (Math.Abs(perfect_time - keyinput_time) <= 0.2f)
            {
                spawn_enemy[spawn_num].miss();
                spawn_num++;
            }
        }

        if (KeyID == 1)
        {
            player.jump();
        }

        else if (KeyID == 2 && database.skill_gauge == 4)
        {
            player.skill();
            shake.Shake(0.5f, 0.1f);
            database.charge_skill_gauge(-4);
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

    IEnumerator stage_up(int[] rhythm)
    {
        yield return new WaitForSeconds((60 / (float)database.BPM) * 4);
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        database.rise_BPM();
        List<List<Note>> score = musicgenerator.generate_8bar_music(rhythm);
        //musicplayer.play_music(score, database.BPM);
        musicplayer.play_mid(score, database.BPM);
        sw.Stop();
        //Debug.Log($"処理時間 {sw.Elapsed}");
    }
    public IEnumerator enemy_generator()
    {
        int [] rhythm = rhythmgenerator.generate_8bar_rhythm();
        //int[] rhythm = new int[] { 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

        int rhythm_num = 0;
        float delta = 0f;
        float time = Time.time;
        while (rhythm_num < 64)
        {
            delta += get_delta(rhythm_num, 1);
            if (rhythm[rhythm_num] > 0)
            {
                StartCoroutine(create_enemy(rhythm[rhythm_num], delta));
                spawn_type.Add(rhythm[rhythm_num]);
                spawn_time.Add(time + delta + get_delta(rhythm_num, 8));
            }
            if(rhythm_num % 8 == 0){
                StartCoroutine(scroll_background(delta));
            }
            rhythm_num++;
        }

        StartCoroutine(stage_up(rhythm));
        yield return new WaitForSeconds(delta);
        StartCoroutine("enemy_generator");
    }

    float get_delta(int rhythm_num, int n)
    {
        float delta = 0;
        for (int i = 0; i < n; i++)
        {
            if (rhythm_num + i < 8)
            {
                delta += (60 / (float)database.BPM / 2);
            }
            else
            {
                delta += (60 / (float)database.get_rised_BPM() / 2);
            }
            rhythm_num ++;
        }
        return delta;
    }

    private IEnumerator create_enemy(int rhythm, float delta)
    {
        yield return new WaitForSeconds(delta);
        Enemy enemy;
        if (rhythm == 2)
        {
            int rnd = UnityEngine.Random.Range(1, 4);
            enemy = Instantiate(color_type[rnd], new Vector3(11, -1.6f, 0), Quaternion.identity);
            enemy.color_number = rnd;
        }
        else
        {
            enemy = Instantiate(enemy_type[rhythm], new Vector3(11, -1.6f, 0), Quaternion.identity);
        }
        spawn_enemy.Add(enemy);
    }

    private IEnumerator scroll_background(float delta)
    {
        yield return new WaitForSeconds(delta);
            if (background_num < 25)
            {
                background = Instantiate(background_order[background_num], new Vector3(18, 0, 0), Quaternion.identity);

                background_num++;
            }

            else
            {
                background = Instantiate(Stage4, new Vector3(18, 0, 0), Quaternion.identity);
            }
    }
}