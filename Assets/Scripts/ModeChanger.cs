using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeChanger : MonoBehaviour
{
    private RankingTable rankingtable;
    private MyScore myscore;

    private GameObject text1;
    private GameObject text2;

    private List<RaycastResult> rayresult;

    public void Start()
    {
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();


        text1 = transform.Find("Text1").gameObject;
        text2 = transform.Find("Text2").gameObject;

        rayresult = new List<RaycastResult>();

        try
        {
            myscore = GameObject.Find("MyScore").GetComponent<MyScore>();
        }
        catch (System.Exception)
        {
        }
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

            if (rankingtable.mode == 0)
            {
                text1.GetComponent<Text>().text = "タイムランキングへ";
            }

            else
            {
                text1.GetComponent<Text>().text = "スコアランキングへ";
            }
        }

        if (Input.GetKey(KeyCode.Tab) )
        {
            text1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            text1.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);

            text2.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            text2.GetComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            text2.transform.Find("BorderLine").gameObject.GetComponent<Image>().color = new Color (1, 1, 1, 1);
            text2.transform.Find("BorderLine").gameObject.GetComponent<Shadow>().effectColor = new Color (0, 0, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void on_click()
    {
        if (rankingtable.mode == 0)
        {
            rankingtable.mode = 1;
        }

        else
        {
            rankingtable.mode = 0;
        }

        try
        {
            GameObject panel = GameObject.Find("Panel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2 (1040, 644);
            panel.transform.localPosition = new Vector3 (0, 0, 0);

            foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject);
            }

            rankingtable.generate_ranking(myscore.player_name); 
        }

        catch (System.Exception)
        {
            rankingtable.generate_ranking(null);
        }
    }
}
