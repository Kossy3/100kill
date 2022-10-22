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
    private RankingTable rankingtable;
    private MyScore myscore;

    public string scores_str;

    void Start()
    {
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();
        myscore = GameObject.Find("MyScore").GetComponent<MyScore>();

        StartCoroutine("GetText");

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

            foreach (string lists in scores_str.Split(':'))
            {
                string[] list = lists.Split(';');

                myscore.name_list.Add(Encoding.GetEncoding("UTF-8").GetString(Convert.FromBase64String(list[0])));
            }

            if (GameObject.Find("Database") == null)
            {
                rankingtable.generate_ranking(null, null);
            }
        }
    }

    public IEnumerator SendText(string player_name, int[] my_scores)
    {
        UnityWebRequest get_request = UnityWebRequest.Get("http://plsk.net/100kill");
        yield return get_request.SendWebRequest();

        if (get_request.isNetworkError || get_request.isHttpError)
        {
            Debug.Log(get_request.error);
        }

        else
        {
            string textResult = get_request.downloadHandler.text;

            Match match = Regex.Match(textResult, "id=\"message\" >(.*)<\\/textarea>");
            scores_str = match.Groups[1].Value;

            if (player_name != "guestplay")
            {
                scores_str = scores_str + ":" + 
                Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes(player_name)) + ";" +
                my_scores[0].ToString() + ";" + my_scores[1].ToString();
            }

            scores_str.Replace("+", "%2B");

            UnityWebRequest send_request = UnityWebRequest.Get("http://plsk.net/edit.php?id=100kill&txt=" + scores_str);

            yield return send_request.SendWebRequest();
     
            if (send_request.isNetworkError || send_request.isHttpError) 
            {
                Debug.Log(send_request.error);
            }

            else 
            {
                Debug.Log("Get upload complete!");

                rankingtable.generate_ranking(player_name, my_scores);
            }
        }
    }
}