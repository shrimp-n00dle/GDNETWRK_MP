using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    public NetworkVariable<int> currentTimeLeft = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    private bool hasTimerStarted;
    private float elapsedTime = 0;
    private int timerDuration = 120;

    public bool HasTimerStarted
    {
        get { return hasTimerStarted; }
        set { hasTimerStarted = value; }
    }

    private void Awake()
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

    public override void OnNetworkSpawn()
    {
        currentTimeLeft.OnValueChanged += OnTimerDurationChanged;
    }

    private void OnTimerDurationChanged(int oldValue, int newValue)
    {
        string formattedTime = FormatTimeToString(newValue);
        UIManager.Instance?.UpdateGameTimer(formattedTime);
    }

    private void Update()
    {
        if (hasTimerStarted && IsOwner)
        {
            elapsedTime += Time.deltaTime;
            int currentDuration = (int)this.elapsedTime - timerDuration;
            //  int current = currentTimeLeft.Value - (int)elapsedTime;

            // Debug.Log("current: " + current);

            RequestModifyCurrentTimeServerRpc(Mathf.Abs(currentDuration));

            //if (current <= 0)
            //{

            //}
        }
    }

    private string FormatTimeToString(int currentTime)
    {
        string formattedTime = string.Format("{0}:{1:00}", currentTime / 60, currentTime % 60);
        return formattedTime;
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestModifyCurrentTimeServerRpc(int currentTime)
    {
        currentTimeLeft.Value = currentTime;
        //Debug.Log("current: " + current);
        Debug.Log("Time Left: " + currentTimeLeft.Value);
    }







}
