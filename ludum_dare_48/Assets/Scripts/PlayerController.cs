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
    private AudioSource m_audio;

    [Header("Movement")]
    [SerializeField] private float m_accelerationForce;
    private ParticleSystem m_thrusterTop;
    private ParticleSystem m_thrusterLeft;
    private ParticleSystem m_thrusterBot;
    private ParticleSystem m_thrusterRight;
    private AudioSource m_thrusterTopAudio;
    private AudioSource m_thrusterLeftAudio;
    private AudioSource m_thrusterBotAudio;
    private AudioSource m_thrusterRightAudio;
    [SerializeField] private AudioClip m_thrusterClip;

    [Header("Lifeline")]

    [SerializeField] private Lifeline m_lifelinePrefab;
    [SerializeField] private Lifeline m_emerLifelinePrefab;
    [SerializeField] private Lifeline m_currentLifeline; // TODO: Remove Serialization
    public Lifeline CurrentLifeline { get => m_currentLifeline; set => m_currentLifeline = value; }
    [SerializeField] private Attachable m_attachableInRange;
    private bool m_reelingLifeline;
    public Attachable AttachableInRange { get => m_attachableInRange; set => m_attachableInRange = value; }

    [Header("Flare")]
    [SerializeField] private GameObject m_flarePrefab;
    [SerializeField] private float m_throwForce;
    [SerializeField] private float m_cooldownFlare;
    private bool m_canFlare;
    public bool CanFlare { 
        get => m_canFlare; 
        set{
            m_canFlare = value;
            if( m_canFlare ){
                UIController.inst.FlareDisplay.SetText( "FLARE //: <color=#ffc216>[LEFT CLICK]</color>" );
            }
            else{
                UIController.inst.FlareDisplay.SetText( "FLARE //: RELOADING" );
            }
            
        }
    }

    // Player Inputs
    private Vector2 m_movementInput;
    private string m_movementInputXRef = "Horizontal";
    private string m_movementInputYRef = "Vertical";
    private string m_interactRef = "Jump";
    private string m_fireFlareRef = "Fire1";
    private string m_lifelineLengthControlRef = "Mouse ScrollWheel";

    private void Start(){

        
        m_sprite = transform.Find( "Sprite" ).GetComponent<SpriteRenderer>();
        m_flareLauncher = transform.Find( "FlareLauncher" );
        m_flareSpawnPos = m_flareLauncher.Find( "FlareSpawn" );
        m_body = GetComponent<Rigidbody2D>();
        m_audio = GetComponent<AudioSource>();
        GetComponent<HingeJoint2D>().connectedBody = m_body;

        m_thrusterTop = transform.Find("ThrusterParticulesTop").GetComponent<ParticleSystem>();
        m_thrusterLeft = transform.Find("ThrusterParticulesLeft").GetComponent<ParticleSystem>();
        m_thrusterBot = transform.Find("ThrusterParticulesBot").GetComponent<ParticleSystem>();
        m_thrusterRight = transform.Find("ThrusterParticulesRight").GetComponent<ParticleSystem>();
        
        m_thrusterTopAudio = m_thrusterTop.GetComponent<AudioSource>();
        m_thrusterLeftAudio = m_thrusterLeft.GetComponent<AudioSource>();
        m_thrusterBotAudio = m_thrusterBot.GetComponent<AudioSource>();
        m_thrusterRightAudio = m_thrusterRight.GetComponent<AudioSource>();

        m_thrusterTopAudio.clip = m_thrusterClip;
        m_thrusterLeftAudio.clip = m_thrusterClip;
        m_thrusterBotAudio.clip = m_thrusterClip;
        m_thrusterRightAudio.clip = m_thrusterClip;

        m_thrusterTop.Stop();
        m_thrusterLeft.Stop(); 
        m_thrusterBot.Stop(); 
        m_thrusterRight.Stop();

        this.enabled = false;
    }

    public  void SetupNewRun( int nb ){
        UIController.inst.DroneNameDisplay.SetText( "DRN-" + nb );

        m_reelingLifeline  = false;
        CanFlare = true;
        CurrentLifeline = Instantiate( m_lifelinePrefab );
        CurrentLifeline.GenerateLifeline( GameObject.Find( "LifelineStart" ).GetComponent<Rigidbody2D>() , m_body, 3 );
    }

    private IEnumerator DeployFlare(){
        CanFlare = false;
        Flare flare = Instantiate( m_flarePrefab, m_flareSpawnPos.transform.position, m_flareLauncher.transform.rotation ).GetComponent<Flare>();
        flare.Activate( m_body, 4 );
        
        yield return new WaitForSeconds( m_cooldownFlare );
        CanFlare = true;
    }

    private void Update()
    {
        m_movementInput = new Vector2( Input.GetAxis( m_movementInputXRef ), Input.GetAxis( m_movementInputYRef )  );

        if( m_movementInput.x < 0 && m_thrusterRight.isStopped ){
            m_thrusterRight.Play();
            m_thrusterRightAudio.Play();
            if( m_thrusterLeft.isPlaying ){
                m_thrusterLeft.Stop();
                m_thrusterLeftAudio.Stop();
            }
        }
        else if( m_movementInput.x > 0 && m_thrusterLeft.isStopped ){
            m_thrusterLeft.Play();
            m_thrusterLeftAudio.Play();
            if( m_thrusterRight.isPlaying ){
                m_thrusterRight.Stop();
                m_thrusterRightAudio.Stop();
            }
        }
        else if( m_movementInput.x == 0){
            if( m_thrusterLeft.isPlaying ){
                m_thrusterLeft.Stop();
                m_thrusterLeftAudio.Stop();
            }
            if( m_thrusterRight.isPlaying ){
                m_thrusterRight.Stop();
                m_thrusterRightAudio.Stop();
            }
        }

        if( m_movementInput.y < 0 && m_thrusterTop.isStopped ){
            m_thrusterTop.Play();
            m_thrusterTopAudio.Play();
            if( m_thrusterBot.isPlaying ){
                m_thrusterBot.Stop();
                m_thrusterBotAudio.Stop();
            }
        }
        else if( m_movementInput.y > 0 && m_thrusterBot.isStopped ){
            m_thrusterBot.Play();
            m_thrusterBotAudio.Play();
            if( m_thrusterTop.isPlaying ){
                m_thrusterTop.Stop();
                m_thrusterTopAudio.Stop();
            }
        }
        else if( m_movementInput.y == 0){
            if( m_thrusterBot.isPlaying ){
                m_thrusterBot.Stop();
                m_thrusterBotAudio.Stop();
            }
            if( m_thrusterTop.isPlaying ){
                m_thrusterTop.Stop();
                m_thrusterTopAudio.Stop();
            }
        }

        Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        float angle = EntityUtils.GetAngleBetweenPositions( transform.position, mouseOnScreen );
        m_flareLauncher.rotation = Quaternion.Euler( new Vector3( 0f, 0f, angle - 90 ) );

        if( Input.GetButtonDown( m_interactRef ) && AttachableInRange ){
            CurrentLifeline.SetNewHooks( null, AttachableInRange.Body );
            CurrentLifeline = Instantiate( m_lifelinePrefab );
            CurrentLifeline.GenerateLifeline( AttachableInRange.Body, m_body, 3 );
            AttachableInRange.SetAttached();
            
        }

        if( Input.GetButtonDown( m_fireFlareRef ) && CanFlare ){
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
            m_body.AddRelativeForce( m_movementInput * ( m_accelerationForce ) );
        }  
    }
}
