using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BranchManager : MonoBehaviour
{
    public GameObject[] Branches;
    public GameObject[] Nav;
    public UnityEngine.UI.Button[] btn;
    public GameObject Coins;
    public GameObject TimeGMB;


    void Start()
    {
        
    }

    void Update()
    {
     
    }

    public void ActivateBranchToPlay(string tag)
    {
        foreach (GameObject branch in Branches)
        {
            if (branch.tag == "Branch")
            {
                SceneManager.LoadScene("Branch1");
                Coins.SetActive(true);
                btn[0].interactable = false;
                btn[1].interactable = false;
            }
            else if (branch.tag == "Branch2")
            {
                SceneManager.LoadScene("Branch2");
                Coins.SetActive(true);
                btn[0].interactable = false;
                btn[1].interactable = false;
            }
            else
            {
                branch.SetActive(false);
            }
        }
    }

    public void Branch1()
    {
        SceneManager.LoadScene("Branch1");
    }

    public void Branch2()
    {
        SceneManager.LoadScene("Branch2");
    }

    public void ActivateHomeShop(string tag)
    {
        foreach (GameObject n in Nav)
        {
            if (n.CompareTag(tag))
            {
                n.SetActive(true);
                if (n.tag == "Home")    
                {
                    btn[0].interactable = false;
                    btn[1].interactable = true;
                }
                else 
                {
                    btn[0].interactable = true;
                    btn[1].interactable = false;
                }
            }
            else
            {
                n.SetActive(false);
            }
        }
    }

    public void setTimeGameObject()
    {
        if (!TimeGMB.activeSelf)
        {
            TimeGMB.SetActive(true);
        }
        else
        {
            TimeGMB.SetActive(false);
        }
        
    }
}
