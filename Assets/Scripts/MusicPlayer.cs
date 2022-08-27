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
    void start()
    {
        Debug.Log(midiOutOpen(out hMidi, 1, null, 0, 0));
        Debug.Log(hMidi);
        midiOutMsgFixed(hMidi, 0x9, 0, 0x45, 40);
        Debug.Log("on");
    }

    void update()
    {
        if (flag && Time.time > 1){
            Debug.Log("off");
            midiOutMsgFixed(hMidi, 0x9, 0, 0x45, 0);
            flag = false;
            midiOutReset(hMidi);
            midiOutClose(hMidi);
        }
    }

    [DllImport("Winmm.dll", EntryPoint="midiOutOpen")]
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

    [DllImport("winmm.dll")]
    private static extern int midiOutShortMsg(IntPtr handle, int message);
    public int midiOutMsgFixed(IntPtr hmo, byte status, byte channel, byte data1, byte data2)
    {
        return midiOutShortMsg(hmo, (status << 4) | channel | (data1 << 8) | (data2 << 16));
    }

    [DllImport("Winmm.dll")]
    public static extern uint midiOutReset(IntPtr hmo);
    [DllImport("Winmm.dll")]
    public static extern uint midiOutClose(IntPtr hmo);


    // Start is called before the first frame updat

    // Update is called once per frame
}


