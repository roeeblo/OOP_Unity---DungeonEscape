using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject Shoppanel;
    public int currentselected = -1;
    public int currentcost = 0;
    private Player player;

    public Text insufficientFundsText;
    public AudioClip purchaseSuccessClip;
    private AudioSource audioSource;

    private void Start()
    {
        Shoppanel.SetActive(false);

        if (insufficientFundsText != null)
        {
            insufficientFundsText.gameObject.SetActive(false);
            Color color = insufficientFundsText.color;
            color.a = 0f;
            insufficientFundsText.color = color;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            if (player != null)
            {
                UIManager.Instance.OpenShop(player.diamonds);
            }
            Shoppanel.SetActive(true);

            if (insufficientFundsText != null)
            {
                insufficientFundsText.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Shoppanel.SetActive(false);
        }
    }

    public void SelectItem(int item)
    {
        currentselected = -1;
        currentcost = 0;

        switch (item)
        {
            case 0:
                UIManager.Instance.UpdateShopSelection(102);
                currentselected = 0;
                currentcost = 200;
                break;
            case 1:
                UIManager.Instance.UpdateShopSelection(-2);
                currentselected = 1;
                currentcost = 400;
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(-102);
                currentselected = 2;
                currentcost = 100;
                break;
            default:
                break;
        }
    }

    public void BuyItem()
    {
        if (player == null)
        {
            return;
        }

        if (player.diamonds >= currentcost && currentselected != -1)
        {
            player.diamonds -= currentcost;

            switch (currentselected)
            {
                case 0:
                    player.attackCooldown = 0.08f;
                    break;
                case 1:
                    player.speed = 8f;
                    break;
                case 2:
                    GameManager.Instance.HasKey = true;
                    break;
                default:
                    break;
            }

            if (audioSource != null && purchaseSuccessClip != null)
            {
                audioSource.PlayOneShot(purchaseSuccessClip);
            }

            Shoppanel.SetActive(false);
        }
        else
        {
            if (insufficientFundsText != null)
            {
                StartCoroutine(ShowInsufficientFundsMessage());
            }
        }
    }

    private IEnumerator ShowInsufficientFundsMessage()
    {
        insufficientFundsText.gameObject.SetActive(true);
        insufficientFundsText.text = "Insufficient Funds";

        Color color = insufficientFundsText.color;
        for (float t = 0; t <= 1; t += Time.deltaTime / 0.5f)
        {
            color.a = Mathf.Lerp(0f, 1f, t);
            insufficientFundsText.color = color;
            yield return null;
        }
        color.a = 1f;
        insufficientFundsText.color = color;

        yield return new WaitForSeconds(1.5f);

        for (float t = 0; t <= 1; t += Time.deltaTime / 0.5f)
        {
            color.a = Mathf.Lerp(1f, 0f, t);
            insufficientFundsText.color = color;
            yield return null;
        }
        color.a = 0f;
        insufficientFundsText.color = color;

        insufficientFundsText.gameObject.SetActive(false);
    }
}
