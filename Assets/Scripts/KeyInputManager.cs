using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    private AudioSource audios;
    public TimingManager timingmanager;
    private Database database;
    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        audios = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                timingmanager.getkey(1);
                audios.Play();
            }
            else if (Input.GetKeyDown(KeyCode.A) && database.skill_gauge == 4)
            {
                timingmanager.getkey(2);
                audios.Play();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                timingmanager.getkey(3);
                audios.Play();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                timingmanager.getkey(4);
                audios.Play();

            }
        }
    }
}
