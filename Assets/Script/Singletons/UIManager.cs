using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player 1 Elements")]
    [SerializeField] private TextMeshProUGUI P1PointsUI;
    [SerializeField] private TextMeshProUGUI P1BuffDurationUI;

    [Header("Player 2 Elements")]
    [SerializeField] private TextMeshProUGUI P2PointsUI;
    [SerializeField] private TextMeshProUGUI P2BuffDurationUI;

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
}
