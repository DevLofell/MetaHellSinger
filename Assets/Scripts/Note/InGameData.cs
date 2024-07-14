using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class InGameData : ScriptableObject
{
    public AudioClip introClip;

    public int mainSongBPM;

    //액트별 필요한 데이터
    public List<InGameActData> inGameActDataList;

    public int bossSongBPM;
    
    public AudioClip finalClip;

    public AudioClip bossContinue;
    public AudioClip bossEnd;

    
}

[System.Serializable]
public class InGameActData
{
    public string name;
    
    public AudioClip x1;
    public AudioClip x2;
    public AudioClip x3;
    public AudioClip x5;
}
