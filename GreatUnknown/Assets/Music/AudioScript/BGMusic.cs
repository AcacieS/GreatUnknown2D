using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] private BGMusicsInfo bgMusics;
    // [SerializeField] private BGMusicInfo creakySource;
    // [SerializeField] private BGMusicInfo anomalySource;
    // [SerializeField] private BGMusicInfo engineSource;

    void Awake()
    {
        for(int i=0; i<bgMusics.musicInfos.Count; i++)
        {
            BGMusicInfo bgMusicInfo = bgMusics.musicInfos[i];
            bgMusicInfo.AddAudioSourceGO(transform.GetChild(i).gameObject);
        }
        // if (creakySource == null) Ext.WarnRefAndDisable("creakySource", this);
        // if (anomalySource == null) Ext.WarnRefAndDisable("anomalySource", this);
        // if (engineSource == null) Ext.WarnRefAndDisable("engineSource", this);
    }

    void Start()
    {
        // creakySource.AddAudioSource();
        // anomalySource.AddAudioSource();
        // engineSource.AddAudioSource();
        
    }
    
    public void PlayNewBGMusic()
    {
        foreach(BGMusicInfo bgMusicInfo in bgMusics.musicInfos)
        {
            AudioInfo audioInfo = SoundManager.instance.GetSoundCatalogue(bgMusicInfo.audioName);
            bgMusicInfo.Activate(audioInfo);
        }
        
    }
    public void StopBGMusic()
    {
        foreach(BGMusicInfo bgMusicInfo in bgMusics.musicInfos)
        {
            bgMusicInfo.Desactivate();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
