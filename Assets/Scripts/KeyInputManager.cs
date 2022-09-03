using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //エラーがでるのでコメントアウト
        if (Input.GetKey(KeyCode.W))
        {
            //gameObject.GetComponent<TimingManager>().getkey(1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //gameObject.GetComponent<TimingManager>().getkey(2);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //gameObject.GetComponent<TimingManager>().getkey(3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //gameObject.GetComponent<TimingManager>().getkey(4);
        }
        else
        {
            //gameObject.GetComponent<TimingManager>().getkey(0);
        }
    }
}
