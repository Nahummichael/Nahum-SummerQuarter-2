using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI timerText;

    // Awake is called when the game object is activated
    private void Awake()
    {
        // check the singleton
        // make this the only instance of the script when there isnt any other
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy any duplicates of this script
            Destroy(gameObject);
        }
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"time: {time:F1}";
    }
}
