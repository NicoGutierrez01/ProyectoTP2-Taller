using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimerImages : MonoBehaviour
{
    public Sprite[] countdownSprites; // [3, 2, 1, GO] en orden
    public Image countdownImage;
    public GameObject player;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Desactiva al jugador
        if (player != null)
            player.SetActive(false);

        for (int i = 0; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i];
            countdownImage.enabled = true;
            yield return new WaitForSeconds(1f);
        }

        countdownImage.enabled = false;

        // Activa al jugador
        if (player != null)
            player.SetActive(true);
    }
}
