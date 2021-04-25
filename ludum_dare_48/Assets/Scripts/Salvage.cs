using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class Salvage : MonoBehaviour
{
    [SerializeField] private Light2D[] m_lightsources;
    [SerializeField] private float[] m_lightsourcesBaseRadius;
    [SerializeField] private float m_blinkTime;
    [SerializeField] private float m_timeBetweenBlinks;

    [SerializeField] public int SalvagableFlares;
    private void Awake() {
        transform.rotation = Quaternion.Euler( 0, 0, Random.Range( 0, 360 ) );
        transform.Find( "FlareLauncher" ).rotation = Quaternion.Euler( 0, 0, Random.Range( 0, 360 ) );

        m_lightsources = GetComponentsInChildren<Light2D>();

        m_lightsourcesBaseRadius = new float[ m_lightsources.Length ];
        for ( int i = 0; i < m_lightsources.Length; i++ ){
            m_lightsourcesBaseRadius[i] = m_lightsources[i].pointLightOuterRadius;
        }

        StartCoroutine( DoBlink() );
    }

    private IEnumerator DoBlink(){
        int index = 0;
        foreach( Light2D light in m_lightsources ){
            DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, m_lightsourcesBaseRadius[ index ], m_blinkTime / 2 ).SetEase( Ease.OutCubic );
            index++;
        }
        yield return new WaitForSeconds( m_blinkTime / 2 );

        foreach( Light2D light in m_lightsources ){
            DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, 0f, m_blinkTime / 2 ).SetEase( Ease.OutCubic );
        }
        yield return new WaitForSeconds( m_blinkTime / 2 );

        yield return new WaitForSeconds( m_timeBetweenBlinks );
        StartCoroutine( DoBlink() );
    }

    private void SetNewLights(){
        for ( int i = 0; i < m_lightsources.Length; i++ ){
            if( m_lightsources[i].name != "GenericLight" ){
                m_lightsources[i].color = Color.green;
            }
        }
    }
}
