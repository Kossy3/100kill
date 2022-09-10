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

    private Scrollbar scrollbar;

    private int my_score;
    private int my_ranking;

    private List<GameObject> flame_list;
    public List<int> score_ranking;

    private bool on_left;
    private bool on_right;

    public void Start()
    {
        scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();

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
                my_score = database.defeated_enemies;
            }

            //score_ranking.Add(database.defeated_enemies);
            score_ranking.AddRange(database.score_list);
        }

        catch (System.Exception)
        {
            my_score = -1;
        }

        score_ranking.Sort((a, b) => b - a);

        if (score_ranking.Count > 14)
        {
            GameObject panel = GameObject.Find("Panel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2 (1044 + ((score_ranking.Count - 14) * 80), 620);
            panel.transform.localPosition = new Vector3 (- ((1044 + ((score_ranking.Count - 14) * 80)) / 2), 0, 0);
        }


        for (int i = 0; i < score_ranking.Count - 1; i++)
        {
            GameObject obj = Instantiate(flame, new Vector3 ((40 - 80 * (i + 1) - 2), 8, 0), Quaternion.identity);
            obj.transform.SetParent(transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform, false);
            
            obj.transform.Find("Rank").gameObject.transform.Find("Text(RANK)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(i + 1, "-") + "位";
            obj.transform.Find("Score").gameObject.transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(score_ranking[i], "〇") + "人切り";

            flame_list.Add(obj);
        }
        
        if (score_ranking.Count > 1)
        {
            flame_list[0].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.85f, 0.7f, 0, 1);
            flame_list[0].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[0].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.85f, 0.7f, 0, 1);
            flame_list[0].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (score_ranking.Count > 2)
        {
            flame_list[1].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.7f, 0.7f, 0.7f, 1);
            flame_list[1].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[1].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.7f, 0.7f, 0.7f, 1);
            flame_list[1].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (score_ranking.Count > 3)
        {
            flame_list[2].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.75f, 0.5f, 0.35f, 1);
            flame_list[2].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[2].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.75f, 0.5f, 0.35f, 1);
            flame_list[2].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (my_score > 0)
        {
            my_ranking = score_ranking.IndexOf(my_score) + 1;

            flame_list[my_ranking - 1].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
            flame_list[my_ranking -1].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().fontStyle  = FontStyle.Bold;
            flame_list[my_ranking - 1].transform.Find("Rank").gameObject.GetComponent<Image>().color =
            new Color (0.95f, 0.95f, 0.95f, 1);
            flame_list[my_ranking - 1].transform.Find("Score").gameObject.GetComponent<Image>().color =
            new Color (0.95f, 0.95f, 0.95f, 1);

            if (flame_list.Count > 13)
            {
                if (my_ranking > 6 && my_ranking < flame_list.Count - 6)
                {
                    transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform.localPosition =
                    new Vector3 (-flame_list[my_ranking - 1].transform.localPosition.x, 0, 0);
                }

                else if (my_ranking >= flame_list.Count - 6)
                {
                    transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform.localPosition =
                    new Vector3 (-flame_list[flame_list.Count - 7].transform.localPosition.x, 0, 0);
                }
            }
        }
    }

    public void Update()
    {
        if (scrollbar.value <= 0 || scrollbar.size == 1)
        {
            GameObject.Find("Button(LEFT)").GetComponent<Image>().color = new Color (0, 0, 0, 0.5f);
        }

        else
        {
            GameObject.Find("Button(LEFT)").GetComponent<Image>().color = new Color (0, 0, 0, 1);
        }

        if (scrollbar.value >= 1 || scrollbar.size == 1)
        {
            GameObject.Find("Button(RIGHT)").GetComponent<Image>().color = new Color (0, 0, 0, 0.5f);
        }

        else
        {
            GameObject.Find("Button(RIGHT)").GetComponent<Image>().color = new Color (0, 0, 0, 1);
        }

        if ((on_left || Input.GetKey(KeyCode.A)) && scrollbar.value > 0)
        {
            scrollbar.value -= 0.01f;
        }

        if ((on_right || Input.GetKey(KeyCode.D)) && scrollbar.value < 1)
        {
            scrollbar.value += 0.01f;
        }
    }

    public void down_click_left()
    {
        on_left = true;
    }

    public void up_click_left()
    {
        on_left = false;
    }

    public void down_click_right()
    {
        on_right = true;
    }

    public void up_click_right()
    {
        on_right = false;
    }
}