using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ScoresDatas
{
    public List<ScoresData> scoresDatas;

    public ScoresDatas(List<ScoresData> scoresDatas)
    {
        this.scoresDatas = scoresDatas;
    }
}

[Serializable]
public class ScoresData
{
    public string playerName;
    public List<int> scores;

    public ScoresData(string playerName, List<int> scores)
    {
        this.playerName = playerName;
        this.scores = scores;
    }
}
