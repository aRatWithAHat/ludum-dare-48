using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Self References
    private SpriteRenderer m_sprite;
    private Rigidbody2D m_body;

    [Header("Movement")]
    [SerializeField] private float m_accelerationForce;
    [SerializeField] private float m_currentLoad; // TODO: Remove Serialization

    // Lifeline

    [SerializeField] private Lifeline m_lifelinePrefab;
    [SerializeField] private Lifeline m_currentLifeline; // TODO: Remove Serialization
    public Lifeline CurrentLifeline { get => m_currentLifeline; set => m_currentLifeline = value; }
    [SerializeField] private Attachable m_attachableInRange;
    public Attachable AttachableInRange { get => m_attachableInRange; set => m_attachableInRange = value; }

    // Flare
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
    private string m_fireFlare = "Fire1";

    private void Awake(){
        m_sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        m_body = GetComponent<Rigidbody2D>();

        SetupSub();
    }

    private void SetupSub(){
        m_currentLoad = 0;
        CurrentLifeline = Instantiate( m_lifelinePrefab );
        CurrentLifeline.GenerateLifeline( GameObject.Find("LifelineStart").GetComponent<Rigidbody2D>() , m_body );

        m_remainingFlares = m_startingFlares;
    }

    private IEnumerator DeployFlare(){
        m_remainingFlares --;
        yield return new WaitForSeconds( m_cooldownFlare );
    }

    private void Update()
    {
        m_movementInput = new Vector2( Input.GetAxis( m_movementInputXRef ), Input.GetAxis( m_movementInputYRef )  );

        if( Input.GetButtonDown( m_interactRef ) && AttachableInRange ){
            Debug.Log("Attaching");
            CurrentLifeline.SetNewHooks( null, AttachableInRange.Body );
            CurrentLifeline = Instantiate( m_lifelinePrefab );
            CurrentLifeline.GenerateLifeline( AttachableInRange.Body, m_body );
            
        }

        if( Input.GetButtonDown( m_fireFlare) && ){

        }
    }

    private void FixedUpdate() {
        if( m_movementInput != Vector2.zero ){
            m_body.AddRelativeForce( m_movementInput * ( m_accelerationForce - m_currentLoad ) );
        }  
    }
}
