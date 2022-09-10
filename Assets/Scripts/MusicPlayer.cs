using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using System;

public class MusicPlayer : MonoBehaviour
{
    static string res;
    [SerializeField]
    private Database database;
    public const int MAXPNAMELEN = 32;
    public struct MidiOutCaps
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
        public string szPname;
        public short wTechnology;
        public short wVoices;
        public short wNotes;
        public short wChannelMask;
        public int dwSupport;
    }
 
    IntPtr handle;
    public bool KeyOn = false;
    public bool device_init = false;
 
 
    // MCI INterface
    [DllImport("winmm.dll")]
    private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, System.IntPtr winHandle);
 
    // Midi API
    [DllImport("winmm.dll")]
    private static extern int midiOutGetNumDevs();

    [DllImport("winmm.dll")]
    private static extern int  midiOutReset(IntPtr handle);
 
 
    [DllImport("winmm.dll")]
    private static extern int midiOutGetDevCaps(System.Int32 uDeviceID, ref MidiOutCaps lpMidiOutCaps, System.UInt32 cbMidiOutCaps);
 
    [DllImport("winmm.dll")]
    private static extern int midiOutOpen(out IntPtr handle, int deviceID, MidiCallBack proc, int instance, int flags);
 
    [DllImport("winmm.dll")]
    private static extern int midiOutShortMsg(IntPtr handle, int message);
 
    //[DllImport("winmm.dll")]
    public int midiOutMsgFixed(IntPtr hmo, byte status, byte channel, byte data1, byte data2)
    {
        return midiOutShortMsg(hmo, (status << 4) | channel | (data1 << 8) | (data2 << 16));
    }
    //public static uint midiOutShortMsg(int hmo, byte status, byte channel, GMProgram data1, byte data2) { return midiOutShortMsg(hmo, (status &lt;&lt; 4) | channel | ((byte)data1 &lt;&lt; 8) | (data2 &lt;&lt; 16)); }
 
    [DllImport("winmm.dll")]
    private static extern int  midiOutClose(IntPtr handle);
 
    private delegate void MidiCallBack(int handle, int msg, int instance, int param1, int param2);
 
    static string Mci(string command)
    {
        StringBuilder reply = new StringBuilder(256);
        mciSendString(command, reply, 256, System.IntPtr.Zero);
        return reply.ToString();
    }
 
    void Start()
    {
        var numDevs = midiOutGetNumDevs();
        //Debug.Log(numDevs);
        MidiOutCaps myCaps = new MidiOutCaps();
 
        //0番ポートの調査を行う。
        //var res = midiOutGetDevCaps(-1, ref myCaps, (System.UInt32)Marshal.SizeOf(myCaps));
        /*for (var i=0; i<numDevs; i++){
            var res = midiOutGetDevCaps(i, ref myCaps, (System.UInt32)Marshal.SizeOf(myCaps));
            Debug.Log(myCaps.szPname);
        }*/
        var res1 = midiOutGetDevCaps(-1, ref myCaps, (System.UInt32)Marshal.SizeOf(myCaps));
        //Debug.Log(myCaps.szPname);
        //引数１はポインタ扱い。
#if UNITY_EDITOR
        //Debug.Log(myCaps.szPname);
#endif
        DeviceInitialize();
        //StartCoroutine("play_music_c");
    }

    int [] test_score(){
        int[] score = new int[64];
        for (var i=0; i<64; i++){
            score[i] = UnityEngine.Random.Range(0, 2);
        }
        return score;
    }

    void play_note(int mode, byte ch, byte no, float delta, float ms,byte velocity){
        if (mode == 0x9){
            StartCoroutine( play_note_c(ch, no, delta, ms, velocity));
        } else if (mode == 0xC){
            StartCoroutine( program_change_c(ch, no, delta));
        }
    }

    IEnumerator play_note_c(byte ch, byte no, float delta, float ms,byte velocity){
        yield return new WaitForSeconds(delta);
        //Debug.Log((ch, noteno, delta, ms, velocity));
        midiOutMsgFixed(handle, 0x9, ch, no, velocity);
        yield return new WaitForSeconds(ms);
        midiOutMsgFixed(handle, 0x8, ch, no, velocity);
    }
    IEnumerator program_change_c(byte ch, byte no, float delta){
        yield return new WaitForSeconds(delta);
        //Debug.Log((ch, noteno, delta, ms, velocity));
        midiOutMsgFixed(handle, 0xC, ch, no, 0);
    }
    public IEnumerator play_music_c(List<List<Note>> score){
        yield return new WaitForSeconds(1f);
        play_music(score, database.BPM);
    }

    public void DeviceInitialize(){
        int midi_no=-1;
        if( device_init == false){
            var res = midiOutOpen( out handle, midi_no , null , 0 , 0);
            device_init = true;
#if UNITY_EDITOR
 
            //Debug.Log(handle);
            //Debug.Log(res);
#endif
        }
    }
 
    public void DeviceClose(){
        if( device_init == true){
            midiOutReset(handle);
            var res=midiOutClose(handle);
#if UNITY_EDITOR
            //Debug.Log(handle);
           // Debug.Log(res);
#endif
            device_init = false;
        }
    }

 
    void OnDestroy()
    {
        res = Mci("close music");
        DeviceClose();
    }
 
    void OnDisable()
    {
        res = Mci("close music");
    }

    public void play_music(List<List<Note>> score, float BPM){
        float C = 60f/(float)BPM;
        for (int j=0; j<score.Count; j++){
            float delta_time = 0;
            List<Note> track = score[j];
            for (int i=0; i<track.Count; i++){
                Note note = track[i];
                delta_time += note.delta*C;
                play_note(note.mode, note.ch, note.no, delta_time, note.len*C, note.velocity);
            }
        }
    }
}
