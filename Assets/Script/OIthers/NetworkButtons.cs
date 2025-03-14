using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkButtons : MonoBehaviour
{
    public void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;

        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if(!NetworkManager.Singleton.IsClient && ! NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host", buttonStyle))
                NetworkManager.Singleton.StartHost();
            //if (GUILayout.Button("Server"))
            //    NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client", buttonStyle))
                NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }
}
