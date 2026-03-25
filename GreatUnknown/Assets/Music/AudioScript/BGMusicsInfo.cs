using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BGMusicsInfo", menuName = "Scriptable Objects/BGMusicsInfo")]
public class BGMusicsInfo: ScriptableObject
{
    public List<BGMusicInfo> musicInfos = new List<BGMusicInfo>();
}