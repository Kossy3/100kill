using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MusicGenerator : MonoBehaviour
{
    Database database;

    RhythmGenerator rhythmGenerator;
    MusicPlayer musicPlayer;
    //テスト用スクリプト
    List<int> defeated_colors = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        //test_start();
    }


    void test_start(){
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        rhythmGenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        var rhythm = rhythmGenerator.generate_8bar_rhythm();
        List<List<Note>> score = generate_8bar_music(rhythm);
        StartCoroutine(musicPlayer.play_music_c(score));
    }

    public List<List<Note>> generate_8bar_music(int[] rhythm){
        List<List<Note>> score = new List<List<Note>>();
        var colors = defeated_colors;
        //アクションに合わせた音
        score.Add(generate_track0(rhythm));
        if (defeated_colors.Count < 1){
            //刻みだけ
            //score.Add(generate_taiko(0));
            //score.Add(generate_track1(rhythm));
        } else {
            //ドラム
            //score.Add(generate_track1_drum(colors[0]));
        }
        //score.Add(generate_track1(rhythm));
        score.Add(generate_melody(rhythm));
        
        
        //メインリズム変化形
        defeated_colors.Add(UnityEngine.Random.Range(1,1+3));
        return score;
    }

    List<Note> generate_track0(int[] rhythm){ //アクションに合わせた音
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

    List<Note> generate_track1(int[] rhythm){ //刻みだけ
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

    List<Note> generate_track1_drum(int type){ //刻みの変化形
        List<Note> track = new List<Note>();
        float delta = 0;
        if(type == type+0){
            for (var i=0; i<8; i++){
                track.Add(new Note(9, 36, delta, 1f/8f, 100));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/2f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/4f, 1f/8f, 127));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                track.Add(new Note(9, 36, 1f/4f, 1f/8f, 100));
                track.Add(new Note(9, 38, 1f/2f, 1f/8f, 127));
                delta = 1f;
            }
        } 
        return track;
    }

    List<Note> generate_taiko(int type){ //刻みの変化形
        List<Note> track = new List<Note>();
        float delta = 0;
        track.Add(new Note(0, 117, 0, 0, 0).program_change());
        if(type == type+0){
            for (var i=0; i<8; i++){
                track.Add(new Note(0, 48, delta, 1f/8f, 100));
                track.Add(new Note(0, 48, 1f/2f, 1f/8f, 100));
                track.Add(new Note(0, 52, 1f/2f, 1f/8f, 127));
                track.Add(new Note(0, 48, 1f/4f, 1f/8f, 100));
                track.Add(new Note(0, 48, 3f/4f, 1f/8f, 127));
                track.Add(new Note(0, 52, 1f/1f, 1f/8f, 127));
                track.Add(new Note(0, 48, 1f/2f, 1f/8f, 127));
                delta = 1f/2f;
            }
        } 
        return track;
    }

    List<Note> generate_test(int type){ //てすと しっくりこないリズムができる
        List<Note> track = new List<Note>();
        float delta = 0;
        for (var i=0; i<4; i++){
            float[] rhythm = create_1bar_rhythm_pattern(0);
            Debug.Log(string.Join(",", rhythm));
            track.Add(new Note(0, 1, 4f - delta, 1f/4f , 1));
            delta = 0;
            for(int ii=0; ii < rhythm.Length; ii++){
                delta += rhythm[ii];
                track.Add(new Note(0, 72, rhythm[ii], 1f/8f , 127));
                ii++;
            }
        }
        return track;
    }

    List<Note> generate_melody(int[] rhythm){
        List<Note> track = new List<Note>();
        int[] scale = new int[]{0, 2, 4, 5, 7, 9, 11};
        int scale_index = 0;
        int base_note = 60;
        float delta = 0;
        for (var i=0; i<rhythm.Length; i++){
            if (rhythm[i] > 0){
                update_scale_index(ref scale_index);
                int scale_index_fix = scale_index%scale.Length;
                if (scale_index%scale.Length < 0){
                    scale_index_fix = scale.Length + scale_index%scale.Length;
                }
                Debug.Log(scale_index_fix);
                byte note = (byte)(base_note + (scale_index - scale_index % scale.Length) / scale.Length * 12 + scale[scale_index_fix]);
                track.Add(new Note(0, note, delta, 1f/7f , 127));
                delta = 0;
            }
            delta += 1f/2f;
        }
        return track;
    }

    void update_scale_index(ref int scale_index){
        int vec = new int[]{1, -1}[GetRandomIndex(new int[]{1,1})];
        int val = new int[]{0, 1, 2, 3, 4, 5, 6, 7}[GetRandomIndex(new int[]{64, 64, 32, 16, 8, 4, 2, 1})];
        scale_index += vec * val;
    }

    int GetRandomIndex(params int[] weightTable){
        var totalWeight = weightTable.Sum();
        var value = UnityEngine.Random.Range(1, totalWeight + 1);
        var retIndex = 0;
        for (var i = 0; i < weightTable.Length; i++)
        {
            retIndex += weightTable[i];
            if (retIndex >= value)
            {
                return i;
            }
        }
        return 0;
    }

    float[] create_1bar_rhythm_pattern(int notes){
        int bar_divide = new int[] {1, 2, 4} [GetRandomIndex(new int[]{1, 2, 4})];
        float[] pattern = new float[16];
        int index = 0;
        float delta = 0;
        int beat = notes;
        Debug.Log($"------------------------------------");
        for (int i=0; i< bar_divide; i++){
            Debug.Log($"---------");
            int beat_divide = new int[] {2, 3, 4} [GetRandomIndex(new int[]{4, 1, 16})];
            for (int ii=0; ii<beat_divide; ii++){
                if (UnityEngine.Random.Range(0,2) > 0){
                    pattern[index] = delta;
                    index ++;
                    Debug.Log($"区切り{bar_divide} 刻み{beat_divide} delta={delta}");
                    delta = 4f/(float)bar_divide/(float)beat_divide;;
                } else {
                    delta += 4f/(float)bar_divide/(float)beat_divide;
                }
            }
        }
        Array.Resize(ref pattern, index);
        return pattern;
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