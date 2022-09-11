using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {

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
