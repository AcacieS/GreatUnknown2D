using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable] 
public class NbFishPerGame
{
    [SerializeField] private int minNbOfFish;
    [SerializeField] private int maxNbOfFish;
    public NbFishPerGame()
    {
        
    }

    public int randomNbFish()
    {
        return UnityEngine.Random.Range(minNbOfFish, maxNbOfFish+1);
    }
}
[Serializable] 
public class MutatedPercentage
{
    [SerializeField, Range(0f,100f)] private float percentage;
    public MutatedPercentage()
    {
        
    }
    public float GetPercentage()
    {
        return percentage;
    }
}
[Serializable] 
public class FishGameInfo
{
    [SerializeField] private List<FishTypeInfo> fishesType;
    [SerializeField] private List<NbFishPerGame> nbOfFishPerGames;
    [SerializeField] private List<MutatedPercentage> percentageMutatedFishes;

    public int GetNbGame(){
        return nbOfFishPerGames.Count;
    }
    public FishGameInfo()
    {
        
    }
    public float GetPercentageMutatedFishes(int currentGameIndex)
    {
        return percentageMutatedFishes[currentGameIndex].GetPercentage();
    }
    public int GetNbOfFishPerGame(int currentGameIndex)
    {
        return nbOfFishPerGames[currentGameIndex].randomNbFish();
    }
    public FishTypeInfo GetFishInfo(int fishIndex)
    {
        return fishesType[fishIndex];
    }
    public int GetTotalFishType()
    {
        return fishesType.Count;
    }
    public void Validate()
    {
        if (nbOfFishPerGames.Count != percentageMutatedFishes.Count)
        {
            Debug.LogWarning("FishGameInfo lists were not same lengths.");
        }
    }
    
}
[Serializable] 
public class Fish{
    public bool isMutated;
    public Sprite sprite;
    public Fish(Sprite sprite, bool isMutated)
    {
        this.sprite = sprite;
        this.isMutated = isMutated;
    }
    public bool GetIsMutated()
    {
        return isMutated;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
}
public class FishManagement : MonoBehaviour
{
    public static FishManagement Instance {get; private set;}
    
    [SerializeField] private FishGameInfo fishGameInfo;
    
    [Header("Places")]
    [SerializeField] private GameObject fishPlace;
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGamePlace;

    [Header("Show for Debug")]
    [SerializeField] List<Fish> currentFishes = new List<Fish>();
    private int currentFishNb = 0;
    int currentFishIndex = 0;
    private bool isAnimatingOut = false;
    
    private void OnValidate()
    {
        fishGameInfo?.Validate();
    }
    void Start()
    {
        
    }
    public void StartFishGame()
    {
        if(GameManagement.Instance.GetNbDayPassed() >= fishGameInfo.GetNbGame())
        {
            Debug.LogError("No info for this nb of Game");
            return;
        }
        
        if(GameManagement.Instance.isFishGameFinished) return;
        GameManagement.Instance.isFishGameFinished = true;
        fishGamePlace.SetActive(true);
        workPlace.SetActive(false);

        currentFishNb = fishGameInfo.GetNbOfFishPerGame(GameManagement.Instance.GetNbDayPassed());
        RandomFishes();
        ShowFirstFish();
    }
    private void ShowFirstFish()
    {
        fishPlace.GetComponent<SpriteRenderer>().sprite = currentFishes[currentFishIndex].GetSprite();
        // currentFishIndex++;
    }
    
    public void NextFish(){
        if(isAnimatingOut) return;
        //TODO: mutation check
        fishPlace.GetComponent<Animator>().SetTrigger("fish_out");
        isAnimatingOut = true;
        currentFishIndex++;
    }
    public void ShowNewFish()
    {
        isAnimatingOut = false;
        if(currentFishIndex >= currentFishes.Count)
        {
            Debug.Log("No more fish to show");
            //TODO: score fish game
            fishGamePlace.SetActive(false);
            workPlace.SetActive(true);
            return;
        }
        fishPlace.GetComponent<SpriteRenderer>().sprite = currentFishes[currentFishIndex].GetSprite();
    }
    
    private void RandomFishes()
    {
        currentFishes = new List<Fish>();
        currentFishIndex = 0;
        for(int i = 0; i<currentFishNb; i++)
        {
            bool isMutate = IsFishMutated(fishGameInfo.GetPercentageMutatedFishes(GameManagement.Instance.GetNbDayPassed()));
            if (isMutate)
            {
                int randomIndex = UnityEngine.Random.Range(0, fishGameInfo.GetTotalFishType());
                FishTypeInfo randomFishType = fishGameInfo.GetFishInfo(randomIndex);
                int randomFishMutatedIndex = UnityEngine.Random.Range(0, randomFishType.mutatedFishes.Length);
                Sprite mutatedFish = randomFishType.mutatedFishes[randomFishMutatedIndex];
                currentFishes.Add(new Fish(mutatedFish, isMutate));
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, fishGameInfo.GetTotalFishType());
                FishTypeInfo randomFishType = fishGameInfo.GetFishInfo(randomIndex);
                int randomFishNormalIndex = UnityEngine.Random.Range(0, randomFishType.normalFishes.Length);
                Sprite normalFish = randomFishType.normalFishes[randomFishNormalIndex];
                currentFishes.Add(new Fish(normalFish, isMutate));
            }
        }
    }
    private bool IsFishMutated(float mutationRate)
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        return roll < mutationRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    
}
