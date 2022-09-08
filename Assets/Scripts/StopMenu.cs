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

    private Image image;

    public List<GameObject> button_list;
    public GameObject select_button;

    private List<RaycastResult> rayresult = new List<RaycastResult>();

    void Start()
    {
        countdown = GameObject.Find("CountDown").GetComponent<CountDown>();
        eventsystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        image.enabled = false;

        button_list = new List<GameObject>();
        select_button = null;

        for (int i = 0; i < 3; i++)
        {
            button_list.Add(gameObject.transform.GetChild(i).gameObject);
            button_list[i].SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            image.enabled = true;
            
            for (int i = 0; i < 3; i++)
            {
                button_list[i].SetActive(true);
            }
        }

        if (Time.timeScale == 0)
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
                }
            }


            if (select_button == null)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    select_button = button_list[0];
                }

                else if (Input.GetKeyDown(KeyCode.S))
                {
                    select_button = button_list[1];
                }

                else if (Input.GetKeyDown(KeyCode.D))
                {
                    select_button = button_list[2];
                }
            }

            else if (select_button == button_list[0] && Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[1];
            }

            else if (select_button == button_list[1] && Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[0];
            }

            else if (select_button == button_list[1] && Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[2];
            }

            else if (select_button == button_list[2] && Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[1];
            }

            if (select_button)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject all_text = button_list[i].transform.Find("Text").gameObject;
                    all_text.GetComponent<Text>().color = new Color(0, 0, 0, 1);
                    all_text.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 1);
                }

                GameObject text_1 = select_button.transform.Find("Text").gameObject;
                text_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                text_1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            }

            if (select_button && Input.GetKeyDown(KeyCode.Space))
            {
                select_button.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void On_click_continue()
    {
        countdown.StartCoroutine("count_down");

        select_button = null;

        image.enabled = false;

        for (int i = 0; i < 3; i++)
        {
            button_list[i].SetActive(false);
        }
    }

    public void On_click_startover()
    {
        SceneManager.LoadScene("Main");
    }

    public void On_click_giveup()
    {
        SceneManager.LoadScene("Start");
    }
}
