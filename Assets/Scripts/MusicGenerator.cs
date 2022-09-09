using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicGenerator : MonoBehaviour
{
    [SerializeField]
    private Database database;
    RhythmGenerator rhythmGenerator;
    MusicPlayer musicPlayer;
    //テスト用スクリプト
    List<int> defeated_colors = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        //database = GameObject.Find("Database").GetComponent<Database>();
        //test_start();
    }

    void test_start(){
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        rhythmGenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        var rhythm = rhythmGenerator.generate_8bar_rhythm();
        List<List<Note>> score = generate_8bar_music(rhythm);
        StartCoroutine(musicPlayer.play_music_c(score));
    }
    int GetRandomIndex(int[] weightTable)
    {
        var totalWeight = weightTable.Sum();
        var value = UnityEngine.Random.Range(1, totalWeight + 1);
        var retIndex = -1;
        for (var i = 0; i < weightTable.Length; ++i)
        {
            if (weightTable[i] >= value)
            {
                retIndex = i;
                break;
            }
            value -= weightTable[i];
        }
        return retIndex;
    }

    public List<List<Note>> generate_8bar_music(int[] rhythm){
        List<List<Note>> score = new List<List<Note>>();
        var colors = database.defeated_color_number;
        //アクションに合わせた音
        score.Add(generate_track0(rhythm));
        if (defeated_colors.Count < 1){
            //刻みだけ
            
            score.Add(generate_track1(rhythm));
        } else {
            //ドラム
            score.Add(generate_track1_drum(colors[0]));
        }
        
        //メインリズム変化形
        defeated_colors.Add(UnityEngine.Random.Range(1,1+3));
        return score;
    }

    List<Note> generate_track0(int[] rhythm){
        List<Note> track = new List<Note>();
        float delta = 0;
        for (var i=0; i<rhythm.Length; i++){
            if (rhythm[i] > 0){
                track.Add(new Note(9, 42, delta, 1f/8f, 127));
                delta = 0;
            }
            delta += 1f/2f;
        }
        return track;
    }

    List<Note> generate_track1(int[] rhythm){
        List<Note> track = new List<Note>();
        for (var i=0; i<rhythm.Length; i++){
            if (i == 0){
                track.Add(new Note(9, 36,     0, 1f/4f, 100));
            } else if (i % 4 == 2){
                track.Add(new Note(9, 38, 1f/2f, 1f/4f, 127));
            }else{
                track.Add(new Note(9, 36, 1f/2f, 1f/4f, 100));
            }       
        }
        return track;
    }
    List<Note> generate_track1_drum(int type){
        List<Note> track = new List<Note>();
        float delta = 0;
        if(type == 1){
            for (var i=0; i<8; i++){
                track.Add(new Note(9, 36, delta, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/4f, 1f/8f, 127));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                delta = 1f/2f;
            }
        } else if (type ==2){
            for (var i=0; i<16; i++){
                track.Add(new Note(9, 36, delta, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/4f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                delta = 1f/4f;
            }
        } else {
            for (var i=0; i<16; i++){
                track.Add(new Note(9, 36, delta, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 127));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 100));
                delta = 1f/2f;
            }
        }
        return track;
    }
    List<Note> generate_track2(int[] rhythm){
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