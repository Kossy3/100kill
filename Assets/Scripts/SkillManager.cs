using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skill = new GameObject[2];
    private float skill_gauge;
    public Database database;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        skill_gauge = database.skill_gauge;
        if (skill_gauge == 0)
        {
            skill[0].gameObject.SetActive(true);
            skill[1].gameObject.SetActive(false);
        }
        if (skill_gauge == 1)
        {
            skill[0].gameObject.SetActive(false);
            skill[1].gameObject.SetActive(true);
        }
    }
}
