using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    private Database database;
    private RhythmGenerator rhythmgenerator;
    private Enemy enemy;

    public Enemy enemy1;
    //public Enemy enemy2;
    //public Enemy enemy3;
    //public Enemy enemy4;

    private int input_key;
    private int[] rhythm;
    private int n = 0;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        rhythmgenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();

        start_game();
    }

    public void start_game()
    {
        StartCoroutine("StageManager");
    }

    public void getkey(int KeyID)
    {
        input_key = KeyID;
    }

    public IEnumerator StageManager()
    {
        rhythm = rhythmgenerator.generate_8bar_rhythm();

        StartCoroutine("EnemyGenerator");

        yield return new WaitForSecondsRealtime(((float)database.BPM / 60) * 32);

        database.charge_skill_gauge(1);
        database.rise_BPM();

        StartCoroutine("StageManager");
    }

    public IEnumerator EnemyGenerator()
    {
        if (rhythm[n] != 0)
        {
            enemy = Instantiate(enemy1, new Vector3(10, 0, 0), Quaternion.identity);
        }

        yield return new WaitForSecondsRealtime(((float)database.BPM / 60) / 2);

        if (n < 64)
        {
            StartCoroutine("EnemyGenerator");
        }

        n++;
    }
}

