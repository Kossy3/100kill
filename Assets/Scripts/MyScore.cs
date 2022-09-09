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

    public int my_score;

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
            bool[] scene_number_identifier = new bool[1];

            if (scene_number_identifier[database.scene_number])
            {
            }

            my_score = database.defeated_enemies;
            text.text = tokansuji.to_kansuji(my_score, "〇") + "人切り";
        }

        catch (System.Exception)
        {
            gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            ranking.transform.Find("Text(RANKING)").gameObject.SetActive(true);
            ranking.transform.Find("Table").gameObject.SetActive(true);
            escapebutton.transform.Find("Text(ESCAPE)").gameObject.SetActive(true);  
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
    }
}
