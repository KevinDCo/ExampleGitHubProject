using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using echo17.Signaler.Core;
using Sirenix.OdinInspector;
using Doozy.Engine.UI;
using Doozy.Engine;
using System;
using TMPro;
using DG.Tweening;
public class DeathBySignal
{
    public string DeathObject;
}

public enum ExperienceState { None, StartScreen, Gameplay, End }
public class ExperienceManager : SerializedMonoBehaviour, ISubscriber
{
    #region variables
    [SerializeField] ExperienceState experienceState = ExperienceState.None;
    public ExperienceState ExperienceState => experienceState;
    #endregion

    #region references
    [SerializeField] TextMeshProUGUI deathText = null;
    [SerializeField] GameObject car = null;
    #endregion

    #region handlers
    void OnEnable()
    {
        Signaler.Instance.Subscribe<StartButtonSignal>(this, OnStartButton);
        Signaler.Instance.Subscribe<RestartButtonSignal>(this, OnRestartButton);
        Signaler.Instance.Subscribe<DeathBySignal>(this, OnDeath);
    }


    void OnDisable()
    {
        Signaler.Instance.UnSubscribe<StartButtonSignal>(this, OnStartButton);
        Signaler.Instance.UnSubscribe<RestartButtonSignal>(this, OnRestartButton);
        Signaler.Instance.UnSubscribe<DeathBySignal>(this, OnDeath);
    }

    bool OnDeath(DeathBySignal signal)
    {
        switch(ExperienceState)
        {
            case ExperienceState.Gameplay:
                Continue(signal);
                break;
        }
        return true;
    }

    bool OnStartButton(StartButtonSignal signal)
    {
        switch(ExperienceState)
        {
            case ExperienceState.None:
                break;
            case ExperienceState.StartScreen:
                Continue();
                break;
            case ExperienceState.Gameplay:
                break;
        }
        return true;
    }
    bool OnRestartButton(RestartButtonSignal signal)
    {
        switch(ExperienceState)
        {
            case ExperienceState.End:
                Continue();
                break;
        }
        return true;
    }
    #endregion

    #region logic
    void Start()
    {
        Continue();
    }
    void Continue(DeathBySignal deathSignal = null)
    {
        switch(ExperienceState)
        {
            case ExperienceState.None:
                TransitionNoneToStartScreen();
                break;
            case ExperienceState.StartScreen:
                TransitionStartScreenToGameplay();
                break;
            case ExperienceState.Gameplay:
                TransitionGameplayToEnd(deathSignal);
                break;
            case ExperienceState.End:
                TransitionEndToNone();
                break;
        }
    }
    #endregion
    #region internal functions
    void ChangeState(ExperienceState newState)
    {
        experienceState = newState;
    }
    void TransitionNoneToStartScreen()
    {
        ChangeState(ExperienceState.StartScreen);
        Sequence mainSequence = DOTween.Sequence();
        mainSequence.InsertCallback(.1f, () => car.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = false);
        mainSequence.InsertCallback(.5f, () => GameEventMessage.SendEvent("StartGame"));
    }
    void TransitionStartScreenToGameplay()
    {
        ChangeState(ExperienceState.Gameplay);
        Sequence mainSequence = DOTween.Sequence();
        mainSequence.InsertCallback(.1f, () => car.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = true);
    }
    void TransitionGameplayToEnd(DeathBySignal signal)
    {
        ChangeState(ExperienceState.End);
        Sequence mainSequence = DOTween.Sequence();
        mainSequence.InsertCallback(0f, () => car.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = false);
        mainSequence.InsertCallback(.2f, () => car.GetComponent<Rigidbody>().velocity = Vector3.zero);
        mainSequence.InsertCallback(.2f, () => car.GetComponent<Rigidbody>().isKinematic = true);
        mainSequence.InsertCallback(.5f, () => car.GetComponent<Rigidbody>().isKinematic = false);
        deathText.text = "KABOOM! YOU DIED BY " + signal.DeathObject.ToUpper();        
        GameEventMessage.SendEvent("Death");
    }
    void TransitionEndToNone()
    {
        ChangeState(ExperienceState.None);
        car.transform.position = Vector3.zero;
        car.transform.rotation = Quaternion.identity;
        Sequence mainSequence = DOTween.Sequence();
        mainSequence.InsertCallback(0f, () => Continue());
    }
    #endregion
}
