using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashIndicator : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem indicatorParticle;
    public void ActivateIndicator(float duration)
    {
        indicatorParticle.Stop();
        var main = indicatorParticle.main;
        main.duration = duration;
        indicatorParticle.Play();
    }
}
