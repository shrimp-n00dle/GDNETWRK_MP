using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player 1 Elements")]
    [SerializeField] private TextMeshProUGUI P1PointsUI;
    [SerializeField] private TextMeshProUGUI P1BuffDurationUI;

    [Header("Player 2 Elements")]
    [SerializeField] private TextMeshProUGUI P2PointsUI;
    [SerializeField] private TextMeshProUGUI P2BuffDurationUI;

    [Header("Timer Elements")]
    [SerializeField] private TextMeshProUGUI TimerUI;

    [Header("Winner Elements")]
    [SerializeField] private GameObject panelWinnerUI;
    [SerializeField] private TextMeshProUGUI WinnerUI;

    [Header("Fruit Spawn Elements")]
    [SerializeField] private GameObject panelFruitSpawnUI;

    bool showFruitSpawnPanel = false;
    float elapsedTime = 0;
    float fruitDurationUI = 5;

    public bool ShowFruitSpawnPanel
    {
        get { return showFruitSpawnPanel; }
        set {  showFruitSpawnPanel = value; }
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(showFruitSpawnPanel)
        {
            panelFruitSpawnUI.SetActive(true);
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= fruitDurationUI)
            {
                panelFruitSpawnUI.SetActive(false);
                showFruitSpawnPanel = false;
                elapsedTime = 0;
            }
        }
    }

    public void UpdatePlayerPointsUI(int player, int points)
    {
        switch (player)
        {
            case 1:
                P1PointsUI.text = points.ToString();
                break;
            case 2:
                P2PointsUI.text = points.ToString();
                break;
        }
    }

    public void UpdatePlayerBuffDurationUI(int player, int buffDuration)
    {
        switch (player)
        {
            case 1:
                P1BuffDurationUI.text = buffDuration.ToString();
                break;
            case 2:
                P2BuffDurationUI.text = buffDuration.ToString();
                break;
        }
    }

    public void UpdateGameTimer(string timerLeft)
    {
        TimerUI.text = timerLeft;
    }

    public void ShowWinner(string outcome)
    {
        if(panelWinnerUI.activeSelf == false)
        {
            panelWinnerUI.SetActive(true);
            WinnerUI.text = outcome;
        }
    }
}
