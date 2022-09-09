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

        for (int i = 0; i < 2; i++)
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

        if (select_button == null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                select_button = button_list[0];
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                select_button = button_list[0];
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                select_button = button_list[1];
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                select_button = button_list[1];
            }
        }

        else if (select_button == button_list[0] && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)))
        {
            select_button = button_list[1];
        }

        else if (select_button == button_list[1] && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W)))
        {
            select_button = button_list[0];
        }

        if(Input.GetKey(KeyCode.Space))
        {
            select_button = button_list[0];
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            button_list[0].gameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            select_button = button_list[1];
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            button_list[1].gameObject.GetComponent<Button>().onClick.Invoke();
        }
        
        for (int i = 0; i < 2; i++)
        {
            GameObject all_text = button_list[i].transform.Find("Text").gameObject;
            all_text.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            all_text.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 0.5f);
        }
        
        if (select_button)
        {
            GameObject text_1 = select_button.transform.Find("Text").gameObject;
            text_1.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
            text_1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
        }
    }

    public void on_click_start()
    {
        try
        {
            GameObject.Find("Database").GetComponent<Database>().reset();
        }

        catch (System.NullReferenceException)
        {
        }

        SceneManager.LoadScene("Main");
    }

    public void on_click_escape()
    {
        SceneManager.LoadScene("Score");
    }
}