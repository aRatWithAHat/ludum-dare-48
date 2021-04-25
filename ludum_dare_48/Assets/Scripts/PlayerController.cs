using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Self References
    private SpriteRenderer m_sprite;
    private Transform m_flareLauncher;
    private Transform m_flareSpawnPos;
    private Rigidbody2D m_body;

    [Header("Movement")]
    [SerializeField] private float m_accelerationForce;
    [SerializeField] private float m_currentLoad; // TODO: Remove Serialization

    [Header("Lifeline")]

    [SerializeField] private Lifeline m_lifelinePrefab;
    [SerializeField] private Lifeline m_currentLifeline; // TODO: Remove Serialization
    public Lifeline CurrentLifeline { get => m_currentLifeline; set => m_currentLifeline = value; }
    [SerializeField] private Attachable m_attachableInRange;
    private bool m_reelingLifeline;
    public Attachable AttachableInRange { get => m_attachableInRange; set => m_attachableInRange = value; }

    [Header("Flare")]
    [SerializeField] private GameObject m_flarePrefab;
    [SerializeField] private float m_throwForce;
    [SerializeField] private int m_startingFlares;
    [SerializeField] private float m_cooldownFlare;
    private int m_remainingFlares;

    // Player Inputs
    private Vector2 m_movementInput;
    private string m_movementInputXRef = "Horizontal";
    private string m_movementInputYRef = "Vertical";
    private string m_interactRef = "Jump";
    private string m_fireFlareRef = "Fire1";
    private string m_lifelineLengthControlRef = "Mouse ScrollWheel";

    private void Awake(){
        m_sprite = transform.Find( "Sprite" ).GetComponent<SpriteRenderer>();
        m_flareLauncher = transform.Find( "FlareLauncher" );
        m_flareSpawnPos = m_flareLauncher.Find( "FlareSpawn" );
        m_body = GetComponent<Rigidbody2D>();

        SetupSub();
    }

    private void SetupSub(){
        m_currentLoad = 0;
        m_reelingLifeline  = false;
        CurrentLifeline = Instantiate( m_lifelinePrefab );
        CurrentLifeline.GenerateLifeline( GameObject.Find( "LifelineStart" ).GetComponent<Rigidbody2D>() , m_body, 3 );

        m_remainingFlares = m_startingFlares;
    }

    private IEnumerator DeployFlare(){
        m_remainingFlares --;

        Flare flare = Instantiate( m_flarePrefab, m_flareSpawnPos.transform.position, m_flareLauncher.transform.rotation ).GetComponent<Flare>();
        flare.Activate( m_body, 4 );

        yield return new WaitForSeconds( m_cooldownFlare );
    }

    private void Update()
    {
        m_movementInput = new Vector2( Input.GetAxis( m_movementInputXRef ), Input.GetAxis( m_movementInputYRef )  );

        Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        float angle = EntityUtils.GetAngleBetweenPositions( transform.position, mouseOnScreen );
        m_flareLauncher.rotation = Quaternion.Euler( new Vector3( 0f, 0f, angle - 90 ) );

        if( Input.GetButtonDown( m_interactRef ) && AttachableInRange ){
            Debug.Log("Attaching");
            CurrentLifeline.SetNewHooks( null, AttachableInRange.Body );
            CurrentLifeline = Instantiate( m_lifelinePrefab );
            CurrentLifeline.GenerateLifeline( AttachableInRange.Body, m_body, 3 );
            
        }

        if( Input.GetButtonDown( m_fireFlareRef ) && m_remainingFlares > 0 ){
            StartCoroutine( DeployFlare() );
        }

        if( !m_reelingLifeline ){
            if( Input.GetAxis( m_lifelineLengthControlRef ) < 0f ){
                m_reelingLifeline = true;
                StartCoroutine( ReelInLifeline() );
            }
            else if( Input.GetAxis( m_lifelineLengthControlRef ) > 0f ){
                m_reelingLifeline = true;
                StartCoroutine( ReelOutLifeline() );
            }
        }
    }

    private IEnumerator ReelInLifeline(){
        CurrentLifeline.TryRemoveLifelineSegment();
        yield return new WaitForSeconds( 0.5f );
        m_reelingLifeline = false;
        
    }

    private IEnumerator ReelOutLifeline(){
        CurrentLifeline.TryAddLifelineSegment();
        yield return new WaitForSeconds( 0.5f );
        m_reelingLifeline = false;
        
    }

    private void FixedUpdate() {
        if( m_movementInput != Vector2.zero ){
            m_body.AddRelativeForce( m_movementInput * ( m_accelerationForce - m_currentLoad ) );
        }  
    }
}
