using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class Online : MonoBehaviour
{
    private const string URL = "http://plsk.net/100kill";
    public string textResult;
    public Database database;
    public string defeates;
    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        defeates = database.defeated_enemies.ToString();
        StartCoroutine(GetText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetText()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            textResult = request.downloadHandler.text;
            byte[] results = request.downloadHandler.data;

        }
    }
}