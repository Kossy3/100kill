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
    [SerializeField]
    Midi[] midi;
    private List<List<List<Note>>> midi_score;
    int[] code_progress = new int[4]; //コード進行保存用
    [SerializeField]
    [Header("melody_scale: コードのスケール。0から")]
    int[] code_scale = new int[] { 0, 2, 4, 5, 7, 9, 11 };
    [SerializeField]
    [Header("melody_scale: 旋律のスケール。0から")]
    int[] melody_scale = new int[] { 0, 2, 4, 7, 9 };
    [SerializeField]
    [Header("melody_root: 旋律の基準音 midiノート番号")]
    int melody_root = 60;
    [SerializeField]
    [Header("melody_max: 旋律の最高音 基準音からのスケール量")]
    int melody_max = 5;
    [SerializeField]
    [Header("melody_min: 旋律の最低音 基準音からのスケール量")]
    int melody_min = -5;


    // Start is called before the first frame update
    void Start()
    {
        
        database = GameObject.Find("Database").GetComponent<Database>();
        //test_start();]
        midi_score = new List<List<List<Note>>>();
        foreach(Midi m in midi){
            midi_score.Add(m.Score());
        }
    }


    void test_start()
    {
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        rhythmGenerator = GameObject.Find("RhythmGenerator").GetComponent<RhythmGenerator>();
        var rhythm = rhythmGenerator.generate_8bar_rhythm();
        List<List<Note>> score = generate_8bar_music(rhythm);
        StartCoroutine(musicPlayer.play_music_c(score));
    }

    public List<List<Note>> generate_8bar_music(int[] rhythm)
    {
        return midi_score[0];
    }
    public List<List<Note>> generate_8bar_music_old(int[] rhythm)
    {
        List<List<Note>> score = new List<List<Note>>();
        var colors = defeated_colors;
        get_random(colors);
        //アクションに合わせた音
        score.Add(generate_track0(rhythm));
        if (defeated_colors.Count < 1)
        {
            //刻みだけ
            //score.Add(generate_taiko(0));
            //score.Add(generate_track1_drum(0));
        }
        else
        {
            //ドラム
            //score.Add(generate_track1_drum(colors[0]));
        }
        //score.Add(generate_track1(rhythm));
        score.Add(test_code(1));
        score.Add(generate_melody(0, rhythm));


        //メインリズム変化形
        defeated_colors.Add(UnityEngine.Random.Range(1, 1 + 3));
        return score;
    }

    List<Note> generate_track0(int[] rhythm)
    { //アクションに合わせた音
        List<Note> track = new List<Note>();
        float delta = 0;
        for (var i = 0; i < rhythm.Length; i++)
        {
            if (rhythm[i] > 0)
            {
                track.Add(new Note(9, 42, delta, 1f / 8f, 127));
                delta = 0;
            }
            delta += 1f / 2f;
        }
        return track;
    }

    List<Note> generate_track1_drum(int type)
    { //刻みの変化形
        List<Note> track = new List<Note>();
        float delta = 0;
        if (type == type + 0)
        {
            for (var i = 0; i < 8; i++)
            {
                track.Add(new Note(9, 36, delta, 1f / 8f, 100));
                track.Add(new Note(9, 36, 1f / 2f, 1f / 8f, 100));
                track.Add(new Note(9, 38, 1f / 2f, 1f / 8f, 127));
                track.Add(new Note(9, 36, 1f / 2f, 1f / 8f, 100));
                track.Add(new Note(9, 38, 1f / 4f, 1f / 8f, 127));
                track.Add(new Note(9, 38, 1f / 2f, 1f / 8f, 127));
                track.Add(new Note(9, 36, 1f / 4f, 1f / 8f, 100));
                track.Add(new Note(9, 38, 1f / 2f, 1f / 8f, 127));
                delta = 1f;
            }
        }
        return track;
    }

    List<Note> generate_taiko(int type)
    { //刻みの変化形
        List<Note> track = new List<Note>();
        float delta = 0;
        track.Add(new Note(0, 117, 0, 0, 0).program_change());
        if (type == type + 0)
        {
            for (var i = 0; i < 8; i++)
            {
                track.Add(new Note(0, 48, delta, 1f / 8f, 100));
                track.Add(new Note(0, 48, 1f / 2f, 1f / 8f, 100));
                track.Add(new Note(0, 52, 1f / 2f, 1f / 8f, 127));
                track.Add(new Note(0, 48, 1f / 4f, 1f / 8f, 100));
                track.Add(new Note(0, 48, 3f / 4f, 1f / 8f, 127));
                track.Add(new Note(0, 52, 1f / 1f, 1f / 8f, 127));
                track.Add(new Note(0, 48, 1f / 2f, 1f / 8f, 127));
                delta = 1f / 2f;
            }
        }
        return track;
    }

    List<Note> generate_test(int type)
    { //てすと しっくりこないリズムができる
        List<Note> track = new List<Note>();
        float delta = 0;
        for (var i = 0; i < 4; i++)
        {
            float[] rhythm = create_1bar_rhythm_pattern(0);
            Debug.Log(string.Join(",", rhythm));
            track.Add(new Note(0, 1, 4f - delta, 1f / 4f, 1));
            delta = 0;
            for (int ii = 0; ii < rhythm.Length; ii++)
            {
                delta += rhythm[ii];
                track.Add(new Note(0, 72, rhythm[ii], 1f / 8f, 127));
                ii++;
            }
        }
        return track;
    }

    List<Note> generate_melody(int ch, int[] rhythm)
    { //メロディーの生成
        List<Note> track = new List<Note>();
        track.Add(new Note(ch, 77, 0, 0, 0).program_change());
        int scale_index = (melody_root - (melody_root % 12)) / 12 * melody_scale.Length; //最初の音
        float delta = 0;
        for (var i = 0; i < rhythm.Length; i++)
        {
            if (rhythm[i] > 0)
            {
                update_scale_index(ref scale_index);
                int trans = melody_scale[scale_index % melody_scale.Length]; //音階
                int octave = (scale_index - scale_index % melody_scale.Length) / melody_scale.Length * 12; //オクターブ
                int modulation = melody_root % 12; //変調
                int note = octave + trans + modulation;
                //Debug.Log($"スケール{scale_index} 音程{note}");
                track.Add(new Note(ch, note, delta, 1f / 2f, 127));
            }
            delta += 1f / 2f;
        }
        for (int i = 0; i + 1 < track.Count; i++)
        {
            track[i].len = track[i + 1].delta - track[i].delta - 1f / 8f;
        }
        track[track.Count - 1].len = 32f - track[track.Count - 1].delta;
        transrate_delta(ref track);
        return track;
    }
    List<Note> test_code(int ch){
        List<Note> track = new List<Note>();
        track.Add(new Note(ch, 2, 0, 0, 0).program_change());
        int[][] val = new int[][] {
            new int[4]{4,12+7,24+0,24+2},
            new int[4]{5,12+7,24+0,24+4},
            new int[4]{7,12+7,24+0,24+2},
            new int[4]{9,12+7,24+0,24+4},
            new int[4]{4,12+7,24+0,24+2},
            new int[4]{5,12+7,24+0,24+4},
            new int[4]{7,12+7,24+0,24+2},
            new int[4]{9,12+7,24+0,24+4}
        };
        float delta = 0;
        for (int i = 0; i < 8; i++)
        {
            track.Add(new Note(ch, 36 +val[i][0], delta, 40f/12f, 80));
            track.Add(new Note(ch, 36 +val[i][1], delta, 40f/12f, 80));
            track.Add(new Note(ch, 36 +val[i][2], delta, 40f/12f, 80));
            track.Add(new Note(ch, 36 +val[i][3], delta, 40f/12f, 80));
            delta += 4f;
        }
        transrate_delta(ref track);
        return track;
    }

    List<Note> generate_code(int ch)
    {
        List<Note> track = new List<Note>();
        track.Add(new Note(ch, 107, 0, 0, 0).program_change());
        int[] val = new int[3] { 1, 2, 5 };
        int[] rate = new int[3] { 1, 1, 1 };
        float delta = 0;
        for (int i = 0; i < 4; i++)
        {
            int rnd = GetRandomIndex(rate);
            code_progress[i] = val[rnd];
            int modulation = melody_root % 12; //変調
            int code_root = 48 + code_scale[val[rnd]] + modulation; //コードのルート音
            int code_3rd = 48 + code_scale[(val[rnd] + 2) % code_scale.Length] + modulation;
            int code_5th = 48 + code_scale[(val[rnd] + 4) % code_scale.Length] + modulation;
            int[] code = new int[] { code_root, code_3rd, code_5th };
            delta = i * 8f;
            code_acc_part(ref track, ch, delta, code, 2);
        }
        transrate_delta(ref track);
        return track;
    }

    void code_acc_part(ref List<Note> track, int ch, float delta, int[] code, int type)
    {
        int bass_root = code[0] - 12;    //ベースの音
        Array.Sort(code);
        switch (type)
        {
            case 1:
                track.Add(new Note(ch, bass_root, delta, 40f / 11f, 100));
                for (int i = 0; i < 2; i++)
                {
                    track.Add(new Note(ch, code[0], delta, 2f / 5f, 100));
                    delta += 1f / 2f;
                    track.Add(new Note(ch, code[2], delta, 2f / 5f, 100));
                    delta += 1f / 2f;
                    track.Add(new Note(ch, code[1], delta, 2f / 5f, 100));
                    delta += 1f / 2f;
                    track.Add(new Note(ch, code[2], delta, 2f / 5f, 100));
                    delta += 1f / 2f;
                }
                break;
            case 2:
                track.Add(new Note(ch, bass_root, delta, 40f / 11f, 100));
                for (int i = 0; i < 2; i++)
                {
                    track.Add(new Note(ch, code[0], delta, 4f / 5f, 100));
                    delta += 1f / 1f;
                    track.Add(new Note(ch, code[1], delta, 2f / 5f, 100));
                    track.Add(new Note(ch, code[2], delta, 2f / 5f, 100));
                    delta += 1f / 2f;
                    track.Add(new Note(ch, code[0], delta, 2f / 5f, 100));
                    delta += 1f / 1f;
                    track.Add(new Note(ch, code[0], delta, 2f / 5f, 100));

                    delta += 1f / 2f;
                    track.Add(new Note(ch, code[1], delta, 4f / 5f, 100));
                    track.Add(new Note(ch, code[2], delta, 4f / 5f, 100));
                    delta += 1f / 1f;
                }
                break;

        }
    }

    void update_scale_index(ref int scale_index)
    {
        int i_root = (melody_root - (melody_root % 12)) / 12 * melody_scale.Length;
        int i_max = i_root + melody_max;
        int i_min = i_root + melody_min;
        int i = 0;
        while (!(i_min <= i && i <= i_max))
        {
            int vec = new int[] { 1, -1 }[GetRandomIndex(new int[] { 1, 1 })];
            int val = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }[GetRandomIndex(new int[] { 16, 64, 32, 16, 8, 4, 2, 1 })];
            i = scale_index + vec * val;
        }
        scale_index = i;
    }

    int GetRandomIndex(params int[] weightTable)
    {
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

    float[] create_1bar_rhythm_pattern(int notes)
    {
        int bar_divide = new int[] { 1, 2, 4 }[GetRandomIndex(new int[] { 1, 2, 4 })];
        float[] pattern = new float[16];
        float[] value = new float[16];
        int index = 0;
        float delta = 0;
        int beat = notes;
        Debug.Log($"------------------------------------");
        for (int i = 0; i < bar_divide; i++)
        {
            Debug.Log($"---------");
            int beat_divide = new int[] { 2, 3, 4 }[GetRandomIndex(new int[] { 4, 1, 16 })];
            for (int ii = 0; ii < beat_divide; ii++)
            {
                if (UnityEngine.Random.Range(0, 2) > 0)
                {
                    pattern[index] = delta;
                    index++;
                    Debug.Log($"区切り{bar_divide} 刻み{beat_divide} delta={delta}");
                    delta = 4f / (float)bar_divide / (float)beat_divide; ;
                }
                else
                {
                    delta += 4f / (float)bar_divide / (float)beat_divide;
                }
            }
        }
        Array.Resize(ref pattern, index);
        return pattern;
    }

    void transrate_delta(ref List<Note> track)
    {
        List<Note> track_fix = new List<Note>();
        float delta = 0;
        float d_save = 0;
        for (int i = 0; i < track.Count; i++)
        {
            if (i > 0 && d_save != track[i].delta)
            {
                delta = track[i].delta - d_save;
            }
            else if (i > 0)
            {
                delta = 0;
            }
            d_save = track[i].delta;
            track[i].delta = delta;
            //Debug.Log(delta);
        }
    }

    int[] get_random(List<int> colors)
    {
        string random_str = "";
        int[] randoms = new int[colors.Count];
        for (int i = 0; i < colors.Count; i++)
        {
            random_str = $"{random_str}{colors[i]}";
            randoms[i] = (int)((uint)random_str.GetHashCode() >> 16);
            Debug.Log($"hash: {randoms[i]}");
        }
        return randoms;
    }
}

[System.Serializable]
public class Note
{
    public byte ch;
    public byte no;
    public float delta;
    public float len;
    public byte velocity;
    public byte mode = 0x9;
    public Note(int ch, int no, float delta, float len, byte velocity)
    {
        this.ch = (byte)ch;
        this.no = (byte)no;
        this.delta = delta;
        this.len = len;
        this.velocity = velocity;
    }

    public Note program_change()
    {
        this.mode = 0xC;
        return this;
    }

    public Note off(){
        this.mode = 0x8;
        return this;
    }
}

[System.Serializable]
public class Track {
    public List<Note> List;
    public Track(List<Note> list){
        List = list;
    }
}

class DrumPattern
{
    public List<Note>[]drums = new List<Note>[4];
    public DrumPattern()
    {
        Drum_8beat(0);
        Drum_maeda(1);

    }
    void Drum_8beat(int n){
        List<Note> track = new List<Note>();
        for (var i = 0; i < 64; i++)
        {
            if (i == 0)
            {
                track.Add(new Note(9, 36, 0, 1f / 4f, 100));
            }
            else if (i % 4 == 2)
            {
                track.Add(new Note(9, 38, 1f / 2f, 1f / 4f, 127));
            }
            else
            {
                track.Add(new Note(9, 36, 1f / 2f, 1f / 4f, 100));
            }
        }
        drums[n] = track;
    }
    void Drum_maeda(int n)
    {
        var track = new List<Note>();
        float delta = 0;
        for (var i = 0; i < 8; i++)
        {
            track.Add(new Note(9, 48, delta, 1f / 8f, 100));
            track.Add(new Note(9, 48, 1f / 2f, 1f / 8f, 100));
            track.Add(new Note(9, 52, 1f / 2f, 1f / 8f, 127));
            track.Add(new Note(9, 48, 1f / 4f, 1f / 8f, 100));
            track.Add(new Note(9, 48, 3f / 4f, 1f / 8f, 127));
            track.Add(new Note(9, 52, 1f / 1f, 1f / 8f, 127));
            track.Add(new Note(9, 48, 1f / 2f, 1f / 8f, 127));
            delta = 1f / 2f;
        }
        drums[n] = track;
    }
}



