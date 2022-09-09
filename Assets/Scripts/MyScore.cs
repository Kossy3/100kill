using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyScore : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji;
    private RankingTable rankingtable;

    private GameObject ranking;
    private GameObject escapebutton;

    private Image image;
    private Text text;

    private int my_score;

    public List<int> score_ranking;

    void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();

        ranking = GameObject.Find("Ranking");
        escapebutton = GameObject.Find("EscapeButton");

        ranking.transform.Find("Text(RANKING)").gameObject.SetActive(false);
        ranking.transform.Find("Table").gameObject.SetActive(false);
        escapebutton.transform.Find("Text(ESCAPE)").gameObject.SetActive(false);

        text = transform.Find("Text(MYSCORE)").gameObject.GetComponent<Text>();

        try
        {
            database =  GameObject.Find("Database").GetComponent<Database>(); 
               
            my_score = database.defeated_enemies;
            text.text = tokansuji.to_kansuji(my_score, "〇") + "人切り";
            database.score_list.Add(my_score);
            database.score_list.Sort((a, b) => b - a);

            score_ranking = database.score_list;
        }

        catch (System.NullReferenceException)
        {
            gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            ranking.transform.Find("Text(RANKING)").gameObject.SetActive(true);
            ranking.transform.Find("Table").gameObject.SetActive(true);
            escapebutton.transform.Find("Text(ESCAPE)").gameObject.SetActive(true);  

            rankingtable.ranking_displayer();          
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void on_click()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(false);
        ranking.transform.Find("Text(RANKING)").gameObject.SetActive(true);
        ranking.transform.Find("Table").gameObject.SetActive(true);
        escapebutton.transform.Find("Text(ESCAPE)").gameObject.SetActive(true);

        rankingtable.ranking_displayer();   
    }
}
