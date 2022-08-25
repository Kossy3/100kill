using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    Animator anim;
    public void start()
    {
        anim = GetComponent<Animator>();
    }

    public void jump()
    {
        anim.SetTrigger("jump");
    }

    public void sliding()
    {
        anim.SetTrigger("sliding");
    }

    public void slash()
    {
        anim.SetTrigger("slash");
    }

    public void skill()
    {
        anim.SetTrigger("skill");
    }
}
