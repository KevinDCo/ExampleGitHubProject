using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.Signaler.Core;

public class ObstacleController : MonoBehaviour, IBroadcaster
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            Signaler.Instance.Broadcast<DeathBySignal>(this, new DeathBySignal { DeathObject = gameObject.name });
        }
    }
}
