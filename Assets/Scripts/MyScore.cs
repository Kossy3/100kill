using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyScore : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji;
    private RankingTable rankingtable;

    private GameObject ranking;
    private GameObject escapebutton;
    private GameObject modechanger;
    private GameObject inputname;

    private Image image;
    private Text text_myscore;
    private Text text_myscoretime;

    private GameObject button;

    private GameObject select_button;

    private List<RaycastResult> rayresult;

    private int my_score;

    private float my_scoretime;

    public string player_name;

    void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();

        ranking = GameObject.Find("Ranking");
        escapebutton = GameObject.Find("EscapeButton");
        modechanger = GameObject.Find("ModeChanger");
        inputname = GameObject.Find("InputName");

        button = inputname.transform.Find("Button").gameObject;

        select_button = null;

        rayresult = new List<RaycastResult>();

        gameObject.SetActive(true);
        ranking.SetActive(false);
        escapebutton.SetActive(false);
        modechanger.SetActive(false);
        inputname.SetActive(false);

        text_myscore = transform.Find("Text(MYSCORE)").gameObject.GetComponent<Text>();
        text_myscoretime = transform.Find("Text(MYSCORETIME)").gameObject.GetComponent<Text>();

        try
        {
            database =  GameObject.Find("Database").GetComponent<Database>(); 
            bool[] scene_number_identifier = new bool[1];

            if (scene_number_identifier[database.scene_number])
            {                
            }

            my_score = database.defeated_enemies;

            text_myscore.text = tokansuji.to_kansuji(my_score, "〇") + "人切り";

            int minutes = 0;

            my_scoretime = database.playing_time;

            if (my_scoretime >= 60f)
            {
                minutes ++;
                my_scoretime -= 60f;
            }

            string time_str;

            if (minutes == 0)
            {
                time_str = tokansuji.to_kansuji((int)my_scoretime, "-") + "秒";
            }

            else
            {
                time_str = tokansuji.to_kansuji(minutes, "-") + "分" + tokansuji.to_kansuji((int)my_scoretime, "-") + "秒";
            }

            text_myscoretime.text = time_str;
        }

        catch (System.Exception)
        {
            gameObject.SetActive(false);
            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
        }
    }

    void Update()
    {
        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        foreach (var raycastresult in rayresult)
        {
            if (raycastresult.gameObject.CompareTag("Button"))
            {
                select_button = raycastresult.gameObject;
                break;
            }

            else
            {
                select_button = null;
            }
        }

        if (select_button == null)
        {
            button.GetComponent<Image>().color = new Color (0, 0, 0, 1);
            button.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
            button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (0, 0, 0, 1);
            button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);

            if (Input.anyKeyDown)
            {
                select_button = button;
            }
        }

        if (select_button)
        {
            button.GetComponent<Image>().color = new Color (1, 1, 1, 1);
            button.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
            button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (1, 1, 1, 1);
            button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);

            if (Input.GetKey(KeyCode.Space))
            {
                select_button.GetComponent<Button>().onClick.Invoke();
            }
        }

        if (Input.anyKeyDown)
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    private int index = 0;

    public void on_click_score()
    {
        if (index == 0)
        {
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.transform.Find("Text(MYSCORETIME)").gameObject.SetActive(false);
            gameObject.transform.Find("Text(MYSCORE)").gameObject.SetActive(false);
            inputname.SetActive(true);
            inputname.transform.Find("Text").gameObject.SetActive(false);
        }

        index ++;
    }

    public void On_click_mane()
    {
        try
        {
            player_name = GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text;

            List<int> my_scores = new List<int> {database.defeated_enemies, (int)database.playing_time};
            database.score_list.Add(player_name, my_scores);

            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
            inputname.SetActive(false);

            rankingtable.generate_ranking();
        }
        catch (System.Exception)
        {
            inputname.transform.Find("Text").gameObject.SetActive(true);
        }
    }
}