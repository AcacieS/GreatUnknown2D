using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class FishManagement : MonoBehaviour
{
    public static FishManagement Instance {get; private set;}
    public static bool isFishGameOn;

    [Header("Fish Days")]
    [SerializeField] private FishDaysInfo fishDaysInfo;
    
    [Header("Places")]
    [SerializeField] private GameObject fishPlace;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGamePlace;

    [Header("Mutation")]
    [SerializeField] private FishSession session;
    [SerializeField] private GameObject wrongMessage;
    [SerializeField] private TextMeshProUGUI wrongMessageTxt;
    [SerializeField] private string[] wrongMessageText;

    [Header("Show for Debug")]
    [SerializeField] int currentFishIndex = -1;
    [SerializeField] List<Fish> currentFishLists = new List<Fish>();

    //----------------------------- PRIVATE FIELD --------------------------------    
    private int currentFishNb = 0;
    private FishDayInfo currentFishDayInfo;
    private int currentDay = 0;
    private bool need2Mutated = false;

    //=============================== FISH GAME GENERAL ===========================
    public void ResetFishGame()
    {
        DestroyPreviousFish();
        currentFishIndex = -1;
        currentFishLists.Clear();
        //fishChoiceTxt.text = "";
        isFishGameOn = false;
    }

    public void StartFishGame()
    {
        if(currentDay>= fishDaysInfo.GetNbGame() || GameManagement.Instance.isFishGameFinished)
        {
            GameManagement.Instance.isFishGameFinished = true;
            return;
        }
        //ACTIVATE 
        fishGamePlace.SetActive(true);
        workPlace.SetActive(false);

        if(isFishGameOn)
        {
            return;
        }
        isFishGameOn = true;
        currentDay = GameManagement.Instance.GetNbDayPassed();
        
        currentFishDayInfo = fishDaysInfo.GetCurrentFishDayInfo(currentDay);
        if (currentDay >= 3)
        {
            need2Mutated = true;
        }
        
        RandomFishes();
        fishPlace.GetComponent<FishState>().ResetFishGame();
    }
    

    //============================================ INITIALIZE CURRENT FISH =========================================
    public void InitializeNewFish()
    {
        Debug.Log("=================== Initialize new fish");
        //fishChoiceTxt.text = "";
        Debug.Log("destroy");
        DestroyPreviousFish();
        
        currentFishIndex++;
        Debug.Log("currentFish Index"+currentFishIndex);
        Debug.LogWarning("count: "+currentFishLists.Count);
        if(currentFishIndex >= currentFishLists.Count)
        {
            Debug.Log("No more fish to show");
            //TODO: score fish game
            GameManagement.Instance.isFishGameFinished = true;
            fishGamePlace.SetActive(false);
            workPlace.SetActive(true);
            isFishGameOn = false;
            return;
        }
        
        Fish currentFish = currentFishLists[currentFishIndex];
        FishTypeInfo fishTypeInfo = currentFish.GetFishType();
        
        //CHANGE FISH SHOWING WAY
        fishPlace.GetComponent<BoxCollider2D>().size = fishTypeInfo.colliderInfo.colliderSize;
        fishPlace.GetComponent<BoxCollider2D>().offset = fishTypeInfo.colliderInfo.offsetPos;
        float yPos = currentFish.GetFishType().fishPosInfo.yPos;
        fishPlace.transform.position = new Vector2(fishPlace.transform.position.x, yPos);
        fishPlace.transform.localScale = fishTypeInfo.fishPosInfo.fishSize;

        foreach (KeyValuePair<CategoryFishBodyPart, FishBodyPart> fishBodyPart in currentFish.GetFishBodyParts())
        {
            CategoryFishBodyPart categoryFishBody = fishBodyPart.Key;
            FishBodyPart fishBody = fishBodyPart.Value;

            GameObject newFishBodyGO = Instantiate(fishPrefab,
            fishPlace.transform.position,
            fishPlace.transform.rotation,
            fishPlace.transform); //maybe 0

            newFishBodyGO.GetComponent<SpriteRenderer>().sprite = fishBody.bodyPartSprite;
            newFishBodyGO.GetComponent<SpriteRenderer>().sortingLayerName = categoryFishBody.sortingLayer.ToString();
            currentFish.AddFishBodyPartGameObj(newFishBodyGO);
        }
    }

    private void DestroyPreviousFish()
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


    //============================================ SPAWN NEW FISH =========================================
    
    //------------------------------- GET FISHES -------------------------
    private void RandomFishes()
    {
        //Initialize
        currentFishLists = new List<Fish>();
        currentFishIndex = -1;
        currentFishNb = fishDaysInfo.GetNbOfFishPerGame(currentDay);
        int minNbOfMutatedFish = currentFishDayInfo.GetMinNbMutatedFish();

        //---------- Get which Mutated + make sure more than the min Nb of Mutated Fish;
        List<int> mutatedFishIndexList = new List<int>();
        int mutatedFishIndex = 0;

        while(mutatedFishIndexList.Count < minNbOfMutatedFish)
        {
            mutatedFishIndexList = new List<int>();
            for(int i=0; i<currentFishNb; i++)
            {
                bool isMutate = fishDaysInfo.GetIsMutated(currentDay);
                if (isMutate)
                {
                    mutatedFishIndexList.Add(i);
                }
            }
        }
        
        //------------ Add fish body part
        for(int i = 0; i<currentFishNb; i++)
        {
            bool isMutate = false;
            if(mutatedFishIndex<mutatedFishIndexList.Count && i == mutatedFishIndexList[mutatedFishIndex])
            {
                isMutate = true;
                mutatedFishIndex++;
            }

            //======= fish type
            int randomFishTypeIndex = Random.Range(0, fishDaysInfo.GetTotalFishType());

            FishTypeInfo fishType = fishDaysInfo.GetFishInfo(randomFishTypeIndex);
            

            //======= searching for what bodyPart
            int nbMutationCurrentFish = 0;
            if (isMutate)
            {
                nbMutationCurrentFish = Mathf.Min(fishDaysInfo.GetRandomNbMutationPerFish(currentDay), fishType.GetNbPossibleMutation());
            }
            
            if (nbMutationCurrentFish < 2 && need2Mutated)
            {
                isMutate = false;
            }
            Fish newFish = new Fish(fishType,isMutate);
            currentFishLists.Add(newFish);

            ChooseBodyParts(fishType, newFish, nbMutationCurrentFish);
        }
    }

    //------------------------------- GET BODY PART FISH -------------------------
    private void ChooseBodyParts(FishTypeInfo fishType, Fish newFish, int nbMutationCurrentFish)
    {
        //======= nb of mutation prepare
        List<int> mutatedFishBodyIndex = new List<int>();
        int currentMutatedFishBodyIndex = -1; // if no mutation: -1
        for(int i=0; i<nbMutationCurrentFish; i++)
        {
            ChooseMutatedFishBody(fishType, newFish, mutatedFishBodyIndex, nbMutationCurrentFish);
            currentMutatedFishBodyIndex  = 0;
        }
        mutatedFishBodyIndex.Sort();


        //======== getting body part
        for(int i=0; i< fishType.categoriesFishLayer.Count; i++)
        {
            // IS MUTATED: SKIP
            if(currentMutatedFishBodyIndex >=0 && currentMutatedFishBodyIndex < mutatedFishBodyIndex.Count && mutatedFishBodyIndex[currentMutatedFishBodyIndex]==i)
            {
                currentMutatedFishBodyIndex++; //already assigned
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

    //------------------------------- GET NATURAL/SPECIAL FISH -------------------------
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

    //------------------------------- GET MUTATED FISH -------------------------
    private void ChooseMutatedFishBody(FishTypeInfo fishType, Fish newFish, List<int> mutatedFishIndex, int nbMutationCurrentFish)
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

    //------------------------------- DIFFICULTY -------------------------
    private bool IsOnlyOneDifficultyPossible(CategoryFishBodyPart partCat, FishBodyPartDifficultyList fishBodyPartsDifficultyList)
    {
        if(partCat.GetNbPossibleMutation() == 1) return true;
        if(partCat.fishPartsMutated.GetNbFishParts(FishBPDifficulty.Easy) == 0 || partCat.fishPartsMutated.GetNbFishParts(FishBPDifficulty.Hard) == 0) return true;
        return false;
    }

    //=============================== CHOICE ===========================
    public void ClickIsMutated()    => HandleChoice(true);
    public void ClickIsNotMutated() => HandleChoice(false);

    private void HandleChoice(bool playerSaysMutated)
    {
        Debug.Log("Choice: "+playerSaysMutated);
        if (session == null) return;

        Fish currentFish = GetCurrentFish();
        if (currentFish == null) return;

        bool correct = (currentFish.isMutated == playerSaysMutated);

        if (correct) {
            session.AddCorrect();
            //DO NOTHING
            Debug.Log("Should appear: "+playerSaysMutated);
        }
        else{
            Debug.Log("Should appear: "+playerSaysMutated);
            session.AddWrong();
            if(session.Wrong > 3)
            {
                GameManagement.Instance.ResetDay();
                return;
            }
            else
            {
                wrongMessageTxt.text = wrongMessageText[session.Wrong-1];
                wrongMessage.GetComponent<Animator>().SetTrigger("Wrong");
            }
        }

        fishPlace.GetComponent<FishState>().NextFish();
    }
    
    //=============================== HELPER ===========================
    public Fish GetCurrentFish()
    {
        if (currentFishLists == null || currentFishLists.Count == 0)
            return null;

        if (currentFishIndex < 0 || currentFishIndex >= currentFishLists.Count)
            return null;

        return currentFishLists[currentFishIndex];
    }
    [ContextMenu("Regenerate Fish Body List")]
    void GenerateFish()
    {
        foreach(FishTypeInfo fishTypeInfo in fishDaysInfo.GetFishInfos())
        {
            foreach(CategoryFishBodyPart catFishBodyPart in fishTypeInfo.categoriesFishLayer)
            {
                catFishBodyPart.InitializeFishParts();
            }
        }
    }

    //=============================== OTHER ===========================
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
}
