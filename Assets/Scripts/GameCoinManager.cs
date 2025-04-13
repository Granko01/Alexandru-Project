using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCoinManager : MonoBehaviour
{
    [Header("Int's")]
    public float Coins;
    public int Energy;
    public int maxEnergy = 100;
    public float energyWaitTime;
    public int BlueCircleAmount;
    public int FirstBranchCoinAmount;
    public int SecondBranchCoinAmount;
    public int SafeChoiceAmount = 0;
    public int EleminateOneAmount = 0;


    [Header("Text's")]
    public Text Energytext;
    public Text CoinText;
    public Text Timer;
    public Text[] BlueCirlceText;
    public Text[] FirstBranchCoinText;
    public Text[] SecondBranchCoinText;
    public Text[] SafeChoiceText;
    public Text[] EleminateText;

    private float lastEnergyGivenTime;
    private float timeLeft;

    [Header("Energy Settings")]
    private const string EnergyKey = "Energy";
    private const string LastEnergyTimeKey = "LastEnergyTime";
    private const string CoinKey = "Coins";
    private const string BlueCoinKey = "BlueCoin";
    private const string FirstBranchCoinKey = "FirstBranchCoin";
    private const string SecondBranchCoinKey = "SecondBranchCoin";
    private const string SafeChoiceKey = "SafeChoiceCoin";
    private const string EleminateOneKey = "EleminateOneCoin";


    public GameObject SellPanel;
    public GameObject SellPanel1;
    public GameObject SellPanel2;
    public StagesScript stagesScript;
   

    void Start()
    {
        LoadEnergy();
        LoadCoin();
        LoadBlueCoin();
        LoadFirstBranchCoin();
        LoadSecondBranchCoin();
        LoadSafeChoice();
        LoadEleminateChoice();
        StartCoroutine(EnergyTimer());
        CoinText.text = ((int)Coins).ToString();
        Energytext.text = Energy.ToString();
        BlueCirlceText[0].text = BlueCircleAmount.ToString();
        BlueCirlceText[1].text = BlueCircleAmount.ToString();
        FirstBranchCoinText[0].text = FirstBranchCoinAmount.ToString();
        SecondBranchCoinText[0].text = SecondBranchCoinAmount.ToString();
        EleminateText[0].text = EleminateOneAmount.ToString();
        SafeChoiceText[0].text = SafeChoiceAmount.ToString();
    }

    void Update()
    {
        if (Energy < maxEnergy)
        {
            timeLeft = energyWaitTime - (Time.time - lastEnergyGivenTime);
            if (timeLeft < 0) timeLeft = 0;

            TimeSpan time = TimeSpan.FromSeconds(timeLeft);
            Timer.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }
        else
        {
            Timer.text = "00:00";
        }

        Energytext.text = Energy.ToString();
    }

    void LoadEnergy()
    {
        Energy = PlayerPrefs.GetInt(EnergyKey, 50);
        string lastTime = PlayerPrefs.GetString(LastEnergyTimeKey, "");

        if (!string.IsNullOrEmpty(lastTime))
        {
            DateTime lastEnergyTimeParsed = DateTime.Parse(lastTime);
            TimeSpan timePassed = DateTime.Now - lastEnergyTimeParsed;

            int energyToAdd = Mathf.FloorToInt((float)(timePassed.TotalSeconds / energyWaitTime));
            Energy = Mathf.Min(Energy + energyToAdd, maxEnergy);

            float leftoverSeconds = (float)(timePassed.TotalSeconds % energyWaitTime);
            lastEnergyGivenTime = Time.time - leftoverSeconds;
        }
        else
        {
            lastEnergyGivenTime = Time.time;
        }

        SaveEnergy();
    }

    public void LoadCoin()
    {
        Coins = PlayerPrefs.GetFloat(CoinKey, Coins);
    }

    public void LoadBlueCoin()
    {
        BlueCircleAmount = PlayerPrefs.GetInt(BlueCoinKey, 4);
    }

    public void LoadFirstBranchCoin()
    {
        FirstBranchCoinAmount = PlayerPrefs.GetInt(FirstBranchCoinKey, 0);
    }
    public void LoadSecondBranchCoin()
    {
        SecondBranchCoinAmount = PlayerPrefs.GetInt(SecondBranchCoinKey, 0);
    }

    public void LoadSafeChoice()
    {
        SafeChoiceAmount = PlayerPrefs.GetInt(SafeChoiceKey, 0);
    }

    public void LoadEleminateChoice()
    {
        EleminateOneAmount = PlayerPrefs.GetInt(EleminateOneKey, 0);
    }

    IEnumerator EnergyTimer()
    {
        while (true)
        {
            if (Energy < maxEnergy)
            {
                float timeToWait = energyWaitTime - (Time.time - lastEnergyGivenTime);
                if (timeToWait > 0)
                    yield return new WaitForSeconds(timeToWait);

                Energy++;
                lastEnergyGivenTime = Time.time;
                SaveEnergy();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void SaveEnergy()
    {
        PlayerPrefs.SetInt(EnergyKey, Energy);
        PlayerPrefs.SetString(LastEnergyTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void SaveBlueCircle()
    {
        PlayerPrefs.SetInt(BlueCoinKey, BlueCircleAmount);
        PlayerPrefs.Save();
    }

    public void SaveFirstBranchCoin()
    {
        PlayerPrefs.SetInt(FirstBranchCoinKey, FirstBranchCoinAmount);
        PlayerPrefs.Save();
    }

    public void SaveSecondBranchCoin()
    {
        PlayerPrefs.SetInt(SecondBranchCoinKey, SecondBranchCoinAmount);
        PlayerPrefs.Save();
    }

    public void SaveSafeChoiceCoin()
    {
        PlayerPrefs.SetInt(SafeChoiceKey, SafeChoiceAmount);
        PlayerPrefs.Save();
    }

    public void SaveEleminateOneCoin()
    {
        PlayerPrefs.SetInt(EleminateOneKey, EleminateOneAmount);
        PlayerPrefs.Save();
    }

    public void SpendEnergy(int amount)
    {
        Energy = Mathf.Max(Energy - amount, 0);
        lastEnergyGivenTime = Time.time;
        SaveEnergy();
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetFloat(CoinKey, Coins);
        PlayerPrefs.Save();
    }

    public void OpenSellPanel()
    {
        if (SellPanel.activeSelf)
        {
            SellPanel.gameObject.SetActive(false);
        }
        else
        {
            SellPanel.gameObject.SetActive(true);
        }
    }
    public void OpenSellPanel1()
    {
        if (SellPanel1.activeSelf)
        {
            SellPanel1.gameObject.SetActive(false);
        }
        else
        {
            SellPanel1.gameObject.SetActive(true);
        }
    }

    public void OpenSellPanel2()
    {
        if (SellPanel2.activeSelf)
        {
            SellPanel2.gameObject.SetActive(false);
        }
        else
        {
            SellPanel2.gameObject.SetActive(true);
        }
    }

    public void SpendCoins(int amount)
    {
        Debug.Log(amount);
        if (amount <= Coins && amount == 120)
        {
            Coins -= amount;
            CoinText.text = ((int)Coins).ToString();
            SafeChoiceAmount++;
            SafeChoiceText[0].text = SafeChoiceAmount.ToString();
            SaveSafeChoiceCoin();
            SaveCoin();
            SellPanel.gameObject.SetActive(false);
        }
        else if (amount == 121 && amount <= Coins)
        {
            Coins -= 120;
            CoinText.text = ((int)Coins).ToString();
            EleminateOneAmount++;
            EleminateText[0].text = EleminateOneAmount.ToString();
            SaveEleminateOneCoin();
            SaveCoin();
            SellPanel1.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Ska pare");
            SellPanel.gameObject.SetActive(false);
            SellPanel1.gameObject.SetActive(false);
        }

    }
    

    public void SpendUniCoins(int Amount)
    {
        if(Amount <= BlueCircleAmount && Amount == 1)
        {
            BlueCircleAmount -= Amount;
            BlueCirlceText[0].text = BlueCircleAmount.ToString();
            BlueCirlceText[1].text = BlueCircleAmount.ToString();
            SafeChoiceAmount++;
            SafeChoiceText[0].text = SafeChoiceAmount.ToString();
            SaveSafeChoiceCoin();
            SaveBlueCircle();
            SellPanel.gameObject.SetActive(false);
        }
        else if (Amount <= BlueCircleAmount && Amount == 2)
        {
            BlueCircleAmount -= 1;
            BlueCirlceText[0].text = BlueCircleAmount.ToString();
            BlueCirlceText[1].text = BlueCircleAmount.ToString();
            EleminateOneAmount++;
            EleminateText[0].text = EleminateOneAmount.ToString();
            SaveEleminateOneCoin();
            SaveBlueCircle();
            SellPanel1.gameObject.SetActive(false);
        }
        else if (Amount <= BlueCircleAmount && Amount == 3)
        {
            BlueCircleAmount -= 1;
            BlueCirlceText[0].text = BlueCircleAmount.ToString();
            BlueCirlceText[1].text = BlueCircleAmount.ToString();
            Energy += 40;
            Energytext.text = Energy.ToString();
            SaveBlueCircle();
            SaveEnergy();
            SellPanel2.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Ska uni coins");
            SellPanel.gameObject.SetActive(false);
            SellPanel1.gameObject.SetActive(false);
            SellPanel2.gameObject.SetActive(false);
        }
    }

    public void SpendSafeChoice(int Amount)
    {
        if (Amount <= BlueCircleAmount)
        {
            SafeChoiceAmount++;
            BlueCircleAmount--;
            BlueCirlceText[0].text = BlueCircleAmount.ToString();
            SafeChoiceText[0].text = SafeChoiceAmount.ToString();
            SaveSafeChoiceCoin();
            SaveBlueCircle();
        }
        else
        {
            Debug.Log("No Safe Choices left");
        }
    }

    public void SpendBranchCoins(int amount)
    {
        if (amount == 2 && FirstBranchCoinAmount >= 2)
        {
            FirstBranchCoinAmount -= 2;
            SecondBranchCoinAmount -= 2;
            FirstBranchCoinText[0].text = FirstBranchCoinAmount.ToString();
            Energy++;
            Energytext.text = Energy.ToString();
            SaveEnergy();
            SaveFirstBranchCoin();
            SaveSecondBranchCoin();
            SellPanel2.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No Branch coins");
            SellPanel2.gameObject.SetActive(false);
        }
    }
}
