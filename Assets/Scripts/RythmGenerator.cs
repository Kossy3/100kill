using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmGenerator : MonoBehaviour
{
    public int[] Rhythm;

    public int[] generate_8bar_rhythm()
    {
        int[] Probability = {0, 1, 3, 4};

        Rhythm = new int[64];
        
        int n = 0;

        for (int i = 0; i < 64; i++)
        {
            int rnd1 = Random.Range(0, 4);

            Rhythm[i] = Probability[rnd1]; 

            if (Rhythm[i] == 4)
            {
                n ++;
            }
        }

        int[] change_point = new int[n];

        int rnd2 = Random.Range(0, change_point.Length - 1);
        Rhythm[change_point[rnd2]] = 2;

        return Rhythm;
    }
}
