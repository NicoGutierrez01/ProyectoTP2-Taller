using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{

    public GameObject nivelbuttonPrefab;
    public Transform buttonContainer;

    public int totallevels = 10;


    public void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        for (int i = 1; i <= totallevels; i++)
        {
            GameObject button = Instantiate(nivelbuttonPrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Nivel " + i;
            int levelIndex = i;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Nivel_" + levelIndex);
            });
        }
    }
}
