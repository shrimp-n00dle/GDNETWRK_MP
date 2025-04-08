using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI P1PointsUI;

    [SerializeField] private TextMeshProUGUI P2PointsUI;

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
}
