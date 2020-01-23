using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using echo17.Signaler.Core;

public struct StartButtonSignal { }
public struct RestartButtonSignal { }
public class CommonButtonSignalController : MonoBehaviour, IBroadcaster
{

    public void StartButton()
    {
        Signaler.Instance.Broadcast(this, new StartButtonSignal{});
    }
    public void RestartButton()
    {
        Signaler.Instance.Broadcast(this, new RestartButtonSignal{});
    }
}
