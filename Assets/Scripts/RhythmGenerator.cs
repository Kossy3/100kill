using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    public int[] Rhythm;

    public Database database;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public int[] generate_8bar_rhythm()
    {
        int[] Probability_List;

        if (database.Stages <= 1)
        {
            Probability_List = new int[] {0, 0, 4, 4};
        }

        else
        {
            Probability_List = new int[] {0, 1, 3, 4};
        }

        Rhythm = new int[64];

        List<int> change_point = new List<int>();

        for (int i = 0; i < 64; i++)
        {
            int rnd1 = Random.Range(0, 4);

            Rhythm[i] = Probability_List[rnd1]; 

            if (Rhythm[i] == 4)
            {
                change_point.Add(i);
            }
        }

        int rnd2 = Random.Range(0, change_point.Count);
        Rhythm[change_point[rnd2]] = 2;

        database.Stages ++;

        return Rhythm;
    }
}
