using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisuals : NetworkBehaviour
{
    private readonly NetworkVariable<Color> netColor = new();
    private Color[] colors = { Color.cyan, Color.magenta };
    private int index = 0;

    [SerializeField] private MeshRenderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        netColor.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy()
    {
        netColor.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(Color prev, Color next)
    {
        renderer.material.color = next;
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            index = (int)OwnerClientId;
            CommitNetworkColorServerRpc(GetNextColor());
        }
        else
        {
            renderer.material.color = netColor.Value;
        }
    }

    [ServerRpc]
    private void CommitNetworkColorServerRpc(Color color)
    {
        netColor.Value = color;
    }

    private Color GetNextColor()
    {
        return colors[index++  % colors.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
