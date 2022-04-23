using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    public RectTransform BtnPanel;
    public RectTransform DinoTxt;
    public GameObject CreditsPanel;
    public GameObject PausePanel;
    public bool isPanelOn = true;

    public void Play()
    {
        StartCoroutine(SceneCoroutine());
    }

    IEnumerator SceneCoroutine()
    {
        BtnPanel.transform.DOMoveY(-20, 1.5f).SetEase(Ease.InBack, 0.7f);
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        CreditsPanel.SetActive(true);
    }

    public void CloseBtn()
    {
        CreditsPanel.SetActive(false);
    }

    public void Start()
    {
        GameObject.Find("PlayBtn").GetComponent<Button>().interactable = false;
        GameObject.Find("CreditsBtn").GetComponent<Button>().interactable = false;
        GameObject.Find("ExitBtn").GetComponent<Button>().interactable = false;

        DinoTxt.transform.DOMoveY(3, 1.5f).SetEase(Ease.OutBack);
        StartCoroutine(DinoHeaderCoroutine());
    }
    
    IEnumerator DinoHeaderCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        BtnPanel.transform.DOMoveX(0, 1.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.5f);

        GameObject.Find("PlayBtn").GetComponent<Button>().interactable = true;
        GameObject.Find("CreditsBtn").GetComponent<Button>().interactable = true;
        GameObject.Find("ExitBtn").GetComponent<Button>().interactable = true;
    }
}
