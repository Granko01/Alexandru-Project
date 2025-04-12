using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class LevelLogic : MonoBehaviour
{
    [Header("Ints")]
    public float Amount = 10;
    private int currentTreeStage = 0;

    [Header("Texts")]
    public Text AmountText;

    [Header("GameObjects")]
    public GameObject[] Bets;
    public GameObject Take;
    public GameObject[] TreeStages;
    public GameObject[] InsideStages;

    [Header("Buttons")]
    public Button StartBtn;
    public Button[] Btn;

    public bool canPlay = false;

    public StagesScript stagesScript;
    public GameCoinManager coinManager;

    void Start()
    {
        AmountText.text = Amount.ToString();
        if (Amount == 0)
        {
            StartBtn.interactable = false;
        }
    }

    void Update()
    {
        
    }

    public void BetsM(string tag)
    {
        if (tag == "Increase")
        {
            if (Amount > 14)
            {
                Amount = 15;
                return;
            }
            Amount++;
            AmountText.text = Amount.ToString();          
        }
        else if (tag == "Decrease")
        {
            if (Amount < 6)
            {
                Amount = 5;
                return;
            }
            Amount--;
            AmountText.text = Amount.ToString();
        }
        if (Amount > 0)
        {
            StartBtn.interactable = true;
        }
        else
        {
            StartBtn.interactable = false;
        }
    }



    public void StartGame()
    {
        for (int i = 0; i < Btn.Length; i++)
        {
            Btn[i].interactable = false;
        }
        Take.gameObject.SetActive(true);
        canPlay = true;
        stagesScript.canPlay = canPlay;
        coinManager.Energy--;
        coinManager.SaveEnergy();
        stagesScript.cashOut.text = Amount.ToString();
        Debug.Log("Can play" + stagesScript.canPlay);
    }

}
