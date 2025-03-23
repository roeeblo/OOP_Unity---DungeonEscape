using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  private static UIManager _instance;
  public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI MANAGER IS NULL");
            }
            return _instance;
        }
    }

    public Text playerGemCount;
    public Image Selection;
    public Text gemcount;
    public Image[] healthbars;

    public void OpenShop(int gemamount)
    {
        playerGemCount.text = "" + gemamount + "G";
    }
    public void UpdateShopSelection(int ypos)
    {
        Selection.rectTransform.anchoredPosition = new Vector2(Selection.rectTransform.anchoredPosition.x, ypos);
    }

    public void UpdateGemCount(int count)
    {
        gemcount.text = "" + count + "G";
    }
    public void UpdateLives(int health)
    {
        for (int i = 0; i <= health; i++) { 
        
            if (i == health)
            {
                healthbars[i].enabled = false;
            }
        }
    }

    private void Awake()
    {
        _instance = this;
    }

}
