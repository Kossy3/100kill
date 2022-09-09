using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatCount : MonoBehaviour
{
    private Database database;
    private ToKansuji tokansuji; 

    private Text text;

    public void Start()
    {
        tokansuji = GameObject.Find("ToKansuji").GetComponent<ToKansuji>();
        database = GameObject.Find("Database").GetComponent<Database>();

        text = GetComponent<Text>();
    }

    public void FixedUpdate()
    {
        text.text = tokansuji.to_kansuji(database.defeated_enemies, "〇") + "人切り";
    }
}
