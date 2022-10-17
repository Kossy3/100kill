using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skills = new GameObject[5];
    private float skill_gauge;
    public Database database;
    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    // Update is called once per frame
    void Update()
    {
        skill_gauge = database.skill_gauge;

        foreach (GameObject skill in skills)
        {
            skill.SetActive(false);
        }

        if (skill_gauge == 0)
        {
            skills[0].SetActive(true);
        }

        else if (skill_gauge == 1)
        {
            skills[1].SetActive(true);
        }

        else if (skill_gauge == 2)
        {
            skills[2].SetActive(true);
        }

        else if (skill_gauge == 3)
        {
            skills[3].SetActive(true);
        }

        else if (skill_gauge == 4)
        {
            skills[4].SetActive(true);
        }
    }
}
