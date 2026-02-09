using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FishManagement : MonoBehaviour
{
    public static FishManagement Instance {get; private set;}
    
    [SerializeField] private FishGameInfo fishGameInfo;
    [SerializeField] private TextMeshProUGUI fishCorrectText;
    
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
    public Fish GetCurrentFish()
    {
    if (currentFishes == null || currentFishes.Count == 0)
        return null;

    if (currentFishIndex < 0 || currentFishIndex >= currentFishes.Count)
        return null;

    return currentFishes[currentFishIndex];
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
       /*  if(currentFishes[currentFishIndex].GetIsMutated() && isMutated)
        {
            fishCorrectText.color = Color.green;
            fishCorrectText.text = "Correctly identified mutated fish";
            Debug.Log("Correctly identified mutated fish");
        }
        else if(!currentFishes[currentFishIndex].GetIsMutated() && !isMutated)
        {
            fishCorrectText.color = Color.green;
            fishCorrectText.text = "Correctly identified normal fish";
            Debug.Log("Correctly identified normal fish");
        }
        else
        {
            fishCorrectText.color = Color.red;
            fishCorrectText.text = "Incorrectly identified fish";
            Debug.Log("Incorrectly identified fish");
        } */
        //TODO: mutation check
        fishPlace.GetComponent<Animator>().SetTrigger("fish_out");
        isAnimatingOut = true;
        currentFishIndex++;
    }
    public void ShowNewFish()
    {
        isAnimatingOut = false;
        if (fishCorrectText) fishCorrectText.text = "";
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
