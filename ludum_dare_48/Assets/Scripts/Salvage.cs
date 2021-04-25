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
    [SerializeField] private bool m_isBlackBox;
    [SerializeField] public GameObject m_flarePrefab;
    private Transform m_flareSpawnPos;

    [SerializeField] private bool m_linked;

    private Transform m_flareLauncher;

    [SerializeField] private AudioClip m_distressSound;

    public bool IsBlackBox { get => m_isBlackBox; set => m_isBlackBox = value; }

    private void Awake() {
        transform.rotation = Quaternion.Euler( 0, 0, Random.Range( 0, 360 ) );
        if(  transform.Find( "FlareLauncher" ) ){
            m_flareLauncher = transform.Find( "FlareLauncher" );
            m_flareLauncher.rotation = Quaternion.Euler( 0, 0, Random.Range( 0, 360 ) );
            m_flareSpawnPos = m_flareLauncher.Find( "FlareSpawn" );
        }
            

        m_linked = false;

        m_lightsources = GetComponentsInChildren<Light2D>();

        m_lightsourcesBaseRadius = new float[ m_lightsources.Length ];
        for ( int i = 0; i < m_lightsources.Length; i++ ){
            m_lightsourcesBaseRadius[i] = m_lightsources[i].pointLightOuterRadius;
        }

        StartCoroutine( DoBlink() );
    }

    private IEnumerator DoBlink(){
        if( !m_linked ){
            AudioSource.PlayClipAtPoint( m_distressSound, transform.position + new Vector3( 0, 0, -5 ) );
        }
        int index = 0;
        foreach( Light2D light in m_lightsources ){
            DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, m_lightsourcesBaseRadius[ index ], m_blinkTime / 2 ).SetEase( Ease.OutCubic );
            index++;
        }
        yield return new WaitForSeconds( m_blinkTime / 2 );

        foreach( Light2D light in m_lightsources ){
            DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, 0.1f, m_blinkTime / 2 ).SetEase( Ease.OutCubic );
        }
        yield return new WaitForSeconds( m_blinkTime / 2 );

        yield return new WaitForSeconds( m_timeBetweenBlinks );
        StartCoroutine( DoBlink() );
    }

    public void StopBlink( bool stayOn ){
        StopAllCoroutines();
        if( stayOn ){
            int index = 0;
            foreach( Light2D light in m_lightsources ){
                DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, m_lightsourcesBaseRadius[ index ], m_blinkTime / 2 ).SetEase( Ease.OutCubic ).SetSpeedBased();
                index++;
            }
        }
        else{
            foreach( Light2D light in m_lightsources ){
            DOTween.To( ()=> light.pointLightOuterRadius, x=> light.pointLightOuterRadius = x, 0f, m_blinkTime / 2 ).SetEase( Ease.OutCubic );
        }
        }

    }

    private void SetAttachedToMainLifeline(){
        m_linked = true;
        GameManager.instance.ChangeCurrentObjective( this );
        for ( int i = 0; i < m_lightsources.Length; i++ ){
            m_lightsources[i].color = new Color( 0.7603404f, 1f, 0.6273585f ); // Oof
        }
        if( !IsBlackBox ){
            MissionControlAlertController.instance.QueueNewAlert( "//: WAKING CLOSEST UNIT AND MARKING WITH FLARE" );
            StartCoroutine( ShotFlareTowardNextObjective() );
        }
        
    }

    private IEnumerator ShotFlareTowardNextObjective(){
        float angle = EntityUtils.GetAngleBetweenPositions( transform.position, GameManager.instance.CurrentNextSalvage.transform.position );
        yield return StartCoroutine( m_flareLauncher.GetComponent<LookAtBehavior>().ForceLookAt( GameManager.instance.CurrentNextSalvage.transform ) );
        yield return new WaitForSeconds(2f);
        Flare flare = Instantiate( m_flarePrefab, m_flareSpawnPos.transform.position, m_flareLauncher.transform.rotation ).GetComponent<Flare>();
        flare.Activate( GetComponent<Rigidbody2D>(), 4 );
    }
}
