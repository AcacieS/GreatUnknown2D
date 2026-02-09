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
    [SerializeField] private GameObject fishShowPrefab;
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGamePlace;

    [Header("Show for Debug")]

    [SerializeField] List<Fish> currentFishLists = new List<Fish>();
    [SerializeField] List<Fish> currentFishList = new List<Fish>();
    private int currentFishNb = 0;
    [SerializeField] int currentFishIndex = -1;
    private bool isAnimatingOut = false;
    
    private void OnValidate()
    {
        fishGameInfo?.Validate();
    }
    void Start()
    {
        // currentFishList.Add(new Fish(fishGameInfo.))
    }
    
    public Fish GetCurrentFish()
    {
    if (currentFishLists == null || currentFishLists.Count == 0)
        return null;

    if (currentFishIndex < 0 || currentFishIndex >= currentFishLists.Count)
        return null;

    return currentFishLists[currentFishIndex];
    }

    public void StartFishGame()
    {
        if(GameManagement.Instance.GetNbDayPassed() >= fishGameInfo.GetNbGame() || GameManagement.Instance.isFishGameFinished)
        {
            return;
        }
        GameManagement.Instance.isFishGameFinished = true;

        fishGamePlace.SetActive(true);
        workPlace.SetActive(false);
        currentFishNb = fishGameInfo.GetNbOfFishPerGame(GameManagement.Instance.GetNbDayPassed());
        RandomFishes();
        InitializeNewFish();
    }

    //============================================ SHOW CURRENT FISH =========================================
    public void InitializeNewFish()
    {
        Debug.Log("=================== Initialize new fish");
        isAnimatingOut = false;
        fishCorrectText.text = "";
        Debug.Log("destroy");
        DestroyPreviousFish();

        currentFishIndex++;
        Debug.Log("currentFish Index"+currentFishIndex);
        Debug.LogWarning("count: "+currentFishLists.Count);
        if(currentFishIndex >= currentFishLists.Count)
        {
            Debug.Log("No more fish to show");
            //TODO: score fish game
            fishGamePlace.SetActive(false);
            workPlace.SetActive(true);
            return;
        }
        
        Fish currentFish = currentFishLists[currentFishIndex];
        FishTypeInfo fishTypeInfo = currentFish.GetFishType();

        // //assign basic object
        // fishPlace.GetComponent<SpriteRenderer>().sprite = fishTypeInfo.fishBasicBody;
        foreach (KeyValuePair<CategoryFishBodyPart, FishBodyPart> fishBodyPart in currentFish.GetFishBodyParts())
        {
            CategoryFishBodyPart categoryFishBody = fishBodyPart.Key;
            FishBodyPart fishBody = fishBodyPart.Value;

            GameObject newFishBodyGO = Instantiate(fishShowPrefab,
            fishPlace.transform.position,
            fishPlace.transform.rotation,
            fishPlace.transform); //maybe 0

            newFishBodyGO.GetComponent<SpriteRenderer>().sprite = fishBody.bodyPartSprite;
            newFishBodyGO.GetComponent<SpriteRenderer>().sortingLayerName = categoryFishBody.sortingLayer.ToString();
            currentFish.AddFishBodyPartGameObj(newFishBodyGO);
        }
    }
    public void DestroyPreviousFish()
    {
        Debug.LogWarning("count in destroy: "+currentFishLists.Count);
        if(currentFishIndex < 0) return;
        Fish previousFish = currentFishLists[currentFishIndex];
        foreach(GameObject previousFishBodyPartsGO in previousFish.GetFishBodyPartsGameObj())
        {
            Destroy(previousFishBodyPartsGO);
        }
        Debug.LogWarning("count after destroy: "+currentFishLists.Count);
    }
    
    
    public void NextFish(bool isMutated){
        if(isAnimatingOut) return;
        if(currentFishLists[currentFishIndex].GetIsMutated() && isMutated)
        {
            fishCorrectText.color = Color.green;
            fishCorrectText.text = "Correctly identified mutated fish";
            Debug.Log("Correctly identified mutated fish");
        }
        else if(!currentFishLists[currentFishIndex].GetIsMutated() && !isMutated)
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
        }
        //TODO: mutation check
        fishPlace.GetComponent<Animator>().SetTrigger("fish_out");
        isAnimatingOut = true;
    }
    
    private void RandomFishes()
    {
        Debug.LogWarning("+++++++++++++++ Random Fishes");
        //Initialize
        currentFishLists = new List<Fish>();
        currentFishIndex = -1;

        //add fishes
        for(int i = 0; i<currentFishNb; i++)
        {
            //want mutated or not
            bool isMutate = IsFishMutated(fishGameInfo.GetPercentageMutatedFishes(GameManagement.Instance.GetNbDayPassed()));

            //fish type
            int randomIndex = UnityEngine.Random.Range(0, fishGameInfo.GetTotalFishType());
            FishTypeInfo fishType = fishGameInfo.GetFishInfo(randomIndex);
            Fish newFish = new Fish(fishType,isMutate);
            currentFishLists.Add(newFish);
            Debug.Log("fish added of currentFishList: "+newFish);

            //ss
            //TODO: many mutated example, but not for now
            //TODO: for now is equivalent chance of bodypart;
            int nbMutated = 0;
            if (isMutate)
            {
                nbMutated = fishGameInfo.GetRandomNbMutationPerFish(GameManagement.Instance.GetNbDayPassed());
            }
            ChooseBodyParts(fishType, newFish, nbMutated);
            
        }
        Debug.LogWarning("count: "+currentFishLists.Count);
    }
    private void ChooseBodyParts(FishTypeInfo fishType, Fish newFish, int nbMutated)
    {
        List<int> mutatedFishIndex = new List<int>();
        int currentMutatedFishIndex = -1;
        for(int i=0; i<nbMutated; i++)
        {
            ChooseMutatedFish(fishType, newFish, mutatedFishIndex);
            currentMutatedFishIndex = 0;
        }
        mutatedFishIndex.Sort();
        
        for(int i=0; i< fishType.categoriesFishLayer.Count; i++)
        {
            if(currentMutatedFishIndex>=0 && currentMutatedFishIndex < mutatedFishIndex.Count)
            {
                Debug.LogWarning("mutatedFishIndex[currentMutatedFishIndex]: "+mutatedFishIndex[currentMutatedFishIndex]);
            }
            if(currentMutatedFishIndex>=0 && currentMutatedFishIndex < mutatedFishIndex.Count && mutatedFishIndex[currentMutatedFishIndex]==i)
            {
                currentMutatedFishIndex++;
            }
            else
            {
                CategoryFishBodyPart fishPartCat = fishType.categoriesFishLayer[i];
                if (fishPartCat.baseBodyPartFish) //so if not optional
                {
                    //TODO: how to made possible different normal?
                    newFish.AddFishBodyPart(fishPartCat, fishPartCat.baseBodyPartFish);
                }
                else
                {
                    //TODO: if optional when do we add it?
                }
            }
            
        }
    }
    
    private void ChooseMutatedFish(FishTypeInfo fishType, Fish newFish, List<int> mutatedFishIndex)
    {
        int randMutatedPartCatIndex = 0;
        do 
        {
            randMutatedPartCatIndex = UnityEngine.Random.Range(0, fishType.categoriesFishLayer.Count); 
        }
        while (mutatedFishIndex.Contains(randMutatedPartCatIndex)|| fishType.categoriesFishLayer[randMutatedPartCatIndex].fishPartsMutated.Count == 0); 
        mutatedFishIndex.Add(randMutatedPartCatIndex);
        CategoryFishBodyPart mutatedPartCat = fishType.categoriesFishLayer[randMutatedPartCatIndex];
        int randMutatedPartIndex = UnityEngine.Random.Range(0, mutatedPartCat.fishPartsMutated.Count); 
        FishBodyPart mutatedPart = mutatedPartCat.fishPartsMutated[randMutatedPartIndex];
        newFish.AddFishBodyPart(mutatedPartCat, mutatedPart);
    }
    private void ChooseNormalFish()
    {
        
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
