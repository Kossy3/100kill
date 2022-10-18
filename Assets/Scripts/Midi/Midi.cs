using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu]
public class Midi : ScriptableObject
{
    public List<NoteData> noteList = new List<NoteData>();
    public HeaderChunkData headerChunk;
    public TrackChunkData[] trackChunks;
    public List<Track> score;
    public void load(BinaryReader reader)
    {
        headerChunk = new HeaderChunkData();
        // リスト初期化
        noteList.Clear();


        // チャンクID
        headerChunk.chunkID = reader.ReadBytes(4);

        // 自分のPCがリトルエンディアンならバイト順を逆に
        if (BitConverter.IsLittleEndian)
        {
            // ヘッダ部のデータ長(値は6固定)
            var byteArray = reader.ReadBytes(4);
            Array.Reverse(byteArray);
            headerChunk.dataLength = BitConverter.ToInt32(byteArray, 0);
            // フォーマット(2byte)
            byteArray = reader.ReadBytes(2);
            Array.Reverse(byteArray);
            headerChunk.format = BitConverter.ToInt16(byteArray, 0);
            // トラック数(2byte)
            byteArray = reader.ReadBytes(2);
            Array.Reverse(byteArray);
            headerChunk.tracks = BitConverter.ToInt16(byteArray, 0);
            // タイムベース(2byte)
            byteArray = reader.ReadBytes(2);
            Array.Reverse(byteArray);
            headerChunk.division = BitConverter.ToInt16(byteArray, 0);
        }
        else
        {
            // ヘッダ部のデータ長(値は6固定)
            headerChunk.dataLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
            // フォーマット(2byte)
            headerChunk.format = BitConverter.ToInt16(reader.ReadBytes(2), 0);
            // トラック数(2byte)
            headerChunk.tracks = BitConverter.ToInt16(reader.ReadBytes(2), 0);
            // タイムベース(2byte)
            headerChunk.division = BitConverter.ToInt16(reader.ReadBytes(2), 0);
        }

        /* トラックチャンク侵入 */
        trackChunks = new TrackChunkData[headerChunk.tracks];

        // トラック数ぶん
        for (int i = 0; i < headerChunk.tracks; i++)
        {
            trackChunks[i] = new TrackChunkData();

            // チャンクID
            trackChunks[i].chunkID = reader.ReadBytes(4);

            // 自分のPCがリトルエンディアンなら変換する
            if (BitConverter.IsLittleEndian)
            {
                // トラックのデータ長読み込み(値は6固定)
                var byteArray = reader.ReadBytes(4);
                Array.Reverse(byteArray);
                trackChunks[i].dataLength = BitConverter.ToInt32(byteArray, 0);
            }
            else
            {
                trackChunks[i].dataLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
            }

            // データ部読み込み
            trackChunks[i].data = reader.ReadBytes(trackChunks[i].dataLength);

            // データ部解析
            TrackDataAnalysis(trackChunks[i].data, headerChunk);
        }
        translate_Note();
    }

    public void TrackDataAnalysis(byte[] data, HeaderChunkData headerChunk)
    {
        uint currentTime = 0;                    // デルタタイムを足していく、つまり現在の時間(ms)（ノーツやソフランのイベントタイムはこれを使う）
        byte statusByte = 0;                     // ステータスバイト
        bool[] longFlags = new bool[128];    // ロングノーツ用フラグ

        // データ分
        for (int i = 0; i < data.Length;)
        {
            // デルタタイム格納用
            uint deltaTime = 0;

            while (true)
            {
                var tmp = data[i++];

                // 下位7bitを格納
                deltaTime |= tmp & (uint)0x7f;

                // 最上位1bitが0ならデータ終了
                if ((tmp & 0x80) == 0) break;

                // 次の下位7bit用にビット移動
                deltaTime = deltaTime << 7;
            }
            // 現在の時間にデルタタイムを足す
            currentTime += deltaTime;
            /* ランニングステータスチェック */
            if (data[i] < 0x80)
            {
                // ランニングステータス適応(前回のステータスバイトを使いまわす)
            }
            else
            {
                // ステータスバイト保存
                statusByte = data[i++];
            }
            // ステータスバイト後のデータ保存用
            byte dataByte0, dataByte1, dataLength;

            /* MIDIイベント(ステータスバイト0x80-0xEF) */
            if (statusByte >= 0x80 && statusByte <= 0xef)
            {
                switch (statusByte & 0xf0)
                {
                    /* チャンネルメッセージ */

                    case 0x80:  // ノートオフ
                                // どのキーが離されたか
                        dataByte0 = data[i++];
                        // ベロシティ値
                        dataByte1 = data[i++];
                        {
                            var note = new NoteData();
                            note.ch = (int)(statusByte & 0x0f);
                            note.eventTime = (int)currentTime;
                            note.laneIndex = (int)dataByte0;
                            note.velocity = (int)dataByte1;
                            note.type = NoteType.OFF;
                            // リストにつっこむ
                            noteList.Add(note);

                            // ロングノーツフラグ解除
                            longFlags[note.laneIndex] = false;
                            //Debug.Log($"ch{note.ch} time{note.eventTime} no{note.laneIndex} type{note.type} ve{note.velocity}");
                        }
                        break;
                    case 0x90:  // ノートオン(ノートオフが呼ばれるまでは押しっぱなし扱い)
                                // どのキーが押されたか
                        dataByte0 = data[i++];
                        // ベロシティ値という名の音の強さ。ノートオフメッセージの代わりにここで0を送ってくるタイプもある
                        dataByte1 = data[i++];
                        // ノート情報生成
                        {
                            var note = new NoteData();
                            note.ch = (int)(statusByte & 0x0f);
                            note.eventTime = (int)currentTime;
                            note.laneIndex = (int)dataByte0;
                            note.velocity = (int)dataByte1;
                            note.type = NoteType.ON;
                            // ノートオフイベントではなく、ベロシティ値0をノートオフとして保存する形式もあるので対応
                            if (dataByte1 == 0)
                            {
                                note.type = NoteType.OFF;
                            }

                            // リストにつっこむ
                            noteList.Add(note);
                            //Debug.Log($"ch{note.ch} time{note.eventTime} no{note.laneIndex} type{note.type} ve{note.velocity}");
                        }
                        break;
                    case 0xa0:  // ポリフォニック キープレッシャー(鍵盤楽器で、キーを押した状態でさらに押し込んだ際に、その圧力に応じて送信される)
                        i += 2; // 使わないのでスルー
                        break;
                    case 0xb0:  // コントロールチェンジ(音量、音質など様々な要素を制御するための命令)
                                // コントロールする番号
                        dataByte0 = data[i++];
                        // 設定する値
                        dataByte1 = data[i++];

                        // ※0x00-0x77までがコントロールチェンジで、それ以上はチャンネルモードメッセージとして処理する
                        if (dataByte0 < 0x78)
                        {
                            // コントロールチェンジ
                        }
                        else
                        {
                            // チャンネルモードメッセージは一律データバイトを2つ使用している
                            // チャンネルモードメッセージ
                            switch (dataByte0)
                            {
                                case 0x78:  // オールサウンドオフ
                                            // 該当するチャンネルの発音中の音を直ちに消音する。後述のオールノートオフより強制力が強い。
                                    break;
                                case 0x79:  // リセットオールコントローラ
                                            // 該当するチャンネルの全種類のコントロール値を初期化する。
                                    break;
                                case 0x7a:  // ローカルコントロール
                                            // オフ:鍵盤を弾くとMIDIメッセージは送信されるがピアノ自体から音は出ない
                                            // オン:鍵盤を弾くと音源から音が出る(基本こっち)
                                    break;
                                case 0x7b:  // オールノートオフ
                                            // 該当するチャンネルの発音中の音すべてに対してノートオフ命令を出す
                                    break;
                                /* MIDIモード設定 */
                                // オムニのオン・オフとモノ・ポリモードを組み合わせて4種類のモードがある
                                case 0x7c:  // オムニモードオフ
                                    break;
                                case 0x7d:  // オムニモードオン
                                    break;
                                case 0x7e:  // モノモードオン
                                    break;
                                case 0x7f:  // モノモードオン
                                    break;
                            }
                        }
                        break;

                    case 0xc0:  // プログラムチェンジ(音色を変える命令)
                        dataByte0 = data[i++];
                        {
                            var note = new NoteData();
                            note.ch = (int)(statusByte & 0x0f);
                            note.eventTime = (int)currentTime;
                            note.laneIndex = (int)dataByte0;
                            note.velocity = (int)dataByte0;
                            note.type = NoteType.ProgramChange;
                            // リストにつっこむ
                            noteList.Add(note);
                        }
                        break;

                    case 0xd0:  // チャンネルプレッシャー(概ねポリフォニック キープレッシャーと同じだが、違いはそのチャンネルの全ノートナンバーに対して有効となる)
                        i += 1;
                        break;

                    case 0xe0:  // ピッチベンド(ウォェーンウェューンの表現で使う)
                        i += 2;
                        // ボルテのつまみみたいなのを実装する場合、ここの値が役立つかも
                        break;
                }
            }

            /* システムエクスクルーシブ (SysEx) イベント*/
            else if (statusByte == 0x70 || statusByte == 0x7f)
            {
                dataLength = data[i++];
                i += dataLength;
            }

            /* メタイベント*/
            else if (statusByte == 0xff)
            {
                // メタイベントの番号
                byte metaEventID = data[i++];
                // データ長
                dataLength = data[i++];

                switch (metaEventID)
                {
                    case 0x00:  // シーケンスメッセージ
                        i += dataLength;
                        break;
                    case 0x01:  // テキストイベント
                        i += dataLength;
                        break;
                    case 0x02:  // 著作権表示
                        i += dataLength;
                        break;
                    case 0x03:  // シーケンス/トラック名
                        i += dataLength;
                        break;
                    case 0x04:  // 楽器名
                        i += dataLength;
                        break;
                    case 0x05:  // 歌詞
                        i += dataLength;
                        break;
                    case 0x06:  // マーカー
                        i += dataLength;
                        break;
                    case 0x07:  // キューポイント
                        i += dataLength;
                        break;
                    case 0x20:  // MIDIチャンネルプリフィクス
                        i += dataLength;
                        break;
                    case 0x21:  // MIDIポートプリフィックス
                        i += dataLength;
                        break;
                    case 0x2f:  // トラック終了
                        i += dataLength;
                        // ここでループを抜けても良い
                        break;
                    case 0x51:  // テンポ変更
                        {
                            // テンポ変更情報リストに格納する


                            // ４分音符の長さをマイクロ秒単位で格納されている
                            uint tempo = 0;
                            tempo |= data[i++];
                            tempo <<= 8;
                            tempo |= data[i++];
                            tempo <<= 8;
                            tempo |= data[i++];
                        }
                        break;
                    case 0x54:  // SMTPEオフセット
                        i += dataLength;
                        break;
                    case 0x58:  // 拍子
                        i += dataLength;
                        // 小節線を表示させるなら使えるかも
                        break;
                    case 0x59:  // 調号
                        i += dataLength;
                        break;
                    case 0x7f:  // シーケンサ固有メタイベント
                        i += dataLength;
                        break;
                }
            }
        }

    }
    private void translate_Note()
    {
        score = new List<Track>();
        for (int i = 0; i < 16; i++)
        {
            score.Add(new Track(new List<Note>()));
        }
        foreach (var d in noteList)
        {
            int eventtime = d.eventTime;
            if(d.ch == 9 && d.type == NoteType.OFF){
                eventtime += 14;
            }
            var note = new Note(d.ch, d.laneIndex, (float)(eventtime - (headerChunk.division * 4)) / headerChunk.division, 0, (byte)d.velocity);
            if (d.type == NoteType.OFF)
            {
                note.off();
            }
            if (d.type == NoteType.ProgramChange){
                note.program_change();
            }
            //Debug.Log($"ch{note.ch} no{note.no}, ve{note.velocity}, type{note.mode}, time{note.delta}");
            score[d.ch].List.Add(note);
        }
    }
    public List<List<Note>> Score (){
        List<List<Note>> sc = new List<List<Note>>();
        foreach (Track tr in score){
            sc.Add(new List<Note>());
            foreach(Note no in tr.List){
                sc[sc.Count-1].Add(no);
            }
        }
        return sc;
    }

    public struct HeaderChunkData
    {
        public byte[] chunkID;      // チャンクのIDを示す(4byte)
        public int dataLength;      // チャンクのデータ長(4byte)
        public short format;        // MIDIファイルフォーマット(2byte)
        public short tracks;        // トラック数(2byte)
        public short division;      // タイムベース MIDI独自の時間の最小単位をtickと呼び、4分音符あたりのtick数がタイムベース 大体480(2byte)
    };

    public struct TrackChunkData
    {
        public byte[] chunkID;      // チャンクのIDを示す(4byte)
        public int dataLength;      // チャンクのデータ長(4byte)
        public byte[] data;         // 演奏情報が入っているデータ
    };

    public enum NoteType
    {
        Normal,      // 通常ノーツ
        LongStart,   // ロング開始
        LongEnd,     // ロング終端
        OFF, //ノーツ終了
        ON, //ノーツ開始
        ProgramChange,
    }

    public struct NoteData
    {
        public int ch;
        public int velocity;
        public int eventTime;  // ノーツタイミング(ms)
        public int laneIndex;  // レーン番号
        public NoteType type;   // ノーツの種類
    }
}