using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    
    public TimingManager timingmanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //エラーがでるのでコメントアウト
        if (Input.GetKeyDown(KeyCode.W))
        {
            //timingmanager.getkey(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //timingmanager.getkey(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //timingmanager.getkey(3);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //timingmanager.getkey(4);
        }
        else
        {
            //timingmanager.getkey(0);
        }
    }
}
