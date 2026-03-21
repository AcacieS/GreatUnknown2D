using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] private BGMusicInfo creakySource;
    [SerializeField] private BGMusicInfo anomalySource;
    [SerializeField] private BGMusicInfo engineSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        creakySource.AddAudioSource();
        anomalySource.AddAudioSource();
        engineSource.AddAudioSource();
    }
    
    public void PlayNewBGMusic(BGMusicType musicInfo)
    {
        AudioInfo audioInfo = SoundManager.instance.GetSoundCatalogue(musicInfo.ToString());
        switch (musicInfo)
        {
            case BGMusicType.anomaly:
                anomalySource.Activate(audioInfo);
                break;
            case BGMusicType.engine:
                engineSource.Activate(audioInfo);
                break;
            case BGMusicType.creaky:
                creakySource.Activate(audioInfo);
                break;
            
        }
        
    }
    public void StopBGMusic(BGMusicType musicInfo)
    {
        AudioInfo audioInfo = SoundManager.instance.GetSoundCatalogue(musicInfo.ToString());
        switch (musicInfo)
        {
            case BGMusicType.anomaly:
                anomalySource.Desactivate(audioInfo);
                break;
            case BGMusicType.engine:
                engineSource.Desactivate(audioInfo);
                break;
            case BGMusicType.creaky:
                creakySource.Desactivate(audioInfo);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
