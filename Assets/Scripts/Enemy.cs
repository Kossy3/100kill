using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    public void Start()
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
