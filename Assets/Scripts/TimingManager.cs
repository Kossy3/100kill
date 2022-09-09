using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    private Database database;
    private RhythmGenerator rhythmgenerator;
    private MusicGenerator musicgenerator;
    private MusicPlayer musicplayer;
    private Player player;
    private Enemy enemy;
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

    //private int[] frist_bar_rhythm;
    private int[] rhythm;
    private List<List<Note>> score;

    private List<Enemy> spawn_enemy;
    private List<int> spawn_type;

    private float keyinput_time;
    private List<float> spawn_time;

    private int rhythm_num;
    private int spawn_num;
    private int background_num;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        musicgenerator = GameObject.Find("MusicGenerator").GetComponent<MusicGenerator>();
        musicplayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        player = GameObject.Find("Player").GetComponent<Player>();

        enemy_type = new Enemy[] {null, upthing, null, stone, enemy1_0};
        color_type = new Enemy[] {null, enemy1, enemy1_2, enemy1_3};

        //frist_bar_rhythm = new int[] {0, 0, 0, 0, 0, 0};

        spawn_enemy = new List<Enemy>();
        spawn_type = new List<int>();

        keyinput_time = 0f;
        spawn_time = new List<float>();

        rhythm_num = 0;
        spawn_num = 0;
        background_num = 0;

        start_game();
    }

    public void FixedUpdate()
    {
        try
        {
            if (Time.time > spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f)
            {
                spawn_enemy[spawn_num].miss();
                spawn_num ++;
            }
        }

        catch (System.ArgumentOutOfRangeException)
        {
        }
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
        keyinput_time = Time.time;

        try
        {
            if (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.1f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.1f)
            {
                if (KeyID == spawn_type[spawn_num])
                {
                    spawn_enemy[spawn_num].good();
                    spawn_num ++;
                }

                else
                {
                    spawn_enemy[spawn_num].miss();
                    spawn_num ++;
                }
            }

            else if (keyinput_time <= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) + 0.2f &&
            keyinput_time >= spawn_time[spawn_num] + (60 / (float)database.BPM * 4) - 0.2f)
            {
                spawn_enemy[spawn_num].miss();
                spawn_num ++;
            }
        }

        catch (System.ArgumentOutOfRangeException)
        {
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

    public void stage_up()
    {
        database.charge_skill_gauge(1);
        database.rise_BPM(8);
    }

    public void music_player()
    {
        musicplayer.play_music(score);
    }

    public IEnumerator enemy_generator()
    {
        if (rhythm_num == 0)
        {
            rhythm = rhythmgenerator.generate_8bar_rhythm();
            score = musicgenerator.generate_8bar_music(rhythm);

            Invoke("stage_up", (60 / (float)database.BPM) * 4);
            Invoke("music_player", ((60 / (float)database.BPM) * 4) - 0.3f);
        }

        while (rhythm_num < 64)
        {
            if (enemy_type[rhythm[rhythm_num]])
            {
                enemy = Instantiate(enemy_type[rhythm[rhythm_num]], new Vector3(10, -2, 0), Quaternion.identity);

                spawn_enemy.Add(enemy);
                spawn_type.Add(rhythm[rhythm_num]);

                spawn_time.Add(Time.time);
            }

            else if (rhythm[rhythm_num] == 2)
            {
                int rnd = Random.Range(1, 4);
                enemy = Instantiate(color_type[rnd], new Vector3(10, -2, 0), Quaternion.identity);
                enemy.color_number = rnd;

                spawn_enemy.Add(enemy);
                spawn_type.Add(rhythm[rhythm_num]);

                spawn_time.Add(Time.time);
            }

            rhythm_num ++;

            yield return new WaitForSeconds((60 / (float)database.BPM) / 2);

            if ( rhythm_num == 64)
            {
                rhythm_num = 0;

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