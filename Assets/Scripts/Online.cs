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
    public List<Dictionary<string, List<int>>> scores_list;

    public string scores_str;

    private const string URL = "http://plsk.net/100kill";

    void Start()
    {
        scores_list = new List<Dictionary<string, List<int>>>();
    }

    private IEnumerator GetText()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
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
    }
}