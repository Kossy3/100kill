using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StopMenu : MonoBehaviour
{
    private CountDown countdown;
    private EventSystem eventsystem;
    private Database database;

    private Image image;

    private List<GameObject> button_list;
    private GameObject select_button;

    private GameObject button_stopmenu;

    private List<RaycastResult> rayresult;

    void Start()
    {
        countdown = GameObject.Find("CountDown").GetComponent<CountDown>();
        eventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        database = GameObject.Find("Database").GetComponent<Database>();

        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        image.enabled = false;

        button_list = new List<GameObject>();
        select_button = null;

        rayresult = new List<RaycastResult>();

        button_stopmenu = GameObject.Find("Button(STOPMENU)");
        button_list.Add(button_stopmenu);

        for(int i = 0; i < 3; i++)
        {
            button_list.Add(gameObject.transform.GetChild(i).gameObject);
            button_list[i + 1].SetActive(false);
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

                if (Time.timeScale == 0 && select_button == button_list[0])
                {
                    select_button = null;
                }
            }
        }
        
        if (rayresult.Count == 0)
        {
            select_button = null;
        }

        if (Time.timeScale == 1)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                select_button = button_list[0];
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                select_button.GetComponent<Button>().onClick.Invoke();
                select_button = null;
            }
        }

        else if (Time.timeScale == 0)
        {
            if (select_button == null)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    select_button = button_list[1];
                }

                else if (Input.GetKeyDown(KeyCode.S))
                {
                    select_button = button_list[2];
                }

                else if (Input.GetKeyDown(KeyCode.D))
                {
                    select_button = button_list[3];
                }
            }

            else if (select_button == button_list[1] && Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[2];
            }

            else if (select_button == button_list[2] && Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[1];
            }

            else if (select_button == button_list[2] && Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[3];
            }

            else if (select_button == button_list[3] && Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[2];
            }

            if (select_button && Input.GetKeyDown(KeyCode.Space))
            {
                select_button.GetComponent<Button>().onClick.Invoke();
            }
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject all_text = button_list[i].transform.Find("Text").gameObject;
            all_text.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            all_text.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 1);

            if (i == 0)
            {
                button_list[i].transform.Find("Text2").gameObject.GetComponent<Text>().color = new Color (0, 0, 0, 1);
                button_list[i].transform.Find("Text2").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
                button_list[i].transform.Find("Text2").gameObject.transform.Find("BorderLine").gameObject.GetComponent<Image>().color =
                new Color (0, 0, 0, 1);
                button_list[i].transform.Find("Text2").gameObject.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor =
                new Color (1, 1, 1, 1);
            }
        }

        if (select_button)
        {
            GameObject text_1 = select_button.transform.Find("Text").gameObject;
            text_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            text_1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);

            if (select_button == button_list[0])
            {
                select_button.transform.Find("Text2").gameObject.GetComponent<Text>().color = new Color (1, 1, 1, 1);
                select_button.transform.Find("Text2").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
                select_button.transform.Find("Text2").gameObject.transform.Find("BorderLine").gameObject.GetComponent<Image>().color =
                new Color (1, 1, 1, 1);
                select_button.transform.Find("Text2").gameObject.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor =
                new Color (0, 0, 0, 1);
            }
        }
    }

    public void on_click_continue()
    {
        countdown.StartCoroutine("count_down");

        select_button = null;

        image.enabled = false;

        for (int i = 0; i < 3; i++)
        {
            button_list[i + 1].SetActive(false);
        }
    }

    public void on_click_startover()
    {
        Destroy(GameObject.Find("Database"));
        SceneManager.LoadScene("Main");

    }

    public void on_click_giveup()
    {
        Destroy(GameObject.Find("Database"));
        SceneManager.LoadScene("Start");
    }

    public void on_click_stopmenu()
    {
        Time.timeScale = 0;
        image.enabled = true;
            
        for (int i = 0; i < 3; i++)
        {
            button_list[i + 1].SetActive(true);
        }
    }
}