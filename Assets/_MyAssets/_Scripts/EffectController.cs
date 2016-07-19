﻿using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {

    [SerializeField]
    private string collectEffectName;
    [SerializeField]
    private string obstacleHitEffectName;
    [SerializeField]
    private string impactEffectName;
    private Animator a;

    void Start()
    {
        a = GetComponent<Animator>();
    }

    public void Collected()
    {
        a.Play(collectEffectName);
    }

    public void Hit()
    {
        a.Play(obstacleHitEffectName);
    }

    public void Impact()
    {
        a.Play(impactEffectName);
    }
}
