using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour
{
    private EventSystem eventsystem;

    private List<RaycastResult> rayresult;

    void Start()
    {
        rayresult = new List<RaycastResult>();
    }

    void Update()
    {
        transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0, 0, 0, 1);
        transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 1);

        rayresult.Clear();

        var currentPointData = new PointerEventData(EventSystem.current);
        currentPointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(currentPointData, rayresult);

        foreach (var raycastresult in rayresult)
        {
            if (raycastresult.gameObject.CompareTag("Button"))
            {
                GameObject text = raycastresult.gameObject.transform.Find("Text").gameObject;
                text.GetComponent<Text>().color = new Color(1, 1, 1, 0.5f);
                text.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
            transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            transform.Find("Text").gameObject.GetComponent<Shadow>().effectColor = new Color(1, 1, 1, 1);
        }
    }

    public void On_Click()
    {
        Destroy(GameObject.Find("Database"));
        SceneManager.LoadScene("Main");
    }
}