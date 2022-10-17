using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Defeats : MonoBehaviour
{
    private Database database;

    public List<GameObject> defeats_image_list;

    private List<int> defeated_color_list;

    private int[] empty_list;

    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();

        defeats_image_list = new List<GameObject>();

        for (int i = 0; i < 5; i++)
        {
            defeats_image_list.Add(gameObject.transform.GetChild(i).gameObject);
        }

        empty_list = new int[] {0, 0, 0, 0, 0};
    }

    private void Update()
    {
        defeated_color_list = new List<int>();
        List<int> defeated_color_number = database.defeated_color_number;

        if (defeated_color_number.Count < 5)
        {
            defeated_color_list.AddRange(defeated_color_number);
            defeated_color_list.AddRange(empty_list);
        }

        else
        {
            for (int i = 0; i < 5; i++)
            {
                defeated_color_list.Add(defeated_color_number[defeated_color_number.Count - 10 + i]);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            defeats_image_list[i].transform.GetChild(0).gameObject.SetActive(false);
            defeats_image_list[i].transform.GetChild(1).gameObject.SetActive(false);
            defeats_image_list[i].transform.GetChild(2).gameObject.SetActive(false);
            defeats_image_list[i].transform.GetChild(3).gameObject.SetActive(false);

            if (defeated_color_list[i] == 0)
            {
                defeats_image_list[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            if (defeated_color_list[i] == 1)
            {
                defeats_image_list[i].transform.GetChild(1).gameObject.SetActive(true);
            }

            if (defeated_color_list[i] == 2)
            {
                defeats_image_list[i].transform.GetChild(2).gameObject.SetActive(true);
            }

            if (defeated_color_list[i] == 3)
            {
                defeats_image_list[i].transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }
}