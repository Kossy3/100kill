using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BackStartScene : MonoBehaviour
{
    private Database database;
    private MyScore myscore;

    private GameObject text1;
    private GameObject text2;

    private List<RaycastResult> rayresult;

    public void Start()
    {
        if (GameObject.Find("Database"))
        {
            database = GameObject.Find("Database").GetComponent<Database>();
        }
        
        if (GameObject.Find("MyScore"))
        {
            myscore = GameObject.Find("MyScore").GetComponent<MyScore>();
        }

        text1 = transform.Find("Text1").gameObject;
        text2 = transform.Find("Text2").gameObject;

        rayresult = new List<RaycastResult>();
    }

    public void Update()
    {
        text1.GetComponent<Text>().color = new Color (0, 0, 0, 1);
        text1.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);

        text2.GetComponent<Text>().color = new Color (0, 0, 0, 1);
        text2.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);
        text2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color (0, 0, 0, 1);
        text2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color (1, 1, 1, 1);

        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        foreach (var raycastresult in rayresult)
        {
            if (raycastresult.gameObject.CompareTag("Button"))
            {
                GameObject text1 = raycastresult.gameObject.transform.Find("Text1").gameObject;
                text1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                text1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);

                GameObject text2 = raycastresult.gameObject.transform.Find("Text2").gameObject;
                text2.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                text2.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
                text2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color (1, 1, 1, 1);
                text2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
            }
        }

        if (Input.GetKey(KeyCode.Escape) )
        {
            text1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            text1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);

            text2.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            text2.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            text2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color (1, 1, 1, 1);
            text2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void on_click()
    {
        if (GameObject.Find("Database") && GameObject.Find("MyScore") && myscore.mode == 1)
        {
            database.score_list.RemoveAt(database.score_list.Count - 1);
        }

        SceneManager.LoadScene("Start");
    }
}
