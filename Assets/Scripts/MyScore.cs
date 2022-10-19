using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;

public class MyScore : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji;
    private RankingTable rankingtable;
    private Online online;

    private GameObject scoreUI;
    private GameObject ranking;
    private GameObject escapebutton;
    private GameObject modechanger;
    private GameObject inputname;
    private GameObject maku;
    private GameObject audioplayer;

    private Text text_myscore;
    private Text text_myscoretime;

    private GameObject button;

    private List<RaycastResult> rayresult;

    public string player_name;
    public int[] my_scores;
    public List<string> name_list;

    public int mode;

    void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();
        online = GameObject.Find("Online").GetComponent<Online>();

        scoreUI = GameObject.Find("ScoreUI");
        ranking = GameObject.Find("Ranking");
        escapebutton = GameObject.Find("EscapeButton");
        modechanger = GameObject.Find("ModeChanger");
        inputname = GameObject.Find("InputName");
        maku = GameObject.Find("maku");
        audioplayer = GameObject.Find("AudioPlayer");

        button = inputname.transform.Find("Button").gameObject;

        name_list = new List<string>();

        rayresult = new List<RaycastResult>();

        gameObject.SetActive(true);
        maku.SetActive(true);
        audioplayer.SetActive(true);
        ranking.SetActive(false);
        escapebutton.SetActive(false);
        modechanger.SetActive(false);
        inputname.SetActive(false);

        text_myscore = transform.Find("Text(MYSCORE)").gameObject.GetComponent<Text>();
        text_myscoretime = transform.Find("Text(MYSCORETIME)").gameObject.GetComponent<Text>();

        mode = 0;

        try
        {
            database =  GameObject.Find("Database").GetComponent<Database>(); 

            int my_score = database.defeated_enemies;

            text_myscore.text = tokansuji.to_kansuji(my_score, "〇") + "人切り";

            int minutes = 0;

            float my_scoretime = database.playing_time;

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
            maku.SetActive(false);
            audioplayer.SetActive(false);
            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
        }
    }

    void Update()
    {
        button.GetComponent<Image>().color = new Color (0, 0, 0, 1);
        button.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
        button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (0, 0, 0, 1);
        button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);

        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        if (rayresult.Count > 1)
        {
            if (rayresult[1].gameObject.CompareTag("Button"))
            {
                button.GetComponent<Image>().color = new Color (1, 1, 1, 1);
                button.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
                button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (1, 1, 1, 1);
                button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
            }
        }

        if (Input.GetKey(KeyCode.Return))
        {
            button.GetComponent<Image>().color = new Color (1, 1, 1, 1);
            button.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
            button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (1, 1, 1, 1);
            button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            button.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.anyKey)
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (GameObject.Find("InputField"))
        {
            if (indexof_containkey(name_list, GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text))
            {
                GameObject text = inputname.transform.Find("Text").gameObject;
                text.SetActive(true);
                text.GetComponent<Text>().text = "その名前はすでに使われています";
            }

            else if (GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "")
            {
                GameObject text = inputname.transform.Find("Text").gameObject;
                text.SetActive(true);
                text.GetComponent<Text>().text = "未記入で決定された場合、記録は残りません";
            }

            else
            {
                inputname.transform.Find("Text").gameObject.SetActive(false);
            }
        }
    }

    private bool indexof_containkey(List<string> name_list, string player_name)
    {
        bool overlap = false;

        foreach (string name in name_list)
        {
            if (player_name == name)
            {
                overlap = true;
                break;
            }
        }

        return overlap;
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
            inputname.transform.Find("WarningBoard").gameObject.SetActive(false);

            inputname.transform.Find("InputField").gameObject.GetComponent<InputField>().Select();
        }

        index ++;
    }

    public void on_click_name()
    {
        if (indexof_containkey(name_list, GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text))
        {
            inputname.transform.Find("WarningBoard").gameObject.SetActive(true);
            GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text = "";
            mode = 0;

            EventSystem.current.SetSelectedGameObject(null);
        }

        else if (GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "")
        {
            inputname.transform.Find("WarningBoard").gameObject.SetActive(true);
            mode = 1;

            EventSystem.current.SetSelectedGameObject(null);
        }

        else
        {
            int my_score = database.defeated_enemies;
            int my_time = (int)database.playing_time;
            my_scores = new int[] {my_score, my_time};

            player_name = GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text;

            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
            inputname.SetActive(false);

            StartCoroutine(online.SendText(player_name, my_scores));
            mode = 0;
        }
    }
}