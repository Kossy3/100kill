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
    private List<int> spawn_index;

    private int spawn_num;
    private int background_num;
    private List<float> wave_start_time;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        musicgenerator = GameObject.Find("MusicGenerator").GetComponent<MusicGenerator>();
        musicplayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        player = GameObject.Find("Player").GetComponent<Player>();

        enemy_type = new Enemy[] {null, stone, null, upthing, enemy1_0};
        color_type = new Enemy[] {null, enemy1, enemy1_2, enemy1_3};

        //frist_bar_rhythm = new int[] {0, 0, 0, 0, 0, 0};

        spawn_enemy = new List<Enemy>();
        spawn_type = new List<int>();
        spawn_index = new List<int>();

        wave_start_time = new List<float>();
        spawn_num = 0;
        background_num = 0;

        start_game();
    }

    public void FixedUpdate()
    {
        if(spawn_enemy.Count > spawn_num && wave_start_time.Count > spawn_num%64)
        {
            float perfect_time = get_perfect_time();
            if (Time.time >= perfect_time )
            {
                spawn_enemy[spawn_num].miss();
                Debug.Log(perfect_time);
                spawn_num ++;
            }
        }
    }

    public float get_perfect_time(){
        return wave_start_time[spawn_num%64] + (60 / ((float)database.BPM) / 2) * spawn_index[spawn_num];
    }

    public void start_game()
    {
        background = Instantiate(Stage1_frist, new Vector3(0.5f, 0, 0), Quaternion.identity);

        //score = musicgenerator.generate_8bar_music(frist_bar_rhythm);
        //musicplayer.play_music(score);

        StartCoroutine("enemy_generator");
        StartCoroutine("scroll_background");
    }

    public void getkey(int KeyID)
    {
        float keyinput_time = Time.time;

        if(spawn_enemy.Count > spawn_num && wave_start_time.Count > spawn_num%64)
        {
            float perfect_time = wave_start_time[spawn_num%64] + (60 / ((float)database.BPM) / 2) * spawn_index[spawn_num];
            if (Math.Abs(perfect_time - keyinput_time) <= 0.1f)
            {
                if (KeyID == spawn_type[spawn_num])
                {
                    spawn_enemy[spawn_num].good();
                }
                else
                {
                    spawn_enemy[spawn_num].miss();
                }
                spawn_num ++;
            }

            else if (Math.Abs(perfect_time - keyinput_time) <= 0.2f)
            {
                spawn_enemy[spawn_num].miss();
                spawn_num ++;
            }
        }
        
        if (KeyID == 1)
        {
            player.jump();
        }

        else if (KeyID == 2 && database.skill_gauge == 1)
        {
            player.skill();
            database.charge_skill_gauge(-1);
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

    public IEnumerator stage_up(List<List<Note>> score)
    {
        yield return new WaitForSeconds((60 / (float)database.BPM) * 4 -0.1f);
        database.rise_BPM(8);
        wave_start_time.Add(Time.time);
        musicplayer.play_music(score, database.BPM);
    }

    public IEnumerator enemy_generator()
    {
        Enemy enemy;
        database.charge_skill_gauge(1);
        int rhythm_num = 0;
        //int [] rhythm = rhythmgenerator.generate_8bar_rhythm();
        int [] rhythm = new int[]{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4};
        List<List<Note>> score = musicgenerator.generate_8bar_music(rhythm);
        StartCoroutine(stage_up(score));

        while (rhythm_num < 64)
        {
            if(rhythm[rhythm_num] > 0){
                if(rhythm[rhythm_num] == 2){
                    int rnd = UnityEngine.Random.Range(1, 4);
                    enemy = Instantiate(color_type[rnd], new Vector3(10, -2, 0), Quaternion.identity);
                    enemy.color_number = rnd;
                } else {
                    enemy = Instantiate(enemy_type[rhythm[rhythm_num]], new Vector3(10, -2, 0), Quaternion.identity);
                }
                
                spawn_enemy.Add(enemy);
                spawn_type.Add(rhythm[rhythm_num]);
                spawn_index.Add(rhythm_num);
            }

            rhythm_num ++;

            yield return new WaitForSeconds((60 / (float)database.BPM) / 2);

            if ( rhythm_num == 64)
            {
                StartCoroutine("enemy_generator");
                break;
            }
        }
    }

    private IEnumerator scroll_background()
    {
        while (true)
        {
            if (background_num < 25)
            {
                background = Instantiate(background_order[background_num], new Vector3(18, 0, 0), Quaternion.identity);

                background_num ++;
            }

            else
            {
                background = Instantiate(Stage4, new Vector3(18, 0, 0), Quaternion.identity);
            }

            yield return new WaitForSeconds (60 / (float)database.BPM * 4);
        }
    }
}