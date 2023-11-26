using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

public class Online : MonoBehaviour
{
    private RankingTable rankingtable;
    private MyScore myscore;

    private string url = "https://script.google.com/macros/s/AKfycbxqIkVKzD92Inh-Zm6AkOBUQzzxeygXv_fVQccKzGiF1MBrngi1wHML9sT8hFVglFuF_A/exec";

    public ScoresDatas scoresDatas = new ScoresDatas (new List<ScoresData> ());

    void Start()
    {
        rankingtable = GameObject.Find("RankingTable").GetComponent<RankingTable>();
        myscore = GameObject.Find("MyScore").GetComponent<MyScore>();

        StartCoroutine("GetText");

    }

    public IEnumerator GetText()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }

        else
        {
            string textResult = request.downloadHandler.text;
            string jsonText = MyAes.DecryptStringFromBytes_Aes(JsonUtility.FromJson<ParseJson>(textResult).code);
            scoresDatas = JsonUtility.FromJson<ScoresDatas>(jsonText);

            foreach (ScoresData scoresData in scoresDatas.scoresDatas)
            {
                myscore.name_list.Add(scoresData.playerName);
            }

            if (GameObject.Find("Database") == null)
            {
                rankingtable.generate_ranking(null, null);
            }
        }
    }

    public IEnumerator SendText(string player_name, int[] my_scores)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }

        else
        {
            string textResult = request.downloadHandler.text;
            string jsonText = MyAes.DecryptStringFromBytes_Aes(JsonUtility.FromJson<ParseJson>(textResult).code);
            scoresDatas = JsonUtility.FromJson<ScoresDatas>(jsonText);

            if (player_name != "guestplay")
            {
                scoresDatas.scoresDatas.Add(new ScoresData (player_name, new List<int> (my_scores)));
            }

            jsonText = JsonUtility.ToJson(new ParseJson(MyAes.EncryptStringToBytes_Aes(JsonUtility.ToJson(scoresDatas))));
                
            byte[] postData = Encoding.UTF8.GetBytes (jsonText);

            using var send_request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(postData),
                downloadHandler = new DownloadHandlerBuffer()
            };

            send_request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return send_request.SendWebRequest();
     
            if (send_request.isNetworkError || send_request.isHttpError) 
            {
                Debug.Log(send_request.error);
            }

            else 
            {
                rankingtable.generate_ranking(player_name, my_scores);
            }
        }
    }
}

public static class MyAes
{
    private const string AES_IV_256 = @"7Xc58HyNcrBPyytK";
    private const string AES_Key_256 = @"U2FsdGVkX1K9nXyrQjCuAIbyU8lZq0iP";

    //暗号化のための関数
    //引数は暗号化したいデータ(string)
    public static string EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            //AESの設定
            aesAlg.BlockSize = 128;  //
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return (System.Convert.ToBase64String(encrypted));
         // Return the encrypted bytes from the memory stream.
    }

    //復号のための関数
    //引数は暗号化されたデータ(byte[])
    public static string DecryptStringFromBytes_Aes(string cipherText)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            //AESの設定(暗号と同じ)
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(System.Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}

public class ParseJson 
{
    public string code;

    public ParseJson(string code) {
    this.code = code;
    }
}