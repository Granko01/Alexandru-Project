using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    [Header("Text's")]
    public Text Energytext;
    public Text CoinText;
    public Text Timer;
    public Text[] BlueCirlceText;
    public Text[] FirstBranchCoinText;

    private float lastEnergyGivenTime;
    private float timeLeft;

    [Header("Energy Settings")]
    private const string EnergyKey = "Energy";
    private const string LastEnergyTimeKey = "LastEnergyTime";
    private const string CoinKey = "Coins";
    private const string BlueCoinKey = "BlueCoin";
    private const string FirstBranchCoinKey = "FirstBranchCoin";

    public GameObject SellPanel;

   

    void Start()
    {
        LoadEnergy();
        LoadCoin();
        LoadBlueCoin();
        LoadFirstBranchCoin();
        StartCoroutine(EnergyTimer());
        CoinText.text = ((int)Coins).ToString();
        Energytext.text = Energy.ToString();
        BlueCirlceText[0].text = BlueCircleAmount.ToString();
        BlueCirlceText[1].text = BlueCircleAmount.ToString();
        FirstBranchCoinText[0].text = FirstBranchCoinAmount.ToString();
        FirstBranchCoinText[1].text = FirstBranchCoinAmount.ToString();
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
        BlueCircleAmount = PlayerPrefs.GetInt(BlueCoinKey, 0);
    }

    public void LoadFirstBranchCoin()
    {
        FirstBranchCoinAmount = PlayerPrefs.GetInt(FirstBranchCoinKey, 0);
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
        SellPanel.gameObject.SetActive(true);
    }

    public void SpendCoins(int amount)
    {
        Debug.Log(amount);
        if (amount <= Coins)
        {
            Coins -= amount;
            CoinText.text = ((int)Coins).ToString();
            FirstBranchCoinAmount++;
            FirstBranchCoinText[0].text = FirstBranchCoinAmount.ToString();
            FirstBranchCoinText[1].text = FirstBranchCoinAmount.ToString();
            //BlueCircleAmount++;
            //BlueCirlceText[0].text = BlueCircleAmount.ToString();
            //BlueCirlceText[1].text = BlueCircleAmount.ToString();
            //SaveBlueCircle();
            SaveFirstBranchCoin();
            SaveCoin();
            SellPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Ska pare");
            SellPanel.gameObject.SetActive(false);
        }

    }

}
