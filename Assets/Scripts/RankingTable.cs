using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankingTable : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji;
    private MyScore myscore;

    public GameObject flame;

    private Text text_rank;
    private Text text_score;

    private Scrollbar scrollbar;

    private int my_score;
    private int my_ranking;

    private Dictionary<string, int> ranking_list;

    private List<GameObject> flame_list;

    private bool on_bottom;
    private bool on_top;

    public int mode;

    public void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        myscore = GameObject.Find("MyScore").GetComponent<MyScore>();

        scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();

        mode = 0;
        
        try
        {
            database = GameObject.Find("Database").GetComponent<Database>();
            if (database.scene_number == 1)
            {
                generate_ranking();
            }
        }

        catch (System.Exception)
        {
        }
    }

    public void Update()
    {
        if (scrollbar.value <= 0 || scrollbar.size == 1)
        {
            GameObject.Find("Button(BOTTOM)").GetComponent<Image>().color = new Color (0, 0, 0, 0.5f);
        }

        else
        {
            GameObject.Find("Button(BOTTOM)").GetComponent<Image>().color = new Color (0, 0, 0, 1);
        }

        if (scrollbar.value >= 1 || scrollbar.size == 1)
        {
            GameObject.Find("Button(TOP)").GetComponent<Image>().color = new Color (0, 0, 0, 0.5f);
        }

        else
        {
            GameObject.Find("Button(TOP)").GetComponent<Image>().color = new Color (0, 0, 0, 1);
        }

        if ((on_bottom || Input.GetKey(KeyCode.S)) && scrollbar.value > 0)
        {
            scrollbar.value -= 0.01f;
        }

        if ((on_top || Input.GetKey(KeyCode.W)) && scrollbar.value < 1)
        {
            scrollbar.value += 0.01f;
        }
    }

    public void generate_ranking()
    {
        ranking_list = new Dictionary<string, int>();

        flame_list = new List<GameObject>();

        foreach (KeyValuePair<string, List<int>> item in database.score_list)
        {
            ranking_list.Add(item.Key, item.Value[mode]);
        }

        ranking_list = ranking_list.OrderByDescending(v => v.Value).ToDictionary(key => key.Key, val => val.Value);

        if (ranking_list.Count > 8)
        {
            GameObject panel = GameObject.Find("Panel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2 (1040, 644 + (ranking_list.Count - 8) * 80);
            panel.transform.localPosition = new Vector3 (0, -((644 + ((ranking_list.Count - 8) * 80)) / 2), 0);
        }

        int index = 0;

        foreach (KeyValuePair<string, int> item in ranking_list)
        {
            GameObject obj = Instantiate(flame, new Vector3 (-8, 40 - 80 * (index + 1) - 2, 0), Quaternion.identity);

            obj.transform.SetParent(transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform, false);
            obj.transform.Find("Rank").gameObject.transform.Find("Text(RANK)").gameObject.GetComponent<Text>().text =
            tokansuji.to_kansuji(index + 1, "-") + "位";
            obj.transform.Find("PlayerName").gameObject.transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Text>().text =
            item.Key;

            if (item.Key == myscore.player_name)
            {
                my_ranking = index;
            }

            if (mode == 0)
            {
                obj.transform.Find("Score").gameObject.transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().text =
                tokansuji.to_kansuji(item.Value, "〇") + "人切り";
            }

            else
            {
                int seconds = item.Value;
                int minutes = 0;

                while (true)
                {
                    if (seconds >= 60)
                    {
                        minutes ++;
                        seconds -= 60;
                    }

                    else
                    {
                        break;
                    }
                }

                string time_str;

                if (minutes == 0)
                {
                    time_str = tokansuji.to_kansuji(seconds, "-") + "秒";
                }

                else
                {
                    time_str = tokansuji.to_kansuji(minutes, "-") + "分" + tokansuji.to_kansuji(seconds, "-") + "秒";
                }

                obj.transform.Find("Score").gameObject.transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().text = time_str;
            }

            flame_list.Add(obj);

            index ++;
        }
        
        if (ranking_list.Count > 0)
        {
            flame_list[0].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.85f, 0.7f, 0, 1);
            flame_list[0].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[0].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Text>().color = 
            new Color (0.85f, 0.7f, 0, 1);
            flame_list[0].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[0].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.85f, 0.7f, 0, 1);
            flame_list[0].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (ranking_list.Count > 1)
        {
            flame_list[1].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.7f, 0.7f, 0.7f, 1);
            flame_list[1].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);            
            flame_list[1].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Text>().color = 
            new Color (0.7f, 0.7f, 0.7f, 1);
            flame_list[1].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[1].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.7f, 0.7f, 0.7f, 1);
            flame_list[1].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (ranking_list.Count > 2)
        {
            flame_list[2].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().color = 
            new Color (0.75f, 0.5f, 0.35f, 1);
            flame_list[2].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[2].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Text>().color = 
            new Color (0.75f, 0.5f, 0.35f, 1);
            flame_list[2].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
            flame_list[2].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().color =
            new Color (0.75f, 0.5f, 0.35f, 1);
            flame_list[2].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Shadow>().effectColor =
            new Color (0, 0, 0, 1);
        }

        if (database.scene_number == 0)
        {
            flame_list[my_ranking].transform.Find("Rank").gameObject.
            transform.Find("Text(RANK)").gameObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
            flame_list[my_ranking].transform.Find("PlayerName").gameObject.
            transform.Find("Text(PLAYERNAME)").gameObject.GetComponent<Text>().fontStyle  = FontStyle.Bold;
            flame_list[my_ranking].transform.Find("Score").gameObject.
            transform.Find("Text(SCORE)").gameObject.GetComponent<Text>().fontStyle  = FontStyle.Bold;

            flame_list[my_ranking].transform.Find("Rank").gameObject.GetComponent<Image>().color =
            new Color (0.95f, 0.95f, 0.95f, 0.08f);
            flame_list[my_ranking].transform.Find("PlayerName").gameObject.GetComponent<Image>().color =
            new Color (0.95f, 0.95f, 0.95f, 0.08f);
            flame_list[my_ranking].transform.Find("Score").gameObject.GetComponent<Image>().color =
            new Color (0.95f, 0.95f, 0.95f, 0.08f);

            if (flame_list.Count > 8)
            {
                if (my_ranking > 4 && my_ranking < flame_list.Count - 4)
                {
                    transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform.localPosition =
                    new Vector3 (0, -flame_list[my_ranking].transform.localPosition.y, 0);
                }

                else if (my_ranking >= flame_list.Count - 4)
                {
                    transform.Find("Viewport").gameObject.transform.Find("Panel").gameObject.transform.localPosition =
                    new Vector3 (0, -flame_list[flame_list.Count - 4].transform.localPosition.y, 0);
                }
            }
        }
    }

    public void down_click_bottom()
    {
        on_bottom = true;
    }

    public void up_click_bottom()
    {
        on_bottom = false;
    }

    public void down_click_top()
    {
        on_top = true;
    }

    public void up_click_top()
    {
        on_top = false;
    }
}