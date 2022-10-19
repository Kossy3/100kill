using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WarningBoard : MonoBehaviour
{
    private Database database;
    private MyScore myscore;
    private RankingTable rankingtable;
    private Online online;

    private GameObject ranking;
    private GameObject escapebutton;
    private GameObject modechanger;
    private GameObject inputname;

    private Text text;

    private GameObject[] button_list;
    private GameObject select_button;

    private List<RaycastResult> rayresult;

    public void Start()
    {
        myscore = GameObject.Find("MyScore").GetComponent<MyScore>();
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();
        online = GameObject.Find("Online").GetComponent<Online>();

        ranking = GameObject.Find("Ranking");
        escapebutton = GameObject.Find("EscapeButton");
        modechanger = GameObject.Find("ModeChanger");
        inputname = GameObject.Find("InputName");

        text = transform.Find("Text").GetComponent<Text>();
        
        rayresult = new List<RaycastResult>();

        try
        {
            database = GameObject.Find("Database").GetComponent<Database>();
            
            button_list = new GameObject[] {transform.Find("Button(YES)").gameObject, transform.Find("Button(NO)").gameObject};
        }

        catch (System.Exception)
        {
        }
    }

    public void Update()
    {
        if (myscore.mode == 0)
        {
            transform.Find("Text").GetComponent<Text>().text = 
            "その名前はすでに使われています。\n同じ名前を使用しますか？";
        }

        else
        {
            transform.Find("Text").GetComponent<Text>().text =
            "未記入のため記録は残りません。\nよろしいですか？";
        }

        GameObject input_button = GameObject.Find("InputName").transform.Find("Button").gameObject;
        input_button.GetComponent<Image>().color = new Color (0, 0, 0, 1);
        input_button.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
        input_button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (0, 0, 0, 1);
        input_button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);

        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        if (rayresult.Count > 1)
        {
            if (rayresult[1].gameObject.CompareTag("Button"))
            {
                select_button = rayresult[1].gameObject;
            }

            else
            {
                select_button = null;
            }
        }



        if (select_button == null)
        {
            foreach (GameObject button in button_list)
            {
                button.GetComponent<Image>().color = new Color (0, 0, 0, 1);
                button.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
                button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (0, 0, 0, 1);
                button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
            }
        }

        else
        {
            select_button.GetComponent<Image>().color = new Color (1, 1, 1, 1);
            select_button.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
            select_button.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color (1, 1, 1, 1);
            select_button.transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
        }            
        
        if (Input.GetKey(KeyCode.Y))
        {
            select_button = button_list[0];
        }

        else if (Input.GetKey(KeyCode.N))
        {
            select_button = button_list[1];
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            button_list[0].GetComponent<Button>().onClick.Invoke();
        }

        else if (Input.GetKeyUp(KeyCode.N))
        {
            button_list[1].GetComponent<Button>().onClick.Invoke();
        }
    }

    public void on_click_ture()
    {
        if (myscore.mode == 0)
        {
            myscore.player_name = GameObject.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text;

            int my_time = (int)database.playing_time;
            int my_score = database.defeated_enemies;
            myscore.my_scores = new int[] {my_score, my_time};

            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
            inputname.SetActive(false);

            rankingtable.generate_ranking(myscore.player_name, myscore.my_scores, true);
        }

        else
        {
            myscore.player_name = "guestplay";

            int my_score = database.defeated_enemies;
            int my_time = (int)database.playing_time;
            myscore.my_scores = new int[] {my_score, my_time};

            ranking.SetActive(true);
            escapebutton.SetActive(true);
            modechanger.SetActive(true);
            inputname.SetActive(false);

            StartCoroutine(online.SendText(myscore.player_name, myscore.my_scores));
        }
    }

    public void on_click_false()
    {
        GameObject.Find("InputField").GetComponent<InputField>().Select();
        gameObject.SetActive(false);
    }
}
