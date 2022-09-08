using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Defeats : MonoBehaviour
{
    private Database database;

    public Image[] kabuto_list = new Image[4];

    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    void Update()
    {
    }
}
