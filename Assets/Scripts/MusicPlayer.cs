using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioSynthesis.Synthesis;

public class MusicPlayer : MonoBehaviour
{
    Synthesizer synth;
    // Start is called before the first frame update
    void Start()
    {
        synth = new Synthesizer(8000, 1).NoteOn((System.Int32)9, (System.Int32)70, (System.Int32)100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
