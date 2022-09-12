using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProbabilityPattern
{
    public int[] pattern = new int[20];
}

public class RhythmGenerator : MonoBehaviour
{
    public int[] Rhythm;

    public Database database;

    public ProbabilityPattern[] patterns = new ProbabilityPattern[5];

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public int[] generate_8bar_rhythm()
    {
        int[] probability_pattern = new int[20];

        if (database.Stages > 3)
        {
            probability_pattern = patterns[4].pattern;
        }

        else
        {
            probability_pattern = patterns[database.Stages + 1].pattern;
        }

        Rhythm = new int[64];

        List<int> change_point = new List<int>();

        for (int i = 0; i < 64; i++)
        {
            int rnd1 = UnityEngine.Random.Range(0, 20);

            Rhythm[i] = probability_pattern[rnd1]; 

            if (Rhythm[i] == 4)
            {
                change_point.Add(i);
            }
        }

        int rnd2 = UnityEngine.Random.Range(0, change_point.Count);
        Rhythm[change_point[rnd2]] = 2;

        database.Stages ++;

        return Rhythm;
    }
}
