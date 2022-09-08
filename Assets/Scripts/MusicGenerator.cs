using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    RhythmGenerator rhythmGenerator;
    MusicPlayer musicPlayer;
#endif
    // Start is called before the first frame update
    void Start()
    {
        test_start();
    }

    void test_start(){
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        rhythmGenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        var rhythm = rhythmGenerator.generate_8bar_rhythm();
        List<List<Note>> score = generate_8bar_music(rhythm);
        StartCoroutine(musicPlayer.play_music_c(score));
    }

    List<List<Note>> generate_8bar_music(int[] rhythm){
        List<List<Note>> score = new List<List<Note>>();
        //基本トラック
        score.Add(generate_trak0(rhythm));
        //メインリズム変化形
        score.Add(generate_trak1(rhythm));
        return score;
    }

    List<Note> generate_trak0(int[] rhythm){
        List<Note> track = new List<Note>();
        for (var i=0; i<rhythm.Length; i++){
            if (i == 0){
                track.Add(new Note(9, 38+30, 0, 1f/4f, 80));
            } else if (i % 4 == 2){
                track.Add(new Note(9, 38+42, 1f/2f, 1f/4f, 127));
            }else{
                track.Add(new Note(9, 38+30, 1f/2f, 1f/4f, 80));
            }
            if (rhythm[i] > 0){
                track.Add(new Note(9, 51, 0f, 1f/4f, 127));
            }       
        }
        return track;
    }
    List<Note> generate_trak1(int[] rhythm){
        List<Note> track = new List<Note>();
        track.Add(new Note(0, 117, 0, 0, 0).program_change());
        int[] rhythm2 = new int[128];
        for (var i=0; i<rhythm2.Length; i++){
            if (i%2 == 0){
                rhythm2[i] = rhythm[i/2]; 
            } else {
                rhythm2[i] = UnityEngine.Random.Range(0, 2); 
            }
        }
        float delta = 0;
        for (var i=0; i<rhythm2.Length; i++){
            if (rhythm2[i] > 0){
                track.Add(new Note(0, 30, delta, 1f/8f, 80));
                delta = 0;
            }
            delta += 1f/4f;
        }
        return track;
    }
}

public class MusicMode {
    int range = 7;

}

public class Note {
    public byte ch;
    public byte no;
    public float delta;
    public float len;
    public byte velocity;
    public int mode = 0x9;
    public Note(byte ch, byte no, float delta, float len,byte velocity){
        this.ch = ch;
        this.no = no;
        this.delta = delta;
        this.len = len;
        this.velocity = velocity;
    }

    public Note program_change(){
        this.mode = 0xC;
        return this;
    }
}