﻿using System.Collections;
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

    private void Start()
    {
        ShuffleLogic();
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
                                if (firstBet == 1)
                                {
                                    levelLogic.Amount *= 1.5f;
                                    firstBet++;
                                }
                                else
                                {
                                    Debug.Log("Before " + levelLogic.Amount);
                                    levelLogic.Amount *= 1.5f;
                                    Debug.Log("After " + levelLogic.Amount);

                                }
                                cashOut.text = levelLogic.Amount.ToString();
                                StartCoroutine(HandleStageProgressWithDelay());
                            }
                        }
                        else if (buttonTag == "Regress")
                        {
                            Debug.Log("Regress found");
                            Buttonpress++;
                            if (Buttonpress == 1)
                            {
                                TreeStageRegress();                              
                                Vector3 pos = RowHolder.transform.position;
                                pos.y -= 160f;
                                RowHolder.transform.position = pos;
                            }
                        }

                        else if (buttonTag == "Gameover")
                        {
                            Debug.Log("Gameover found");
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

        yield return new WaitForSeconds(3f);

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


    private void RevealAllImages()
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

        currentRow = currentTreeStage;
        ShuffleLogic();
        currentRevealIndex = revealIndicesHistory.Count > 0 ? revealIndicesHistory[^1] : 0;
        revealIndicesHistory.RemoveAt(revealIndicesHistory.Count - 1);
        DecreaseNextProgressBar();
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

    IEnumerator GoHome()
    {
        Debug.Log("Finished");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Branch1");
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

    public void CashOut()
    {
        gameCoinManager.Coins += (int)levelLogic.Amount;
        cashOutBtn.gameObject.SetActive(false);
        gameCoinManager.CoinText.text = ((int)gameCoinManager.Coins).ToString();
        gameCoinManager.SaveCoin();
        StartCoroutine(GoHome());
    }
}
