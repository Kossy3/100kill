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
        animator[1].Play("slash_discription");
        animator[2].Play("jump_discription");
        animator[3].Play("sliding_discription");

        animator[4].Play("enemy_discription");
        animator[5].Play("enemy_discription1");
        animator[6].Play("enemy_discription2");
        animator[7].Play("enemy_discription3");
        animator[8].Play("katana");
        animator[9].Play("use_katana");
        animator[10].Play("enemy_discription2");
        animator[11].Play("enemy_discription1");
        animator[12].Play("enemy_discription3");
        animator[13].Play("enemy_discription");
        animator[14].Play("enemy_discription");
        animator[15].Play("enemy_discription2");
        animator[16].Play("upthing_discription");
        animator[17].Play("stone_discription");
        animator[18].Play("slash_discription");
    }

}

