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

        gameObject.SetActive(true);
        ranking.SetActive(false);
        escapebutton.SetActive(false);

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
            ranking.SetActive(true);
            escapebutton.SetActive(true);  
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
        ranking.SetActive(true);
        escapebutton.SetActive(true); 
    }
}
