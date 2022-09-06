using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    RythmGenerator rythmGenerator;
    MusicPlayer musicPlayer;
#endif
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        test_start();
#endif
    }

    void test_start(){
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        rythmGenerator = GameObject.Find("RhythmGenerator").GetComponent<RythmGenerator>();
        var rhythm = rythmGenerator.generate_8bar_rhythm();
        generate_8bar_music(rhythm);
        musicPlayer.play_music(rhythm);
    }

    int[] generate_8bar_music(int[] rhythm){
        return rhythm;
    }
}
