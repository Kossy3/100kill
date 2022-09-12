using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyPattern
{
    public int[] enemy_pattern = new int[64];
}

public class RhythmGenerator : MonoBehaviour
{
    public int[] Rhythm;

    public Database database;

    [SerializeField]
    public EnemyPattern[] patterns = new EnemyPattern[5];

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public int[] generate_8bar_rhythm()
    {
        Rhythm = new int[64];

        if (database.Stages > 3)
        {
            Rhythm = patterns[4].enemy_pattern;
        }

        else
        {
            Rhythm = patterns[database.Stages + 1].enemy_pattern;
        }

        int n = 64;

        while (n > 1)
        {
            int m = UnityEngine.Random.Range(0, n);

            int temp = Rhythm[m];
            Rhythm[m] = Rhythm[n - 1];
            Rhythm[n - 1] = temp;

            n--;
        }

        database.Stages++;

        return Rhythm;
    }
}
