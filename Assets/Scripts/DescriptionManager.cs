using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    public List<Animator> animator = new List<Animator>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator[0].Play("slash_discription");
        animator[1].Play("skill_discription");
        animator[2].Play("jump_discription");
        animator[3].Play("sliding_discription");

        animator[4].Play("enemy_discription");
        animator[5].Play("enemy_discription1");
        animator[6].Play("enemy_discription2");
        animator[7].Play("enemy_discription3");
        animator[8].Play("katana");
    }

}

