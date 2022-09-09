using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    private MyScore myscore;
    private ToKansuji tokansuji;

    public GameObject flame;

    private Text text_rank;
    private Text text_score;

    private List<GameObject> flame_list;

    public void Start()
    {
        flame_list = new List<GameObject>();

        myscore = GameObject.Find("MyScore").GetComponent<MyScore>();
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
    }

    public void ranking_displayer()
    {
        for (int i = 0; i < myscore.score_ranking.Count; i++)
        {
            GameObject obj = Instantiate(flame, new Vector3 ((460 - 80 * i), 5, 0), Quaternion.identity);
            obj.transform.SetParent(transform, false);
            
            obj.transform.Find("Rank").gameObject.transform.Find("Text(RANK)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(i + 1, "-") + "位";
            obj.transform.Find("Score").gameObject.transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(myscore.score_ranking[i], "〇") + "人切り";

            flame_list.Add(obj);
        }
    }
}
