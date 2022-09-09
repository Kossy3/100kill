using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji;

    public GameObject flame;

    private Text text_rank;
    private Text text_score;

    private List<GameObject> flame_list;
    private List<int> score_ranking;

    public void Start()
    {
        flame_list = new List<GameObject>();
        score_ranking = new List<int>() {0};

        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();

        try
        {
            database = GameObject.Find("Database").GetComponent<Database>();

            bool[] scene_number_identifier = new bool[] {true, false};

            if (scene_number_identifier[database.scene_number])
            {
                database.score_list.Add(database.defeated_enemies);
            }

            //score_ranking.Add(database.defeated_enemies);
            score_ranking.AddRange(database.score_list);
        }

        catch (System.Exception)
        {

        }

        score_ranking.Sort((a, b) => b - a);

        for (int i = 0; i < score_ranking.Count - 1; i++)
        {
            GameObject obj = Instantiate(flame, new Vector3 ((460 - 80 * i), 5, 0), Quaternion.identity);
            obj.transform.SetParent(transform, false);
            
            obj.transform.Find("Rank").gameObject.transform.Find("Text(RANK)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(i + 1, "-") + "位";
            obj.transform.Find("Score").gameObject.transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(score_ranking[i], "〇") + "人切り";

            flame_list.Add(obj);
        }
    }
}
