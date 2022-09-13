using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneNavigator : MonoBehaviour
{
    private List<GameObject> button_list;
    private GameObject select_button;

    private List<RaycastResult> rayresult;

    void Start()
    {
        button_list = new List<GameObject>();
        select_button = null;

        rayresult = new List<RaycastResult>();

        for (int i = 0; i < 4; i++)
        {
            button_list.Add(transform.GetChild(i).gameObject);
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
            }
        }

        if (rayresult.Count == 0)
        {
            select_button = null;
        }

        if (select_button == null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[0];
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                select_button = button_list[1];
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[2];
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                select_button = button_list[3];
            }
        }

        else if (select_button == button_list[0] && Input.GetKeyDown(KeyCode.W))
        {
            select_button = button_list[3];
        }   

        else if (select_button == button_list[0] && Input.GetKeyDown(KeyCode.S))
        {
            select_button = button_list[1];
        }

        else if (select_button == button_list[0] &&
        (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            select_button = button_list[2];
        }

        else if (select_button == button_list[1] && Input.GetKeyDown(KeyCode.W))
        {
            select_button = button_list[0];
        }

        else if (select_button == button_list[1] && Input.GetKeyDown(KeyCode.S))
        {
            select_button = button_list[2];
        }

        else if (select_button == button_list[2] && Input.GetKeyDown(KeyCode.W))
        {
            select_button = button_list[1];
        }

        else if (select_button == button_list[2] && Input.GetKeyDown(KeyCode.S))
        {
            select_button = button_list[3];
        }

        else if (select_button == button_list[3] && Input.GetKeyDown(KeyCode.W))
        {
            select_button = button_list[2];
        }

        else if (select_button == button_list[3] && Input.GetKeyDown(KeyCode.S))
        {
            select_button = button_list[0];
        }

        else if ((select_button == button_list[1] || select_button == button_list[2] || select_button == button_list[3]) &&
        (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            select_button = button_list[0];
        }
        
        if (select_button && Input.GetKeyDown(KeyCode.Space))
        {
            select_button.gameObject.GetComponent<Button>().onClick.Invoke();
        }

        else if(Input.GetKey(KeyCode.Space))
        {
            select_button = button_list[0];
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            button_list[0].gameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKey(KeyCode.AltGr) || Input.GetKey(KeyCode.LeftAlt))
        {
            select_button = button_list[1];
        }

        if (Input.GetKeyUp(KeyCode.AltGr) || Input.GetKeyUp(KeyCode.LeftAlt))
        {
            button_list[1].gameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            select_button = button_list[2];
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            button_list[2].gameObject.GetComponent<Button>().onClick.Invoke();
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {
            select_button = button_list[3];
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            button_list[3].gameObject.GetComponent<Button>().onClick.Invoke();
        }
        
        for (int i = 0; i < 4; i++)
        {
            GameObject all_text_1 = button_list[i].transform.Find("Text1").gameObject;
            all_text_1.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            all_text_1.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 0.5f);

            GameObject all_text_2 = button_list[i].transform.Find("Text2").gameObject;
            all_text_2.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            all_text_2.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 0.5f);
            all_text_2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            all_text_2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 0.5f);
        }

        if (select_button)
        {
            GameObject text_1 = select_button.transform.Find("Text1").gameObject;
            text_1.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
            text_1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);

            GameObject text_2 = select_button.transform.Find("Text2").gameObject;
            text_2.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
            text_2.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            text_2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            text_2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
        }
    }

    public void on_click_space()
    {
        try
        {
            GameObject.Find("Database").GetComponent<Database>().reset();
        }

        catch (System.NullReferenceException)
        {
        }

        //Destroy(GameObject.Find("Database"));
        SceneManager.LoadScene("Main");
    }

    public void on_click_alt()
    {
        SceneManager.LoadScene("Method");
    }

    public void on_click_Tab()
    {
        //Destroy(GameObject.Find("Database"));
        try
        {
            GameObject.Find("Database").GetComponent<Database>().scene_number = 1;
        }

        catch (System.NullReferenceException)
        {
        }

        SceneManager.LoadScene("Score");
    }

    public void on_click_escape()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}