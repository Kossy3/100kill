using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMaku : MonoBehaviour
{
    public RectTransform finishmaku;
    private float _move = 350f;
    // Start is called before the first frame update
    void Start()
    {
        finishmaku.position = new Vector2(640, _move);
    }

    // Update is called once per frame
    void Update()
    {
        _move += 300f * Time.deltaTime;
        finishmaku.position = new Vector2(640, _move);
        if (_move > 1200f)
        {
            Destroy(gameObject);
        }
    }
}
