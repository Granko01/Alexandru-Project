using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StagesScript : MonoBehaviour
{
    public GameObject[] logic;
    public int currentTreeStage = 1;
    public int currentRow = 3;
    public int currentImg = 1;
    public GameObject[] TreeStages;
    public GameObject[] Treerow;
    public bool hasExecuted = false;
    public GameObject RowHolder;

    public Text cashOut;

    public float fadeDuration = 1f;

    public int Buttonpress;

    public bool canPlay;

    public int ProgressCount = 2;
    public GameObject[] Imgtoshow;

    public LevelLogic levelLogic;
    public GameCoinManager gameCoinManager;

    public Button cashOutBtn;

    public int firstBet = 1;

    public BranchManager branchManager;

    public GameObject shop;

    public GameObject[] ProgressBar;

    public GameObject LosePanel;

    public int FirstBranchCoinWin = 1;

    public Text Multiplier;

    public Animator anim;

    public int ActivateCount = 1;


    private void Start()
    {
        ShuffleLogic();
        UpdateButtonInteractivity();
        GetButtonStates();
    }

    public void ShuffleLogic()
    {
        string[] requiredTags = { "Progress", "Gameover" };
        if (Random.value > 0.5f)
        {
            string temp = requiredTags[0];
            requiredTags[0] = requiredTags[1];
            requiredTags[1] = temp;
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject parent = logic[i];
            int childCount = parent.transform.childCount;
            if (childCount == 0) continue;

            for (int j = 0; j < childCount; j++)
                parent.transform.GetChild(j).gameObject.SetActive(false);

            for (int j = 0; j < childCount; j++)
            {
                Transform child = parent.transform.GetChild(j);
                if (child.tag == requiredTags[i])
                {
                    child.gameObject.SetActive(true);
                    parent.tag = child.tag;
                    Debug.Log($"Force-assigned {requiredTags[i]} to {parent.name}, index: {j}");
                    break;
                }
            }
        }

        for (int i = 2; i < logic.Length; i++)
        {
            GameObject parent = logic[i];
            int childCount = parent.transform.childCount;
            if (childCount == 0) continue;

            for (int j = 0; j < childCount; j++)
                parent.transform.GetChild(j).gameObject.SetActive(false);

            int randomIndex = Random.Range(0, childCount);
            Transform activeChild = parent.transform.GetChild(randomIndex);
            activeChild.gameObject.SetActive(true);

            parent.tag = activeChild.tag;
            Debug.Log($"Shuffled {parent.name}, activated child index: {randomIndex}, tag set to: {parent.tag}");
        }
    }

    public void CheckStages(GameObject pressedButton)
    {
        if (canPlay)
        {
            if (hasExecuted) return;
            if (!pressedButton) return;

            string buttonTag = pressedButton.tag;

            foreach (GameObject logicParent in logic)
            {
                foreach (Transform child in logicParent.transform)
                {
                    if (logicParent.gameObject == pressedButton && logicParent.gameObject.activeInHierarchy)
                    {
                        Debug.Log($"Button pressed with tag: {buttonTag}");

                        if (buttonTag == "Progress")
                        {
                            Buttonpress++;
                            if (Buttonpress == 1)
                            {
                                if (levelLogic.Amount >= 5 || levelLogic.Amount <= 13)
                                {
                                    Debug.Log(levelLogic.Amount + " Amount");
                                    if (currentRow < 6 && currentRow >= 0)
                                    {
                                        levelLogic.Amount *= 1.75f;
                                        Multiplier.text = "X1.75";
                                        anim.SetBool("play", true);
                                        Debug.Log("Is joining");
                                    }
                                    else if (currentRow >= 6)
                                    {
                                        levelLogic.Amount *= 2.25f;
                                        Multiplier.text = "X2.25";
                                        anim.SetBool("play", true);
                                    }
                                }
                                else if (levelLogic.Amount > 13 || levelLogic.Amount <= 15)
                                {
                                    if (currentRow < 6)
                                    {
                                        levelLogic.Amount *= 1.5f;
                                        Multiplier.text = "X1.5";
                                        anim.SetBool("play", true);
                                    }
                                    else if (currentRow >= 6)
                                    {
                                        levelLogic.Amount *= 1.85f;
                                        Multiplier.text = "X1.85";
                                        anim.SetBool("play", true);
                                    }
                                }
                                cashOut.text = levelLogic.Amount.ToString();
                                StartCoroutine(HandleStageProgressWithDelay());
                                if (currentRow == 6 && FirstBranchCoinWin == 1)
                                {
                                    if (SceneManager.GetActiveScene().name == "Branch1")
                                    {
                                        gameCoinManager.FirstBranchCoinAmount++;
                                        gameCoinManager.FirstBranchCoinText[0].text = gameCoinManager.FirstBranchCoinAmount.ToString();
                                        gameCoinManager.FirstBranchCoinText[1].text = gameCoinManager.FirstBranchCoinAmount.ToString();
                                        gameCoinManager.SaveFirstBranchCoin();
                                        FirstBranchCoinWin++;
                                    }
                                    else if (SceneManager.GetActiveScene().name == "Branch2")
                                    {
                                        gameCoinManager.SecondBranchCoinAmount++;
                                        gameCoinManager.SecondBranchCoinText[0].text = gameCoinManager.SecondBranchCoinAmount.ToString();
                                        gameCoinManager.SaveSecondBranchCoin();
                                        FirstBranchCoinWin++;
                                    }
                                }
                            }
                        }
                        else if (buttonTag == "Regress")
                        {
                            Debug.Log("Regress found");
                            Buttonpress++;
                            if (Buttonpress == 1)
                            {
                                if (levelLogic.Amount >= 5 || levelLogic.Amount <= 13)
                                {
                                    Debug.Log(levelLogic.Amount + " Amount");
                                    if (currentRow < 6 && currentRow >= 0)
                                    {
                                        levelLogic.Amount /= 1.75f;
                                        Multiplier.text = "/1.75";
                                        anim.SetBool("play", true);
                                        Debug.Log("Is joining");
                                    }
                                    else if (currentRow >= 6)
                                    {
                                        levelLogic.Amount /= 2.25f;
                                        Multiplier.text = "/2.25";
                                        anim.SetBool("play", true);
                                    }
                                }
                                else if (levelLogic.Amount > 13 || levelLogic.Amount <= 15)
                                {
                                    if (currentRow < 6)
                                    {
                                        levelLogic.Amount /= 1.5f;
                                        Multiplier.text = "/1.5";
                                        anim.SetBool("play", true);
                                    }
                                    else if (currentRow >= 6)
                                    {
                                        levelLogic.Amount /= 1.85f;
                                        Multiplier.text = "/1.85";
                                        anim.SetBool("play", true);
                                    }
                                }
                                StartCoroutine(HandleStageRegressWithDelay());
                                
                                Vector3 pos = RowHolder.transform.position;
                                pos.y -= 160f;
                                RowHolder.transform.position = pos;
                            }
                        }

                        else if (buttonTag == "Gameover")
                        {
                            Debug.Log("Gameover found");
                            StartCoroutine(GameOverWait());
                        }

                        hasExecuted = true;
                        return;
                    }
                }
            }

            Debug.Log("Button not found");
        }
        else
        {
            Debug.Log("Bet first");
        }
    }
    private IEnumerator HandleStageProgressWithDelay()
    {
        RevealAllImages();
        ShowNextProgressBar();

        yield return new WaitForSeconds(2f);

        anim.SetBool("play", false);
        Multiplier.text = "";

        foreach (GameObject parent in logic)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Image img = child.GetComponent<Image>();
                    if (img != null)
                    {
                        img.enabled = false;
                    }
                }
            }
        }

        TreeStageProgress();

        Vector3 pos = RowHolder.transform.position;
        pos.y += 160f;
        RowHolder.transform.position = pos;
    }

    private IEnumerator HandleStageRegressWithDelay()
    {
        RevealAllImages(false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("play", false);
        Multiplier.text = "";

        foreach (GameObject parent in logic)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Image img = child.GetComponent<Image>();
                    if (img != null)
                    {
                        img.enabled = false;
                    }
                }
            }
        }
        TreeStageRegress();
    }

    private IEnumerator GameOverWait()
    {
        RevealAllImages();
        LosePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    int progressBarCount = 0;

    public void ShowNextProgressBar()
    {
        if (progressBarCount < ProgressBar.Length)
        {
            ProgressBar[progressBarCount].SetActive(true);
            progressBarCount++;
        }
    }

    public void DecreaseNextProgressBar()
    {
        progressBarCount--;
        if (progressBarCount < ProgressBar.Length)
        {
            ProgressBar[progressBarCount].SetActive(false);
        }
    }




    public int currentRevealIndex = 0;
    public List<int> revealIndicesHistory = new List<int>();


    private void RevealAllImages(bool addToHistory = true)
    {
        List<GameObject> activeChildren = new List<GameObject>();

        foreach (GameObject parent in logic)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    activeChildren.Add(child.gameObject);
                }
            }
        }

        int revealCount = 2 + revealIndicesHistory.Count;
        int nextRevealEnd = Mathf.Min(currentRevealIndex + revealCount, activeChildren.Count);

        for (int i = currentRevealIndex; i < nextRevealEnd; i++)
        {
            Image img = activeChildren[i].GetComponent<Image>();
            if (img != null)
            {
                img.enabled = true;
            }
        }

        if (addToHistory)
            revealIndicesHistory.Add(currentRevealIndex);

        currentRevealIndex = nextRevealEnd;
    }





    void TreeStageProgress()
    {
        if (currentTreeStage < TreeStages.Length - 1)
        {
            currentRow = currentTreeStage;
            Treerow[currentRow].gameObject.SetActive(false);
            StartCoroutine(FadeOut(TreeStages[currentTreeStage]));

            currentTreeStage++;
            Debug.Log($"Tree progressed to stage: {currentTreeStage}");

            StartCoroutine(FadeIn(TreeStages[currentTreeStage]));
            currentRow = currentTreeStage + 2;
            Treerow[currentRow].gameObject.SetActive(true);

            ProgressCount++;
        }
        else
        {
            Debug.Log("Tree is fully grown!");
        }

        currentRow = currentTreeStage;
        UpdateButtonInteractivity();
        ShuffleLogic();
        GetButtonStates();
        ActivateCount--;
    }

    void TreeStageRegress()
    {
        if (currentTreeStage > 0)
        {
            currentRow = currentTreeStage + 2;
            Treerow[currentRow].gameObject.SetActive(false);
          
            StartCoroutine(FadeOut(TreeStages[currentTreeStage]));



            currentTreeStage--;
            Debug.Log($"Tree regressed to stage: {currentTreeStage}");

            StartCoroutine(FadeIn(TreeStages[currentTreeStage]));
            currentRow = currentTreeStage;
            Treerow[currentRow].gameObject.SetActive(true);

            currentRevealIndex = 0;
        }
        else
        {
            Debug.Log("Tree is at the smallest stage!");
        }

       
        ShuffleLogic();
        currentRow = currentTreeStage;
        currentRevealIndex = revealIndicesHistory.Count > 0 ? revealIndicesHistory[^1] : 0;
        revealIndicesHistory.RemoveAt(revealIndicesHistory.Count - 1);
        DecreaseNextProgressBar();
        UpdateButtonInteractivity();
        anim.SetBool("play", false);
        ActivateCount--;
        GetButtonStates();
    }


    IEnumerator FadeOut(GameObject treeStage)
    {
        CanvasGroup canvasGroup = treeStage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = treeStage.AddComponent<CanvasGroup>();

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = 1 - (timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        treeStage.SetActive(false);
    }

    IEnumerator FadeIn(GameObject treeStage)
    {
        CanvasGroup canvasGroup = treeStage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = treeStage.AddComponent<CanvasGroup>();

        treeStage.SetActive(true);

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = timeElapsed / fadeDuration; 
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        Buttonpress = 0;
        hasExecuted = false;
    }
    private void UpdateButtonInteractivity()
    {
        for (int i = 0; i < Treerow.Length; i++)
        {
            bool isCurrentRow = (i == currentRow);
            Button[] buttons = Treerow[i].GetComponentsInChildren<Button>(true);

            foreach (Button btn in buttons)
            {
                btn.interactable = isCurrentRow;
            }
        }
    }




    IEnumerator GoHome()
    {
        Debug.Log("Finished");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShopMethod()
    {
        if (!shop.activeInHierarchy)
        {
            shop.gameObject.SetActive(true);
            return;
        }
        shop.gameObject.SetActive(false);
    }
    public GameObject ShopBTN;
    public void ShopButton()
    {
        Button shopButton = ShopBTN.GetComponent<Button>();
        if (shopButton != null)
        {
            shopButton.interactable = false;
        }
    }
    public void CashOut()
    {
        gameCoinManager.Coins += (int)levelLogic.Amount;
        cashOutBtn.gameObject.SetActive(false);
        gameCoinManager.CoinText.text = ((int)gameCoinManager.Coins).ToString();
        gameCoinManager.SaveCoin();
        StartCoroutine(GoHome());
    }
    
    public void ActivateSafeChoice()
    {
       
        if (gameCoinManager.SafeChoiceAmount >= 1)
        {
            int ActiavePerRound = ActivateCount;
            if (ActiavePerRound == 1)
            {
                foreach (GameObject row in logic)
                {
                    row.tag = "Progress";
                }
                SideMethodToSpendSafeChoice();
                ActivateCount++;
            }
            else
            {
                Debug.Log("You already activated");
            }
        }
        else
        {
            Debug.Log("No safe choice coins left");
        }
      
    }

    public void SideMethodToSpendSafeChoice()
    {
        gameCoinManager.SafeChoiceAmount--;
        gameCoinManager.SafeChoiceText[0].text = gameCoinManager.SafeChoiceAmount.ToString();
        gameCoinManager.SaveSafeChoiceCoin();
    }


    public List<Button> GetButtonStates()
    {
        Button[] buttons = Treerow[currentRow].GetComponentsInChildren<Button>(true);
        List<Button> wrongChoices = new List<Button>();
        Debug.Log(wrongChoices.Count);

        foreach (Button btn in buttons)
        {
            if (btn.gameObject.activeInHierarchy && btn.tag != "Progress")
            {
                wrongChoices.Add(btn);
                Debug.Log(wrongChoices.Count + "After");
            }
        }

        return wrongChoices;
    }

    public void ActivateEliminateOne()
    {
        List<Button> wrongChoices = GetButtonStates();
        if (gameCoinManager.EleminateOneAmount > 0)
        {
            int ActiavePerRound = ActivateCount;
            if (ActiavePerRound == 1)
            {


                if (wrongChoices.Count > 0)
                {
                    foreach (Button wrongButton in wrongChoices)
                    {
                        wrongButton.interactable = false;
                    }
                    gameCoinManager.EleminateOneAmount--;
                    gameCoinManager.EleminateText[0].text = gameCoinManager.EleminateOneAmount.ToString();
                    gameCoinManager.SaveEleminateOneCoin();
                    ActivateCount++;
                    Debug.Log($"Eliminate One activated: Disabled all wrong buttons.");

                    if (wrongChoices.Count == Treerow[currentRow].GetComponentsInChildren<Button>(true).Length)
                    {
                        Debug.Log("All buttons are wrong, progressing to the next stage.");
                        TreeStageProgress();
                    }
                }
            }
            else
            {
                Debug.Log("Throw something");
            }
        }
    }




}
