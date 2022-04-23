using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerTxt;
    public int time;

    private void Start()
    {
        StartCoroutine(TimeCoroutine());
    }

    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            time++;
            timerTxt.text = "Elapsed Time: " + time.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}