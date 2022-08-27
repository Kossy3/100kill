using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using System;

public class test : MonoBehaviour
{
    static string res;
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
        Debug.Log(numDevs);
        MidiOutCaps myCaps = new MidiOutCaps();
 
        //0番ポートの調査を行う。
        var res = midiOutGetDevCaps(1, ref myCaps, (System.UInt32)Marshal.SizeOf(myCaps));
 
        //引数１はポインタ扱いの模様。
#if UNITY_EDITOR
        Debug.Log(myCaps.szPname);
#endif
        DeviceInitialize();
        midiOutMsgFixed(handle, 0x9, 0, 0x45, 40);
    }
 
    void Update(){
        if (KeyOn && Time.time > 1){
            Debug.Log("off");
            midiOutMsgFixed(handle, 0x9, 0, 0x45, 0);
            KeyOn = false;
        }
        if( KeyOn == true){
            //midiOutMsgFixed(handle, 0x9, 0, 0x45, 40);
 
        }
        else if(KeyOn==false){
            //midiOutMsgFixed(handle, 0x9, 0, 0x45, 0);
        }
    }
 
    public void Test1(){
            midiOutMsgFixed(handle, 0x9, 0, 0x45, 40);
    }
 
    public void Test2(){
            midiOutMsgFixed(handle, 0x9, 0, 0x45, 0);
    }
 
    public void DeviceInitialize(){
        int midi_no=-1;
        if( device_init == false){
            var res = midiOutOpen( out handle, midi_no , null , 0 , 0);
            device_init = true;
#if UNITY_EDITOR
 
            Debug.Log(handle);
            Debug.Log(res);
#endif
        }
    }
 
    public void DeviceClose(){
        if( device_init == true){
            midiOutReset(handle);
            var res=midiOutClose(handle);
#if UNITY_EDITOR
            Debug.Log(handle);
            Debug.Log(res);
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
}
