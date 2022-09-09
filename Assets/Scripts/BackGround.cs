using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Database database;

    public void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    public void FixedUpdate()
    {
        transform.Translate(new Vector2((-(16 / (60 / ((float)database.BPM / 4)))) * Time.deltaTime, 0));

        if (transform.position.x < -18)
        {
            Destroy(gameObject);
        }
    }
}
