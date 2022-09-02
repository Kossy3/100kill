using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmGenerator : MonoBehaviour
{
    public int[] Rhythm;

    private Database database;

    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public int[] generate_8bar_rhythm()
    {
        int[] Probability_List;

        if (database.Stages <= 2)
        {
            Probability_List = new int[] {0, 0, 4, 4};
        }

        else
        {
            Probability_List = new int[] {0 , 1, 3, 4};
        }

        Rhythm = new int[64];
        
        int n = 0;

        for (int i = 0; i < 64; i++)
        {
            int rnd1 = Random.Range(0, 4);

            Rhythm[i] = Probability_List[rnd1]; 

            if (Rhythm[i] == 4)
            {
                n ++;
            }
        }

        int[] change_point = new int[n];

        int rnd2 = Random.Range(0, change_point.Length - 1);
        Rhythm[change_point[rnd2]] = 2;

        database.Stages ++;

        return Rhythm;
    }
}
