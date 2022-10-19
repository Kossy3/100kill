using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class Online : MonoBehaviour
{
    public RankingTable rankingtable;

    public string scores_str;

    void Start()
    {
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();

        StartCoroutine(GetText());
    }

    public IEnumerator GetText()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://plsk.net/100kill");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }

        else
        {
            string textResult = request.downloadHandler.text;

            Match match = Regex.Match(textResult, "id=\"message\" >(.*)<\\/textarea>");
            scores_str = match.Groups[1].Value;
            Debug.Log(match.Groups[1]);
        }

        if (GameObject.Find("Database") == null)
        {
            rankingtable.generate_ranking(null, null, false);
        }
    }

    public IEnumerator SendText()
    {
        string URL = "http://plsk.net/edit.php?id=100kill&txt=" + scores_str;

        UnityWebRequest request = UnityWebRequest.Get(URL);

        yield return request.SendWebRequest();
     
        if (request.isNetworkError || request.isHttpError) 
        {
            Debug.Log(request.error);
        }

        else 
        {
            Debug.Log("Get upload complete!");
        }
    }
}