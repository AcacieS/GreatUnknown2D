using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class FishManagement : MonoBehaviour
{
    public static FishManagement Instance {get; private set;}
    
    [SerializeField] private FishDaysInfo fishDaysInfo;
    [SerializeField] private TextMeshProUGUI fishCorrectText;
    
    [Header("Places")]
    [SerializeField] private GameObject fishPlace;
    [SerializeField] private GameObject fishShowPrefab;
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGamePlace;
    [Header("Mutation")]
    [SerializeField] private FishSession session;
    [SerializeField] private TextMeshProUGUI fishTxt;

    [Header("Show for Debug")]

    [SerializeField] List<Fish> currentFishLists = new List<Fish>();
    [SerializeField] int currentFishIndex = -1;
    private int currentFishNb = 0;
    private FishDayInfo currentFishDayInfo;
    private bool isAnimatingOut = false;
    private int currentDay = 0;
    public void ResetFishGame()
    {
        DestroyPreviousFish();
        currentFishIndex = -1;
        currentFishLists.Clear();
        fishTxt.text = "";
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
        currentDay = GameManagement.Instance.GetNbDayPassed();
        if(currentDay>= fishDaysInfo.GetNbGame() || GameManagement.Instance.isFishGameFinished)
        {
            return;
        }
        currentFishDayInfo = fishDaysInfo.GetCurrentFishDayInfo(currentDay);
        GameManagement.Instance.isFishGameFinished = true;

        fishGamePlace.SetActive(true);
        workPlace.SetActive(false);
        RandomFishes();
        fishPlace.GetComponent<FishState>().ResetFishGame();
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
    
    public bool GetIsAnimating()
    {
        return isAnimatingOut;
    }

    public void ClickIsMutated()    => HandleChoice(true);
    public void ClickIsNotMutated() => HandleChoice(false);

    private void HandleChoice(bool playerSaysMutated)
    {
        if (session == null||GetIsAnimating()) return;

        Fish currentFish = GetCurrentFish();
        if (currentFish == null) return;

        bool correct = (currentFish.isMutated == playerSaysMutated);

        if (correct) {
            session.AddCorrect();
            //DO NOTHING
            fishTxt.color = Color.green;
            fishTxt.text = "Correctly identified fish";
        }
        else{
            //
            fishTxt.color = Color.red;
            fishTxt.text = "Incorrectly identified fish";
            session.AddWrong();
            if(session.Wrong >= 3)
            {
                GameManagement.Instance.ResetDay();
                return;
            }
        }

        fishPlace.GetComponent<FishState>().NextFish();
    }
    // public void NextFish(){
    //     //if(isAnimatingOut) return;
    //     //fishPlace.GetComponent<Animator>().SetTrigger("fish_out");
    //     //isAnimatingOut = true;
    // }
    
    private void RandomFishes()
    {
        //Initialize
        currentFishLists = new List<Fish>();
        currentFishIndex = -1;
        currentFishNb = fishDaysInfo.GetNbOfFishPerGame(currentDay);

        //add fishes
        for(int i = 0; i<currentFishNb; i++)
        {
            //======= want mutated or not
            bool isMutate = fishDaysInfo.GetIsMutated(currentDay);

            //======= fish type
            int randomFishTypeIndex = Random.Range(0, fishDaysInfo.GetTotalFishType());

            FishTypeInfo fishType = fishDaysInfo.GetFishInfo(randomFishTypeIndex);
            Fish newFish = new Fish(fishType,isMutate);
            currentFishLists.Add(newFish);

            //======= searching for what bodyPart
            int nbMutationCurrentFish = 0;
            if (isMutate)
            {
                nbMutationCurrentFish = Mathf.Min(fishDaysInfo.GetRandomNbMutationPerFish(currentDay), fishType.GetNbPossibleMutation());
            }

            ChooseBodyParts(fishType, newFish, nbMutationCurrentFish);
        }
    }
    private void ChooseBodyParts(FishTypeInfo fishType, Fish newFish, int nbMutationCurrentFish)
    {
        //======= nb of mutation prepare
        List<int> mutatedFishIndex = new List<int>();
        int currentMutatedFishIndex = -1; // if no mutation: -1
        for(int i=0; i<nbMutationCurrentFish; i++)
        {
            ChooseMutatedFish(fishType, newFish, mutatedFishIndex, nbMutationCurrentFish);
            currentMutatedFishIndex = 0;
        }
        mutatedFishIndex.Sort();


        //======== getting body part
        for(int i=0; i< fishType.categoriesFishLayer.Count; i++)
        {
            // IS MUTATED: SKIP
            if(currentMutatedFishIndex>=0 && currentMutatedFishIndex < mutatedFishIndex.Count && mutatedFishIndex[currentMutatedFishIndex]==i)
            {
                currentMutatedFishIndex++; //already assigned
            }
            else //IS NORMAL
            {
                CategoryFishBodyPart fishPartCat = fishType.categoriesFishLayer[i];
                FishBodyPart naturalBodyPartChoosen = ChooseNaturalFishBodyPart(fishPartCat);
                if (naturalBodyPartChoosen)
                {
                    newFish.AddFishBodyPart(fishPartCat, naturalBodyPartChoosen);
                }
            }
            
        }
    }
    private FishBodyPart ChooseNaturalFishBodyPart(CategoryFishBodyPart fishPartCat)
    {
        bool isNormalBase = currentFishDayInfo.GetIsBaseNormal();
        if (isNormalBase || fishPartCat.GetNbPossibleSpecialNormal() == 0)
        {
            return fishPartCat.baseBodyPartFish;
        }
        //========= if is Special Normal
        //----- Difficulty
        List<FishBodyPart> fishBodyParts;
        FishBodyPartDifficultyList fishBodyPartsDifficultyList = fishPartCat.fishPartsNormal;
        if (fishBodyPartsDifficultyList.GetNbFishParts(FishBPDifficulty.Easy) == 0)
        {
            fishBodyParts = fishBodyPartsDifficultyList.GetFishParts(FishBPDifficulty.Hard);
        }else if(fishBodyPartsDifficultyList.GetNbFishParts(FishBPDifficulty.Hard) == 0)
        {
            fishBodyParts = fishBodyPartsDifficultyList.GetFishParts(FishBPDifficulty.Easy);
        }
        else
        {
            FishBPDifficulty difficulty = currentFishDayInfo.GetIsNormalEasy();
            fishBodyParts = fishPartCat.fishPartsNormal.GetFishParts(difficulty);
        }

        FishBodyPart fishBodyPart = fishBodyParts[Random.Range(0, fishBodyParts.Count)];
        return fishBodyPart;
    }
    
    private void ChooseMutatedFish(FishTypeInfo fishType, Fish newFish, List<int> mutatedFishIndex, int nbMutationCurrentFish)
    {
        // ======== Choose Body Type Category
        int randMutatedPartCatIndex = 0;
        do 
        {
            randMutatedPartCatIndex = UnityEngine.Random.Range(0, fishType.categoriesFishLayer.Count); 
        }
        while (mutatedFishIndex.Contains(randMutatedPartCatIndex)|| !fishType.categoriesFishLayer[randMutatedPartCatIndex].HasMutation()); 
        
        mutatedFishIndex.Add(randMutatedPartCatIndex);

        //======== Choose part of the category?
        CategoryFishBodyPart mutatedPartCat = fishType.categoriesFishLayer[randMutatedPartCatIndex];
        List<FishBodyPart> fishBodyPartsDifficulty;
        //in case only 1 mutation: so no need check Difficulty TODO-> need every part if has mutation: have easy, hard
        if(IsOnlyOneDifficultyPossible(mutatedPartCat, mutatedPartCat.fishPartsMutated))
        {
            //found that one
            if (mutatedPartCat.fishPartsMutated.GetNbFishParts(FishBPDifficulty.Easy) > 0)
            {
                //random inside
                fishBodyPartsDifficulty = mutatedPartCat.fishPartsMutated.GetFishParts(FishBPDifficulty.Easy);
            }
            else //hard one
            {
                fishBodyPartsDifficulty = mutatedPartCat.fishPartsMutated.GetFishParts(FishBPDifficulty.Hard);
            }
        }
        else
        {
            FishBPDifficulty difficulty = currentFishDayInfo.GetIsMutationEasy(nbMutationCurrentFish);
            fishBodyPartsDifficulty = mutatedPartCat.fishPartsMutated.GetFishParts(difficulty);
        }
        
        
        int randMutatedPartIndex = UnityEngine.Random.Range(0, fishBodyPartsDifficulty.Count); 
        FishBodyPart mutatedPart = fishBodyPartsDifficulty[randMutatedPartIndex];
        
        newFish.AddFishBodyPart(mutatedPartCat, mutatedPart);
    }

    private bool IsOnlyOneDifficultyPossible(CategoryFishBodyPart partCat, FishBodyPartDifficultyList fishBodyPartsDifficultyList)
    {
        if(partCat.GetNbPossibleMutation() == 1) return true;
        if(partCat.fishPartsMutated.GetNbFishParts(FishBPDifficulty.Easy) == 0 || partCat.fishPartsMutated.GetNbFishParts(FishBPDifficulty.Hard) == 0) return true;
        return false;
    }
    
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    private void OnValidate()
    {
        // fishGameInfo.?.Validate();
    }
    void Start()
    {
    }
    
}
