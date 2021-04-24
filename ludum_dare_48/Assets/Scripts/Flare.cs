using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class Flare : MonoBehaviour
{
    private Light2D m_lightsource;

    [SerializeField] private float m_timeBeforeExtinguishStart;
    [SerializeField] private float m_extinguishLength;

    private void Awake() {
        m_lightsource = GetComponent<Light2D>();

        StartCoroutine( BeginFlareExtinguish() );
    }

    private IEnumerator BeginFlareExtinguish(){
        yield return new WaitForSeconds( m_timeBeforeExtinguishStart );
        DOTween.To( ()=> m_lightsource.intensity, x=> m_lightsource.intensity = x, 0, m_extinguishLength );
    }
}
