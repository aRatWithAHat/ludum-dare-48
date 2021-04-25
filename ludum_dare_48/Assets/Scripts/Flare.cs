using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class Flare : MonoBehaviour
{
    private Light2D m_lightsource;
    private Rigidbody2D m_body;
    [SerializeField] private float m_startupTime;
    [SerializeField] private float m_timeBeforeExtinguishStart;
    [SerializeField] private float m_extinguishLength;

    public Rigidbody2D Body { get => m_body; set => m_body = value; }

    private void Awake() {
        m_lightsource = GetComponent<Light2D>();
        Body = GetComponent<Rigidbody2D>();
    }

    public void Activate( Rigidbody2D launcherBody, float velocity ) {
        Vector3 parentVelocity = launcherBody.GetPointVelocity( launcherBody.transform.position );
        Body.velocity = Body.transform.up * velocity + parentVelocity;
        StartCoroutine( FlareLifetime() );
    }

    private IEnumerator FlareLifetime(){
        yield return StartCoroutine( FlareStartup() );
        yield return new WaitForSeconds( m_timeBeforeExtinguishStart );
        yield return StartCoroutine( BeginFlareExtinguish() );
    }

    private IEnumerator FlareStartup(){
        float maxLight = m_lightsource.intensity;
        m_lightsource.intensity = 0;
        yield return DOTween.To( ()=> m_lightsource.intensity, x=> m_lightsource.intensity = x, maxLight, m_startupTime ).WaitForCompletion();
    }

    private IEnumerator BeginFlareExtinguish(){
        DOTween.To( ()=> m_lightsource.pointLightInnerRadius, x=> m_lightsource.pointLightInnerRadius = x, 0f, m_extinguishLength );
        DOTween.To( ()=> m_lightsource.pointLightOuterRadius, x=> m_lightsource.pointLightOuterRadius = x, 3f, m_extinguishLength );
        yield return DOTween.To( ()=> m_lightsource.intensity, x=> m_lightsource.intensity = x, 0.5f, m_extinguishLength ).WaitForCompletion();
        yield return new WaitForSeconds( 10 );
        yield return GetComponent<SpriteRenderer>().DOColor( Color.clear, 0.5f ).WaitForCompletion();
        Destroy( gameObject );
    }
}
