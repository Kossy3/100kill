using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BackStartScene : MonoBehaviour
{
    private EventSystem eventsystem;

    private GameObject text;

    private List<RaycastResult> rayresult;

    public void Start()
    {
        text = transform.Find("Text(ESCAPE)").gameObject;

        rayresult = new List<RaycastResult>();
    }

    public void Update()
    {
        transform.Find("Text(ESCAPE)").gameObject.GetComponent<Text>().color = new Color(0, 0, 0, 1);
        transform.Find("Text(ESCAPE)").gameObject.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 1);

        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        foreach (var raycastresult in rayresult)
        {
            if (raycastresult.gameObject.CompareTag("Button"))
            {
                GameObject text = raycastresult.gameObject.transform.Find("Text(ESCAPE)").gameObject;
                text.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
                text.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            }
        }

        if (Input.GetKey(KeyCode.Escape) )
        {
            text.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
            text.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void on_click()
    {
        SceneManager.LoadScene("Start");
    }
}
