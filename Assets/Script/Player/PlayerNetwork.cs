using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> netPosition = new (writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Quaternion> netRotation = new (writePerm: NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            netPosition.Value = transform.position;
            netRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = netPosition.Value;
            transform.rotation = netRotation.Value;
        }
    }
}
