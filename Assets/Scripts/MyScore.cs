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
    private GameObject maku;

    private Text text_myscore;
    private Text text_myscoretime;

    private GameObject button;

    private List<RaycastResult> rayresult;

    private int my_score;

    private float my_scoretime;

    public string player_name;
    public int mode;

    void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();

        ranking = GameObject.Find("Ranking");
        escapebutton = GameObject.Find("EscapeButton");
        modechanger = GameObject.Find("ModeChanger");
        inputname = GameObject.Find("InputName");
        maku = GameObject.Find("maku");

        button = inputname.transform.Find("Button").gameObject;

        rayresult = new List<RaycastResult>();

        gameObject.SetActive(true);
        maku.SetActive(true);
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
            maku.SetActive(false);
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
            if (indexof_containkey(database.score_list, GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text))
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

    private bool indexof_containkey(List<Dictionary<string, List<int>>> list, string dic_key)
    {
        bool overlap = false;

        foreach (Dictionary<string, List<int>> dic in list)
        {
            if (dic.ContainsKey(dic_key))
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
        if (indexof_containkey(database.score_list, GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text))
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
            player_name = GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text;

            List<int> scores_list = new List<int>() {database.defeated_enemies, (int)database.playing_time};
            Dictionary<string, List<int>> scores_dic = new Dictionary<string, List<int>>() {{player_name, scores_list}};
            database.score_list.Add(scores_dic);

            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
            inputname.SetActive(false);

            rankingtable.generate_ranking(player_name);
            mode = 0;
        }
    }
}