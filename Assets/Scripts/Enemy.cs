using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    public void start()
    {
        anim = GetComponent<Animator>();
    }
    public void good()
    {
        anim.SetTrigger("enemy_yarare");
    }

    public void miss()
    {
        
    }
}
