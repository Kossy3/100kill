using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioSynthesis.Synthesis;
using System;
using System.Runtime.InteropServices;

public class MusicPlayer : MonoBehaviour
{
    Synthesizer synth;

    IntPtr hMidi;
    bool flag = true;
    void Start()
    {
        midiOutOpen(out hMidi, -1, null, 0, CALLBACK.NULL).ToString();
        midiOutShortMsg(hMidi, 0x007f3c90);
        Debug.Log("on");
    }

    [DllImport("Winmm.dll")]
    public static extern uint midiOutOpen(
        out IntPtr lphmo,
        int uDeviceID,
        MidiOutProc dwCallback,
        int dwCallbackInstance,
        CALLBACK dwFlags
    );

    public enum CALLBACK
    {
        EVENT = 0x50000,
        FUNCTION = 0x30000,
        NULL = 0x0,
        THREAD = 0x20000,
        WINDOW = 0x10000,
    }

    public delegate void MidiOutProc(
        IntPtr hmo,
        uint hwnd,
        int dwInstance,
        int dwParam1,
        int dwParam2
    );

    [DllImport("Winmm.dll")]
    public static extern uint midiOutShortMsg(IntPtr hmo, int dwMsg);


    // Start is called before the first frame update
    void start()
    {
        synth = new Synthesizer(8000, 1);
        synth.NoteOn((System.Int32)9, (System.Int32)70, (System.Int32)100);
    }

    // Update is called once per frame
    void Update()
    {
        if (flag && Time.time >1){
            Debug.Log("off");
            midiOutShortMsg(hMidi, 0x007f3c80);
            flag = false;
        }
    }
}


