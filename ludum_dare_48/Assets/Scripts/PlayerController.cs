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

    // Player Inputs
    private Vector2 m_movementInput;
    private string m_movementInputX = "Horizontal";
    private string m_movementInputY = "Vertical";
    
    private void Awake(){
        m_sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        m_body = GetComponent<Rigidbody2D>();

        SetupSub();
    }

    private void SetupSub(){
        m_currentLoad = 0;
    }

    private void Update()
    {
        Debug.Log( m_movementInput );
        m_movementInput = new Vector2( Input.GetAxis( m_movementInputX ), Input.GetAxis( m_movementInputY)  );
    }

    private void FixedUpdate() {
        if( m_movementInput != Vector2.zero ){
            m_body.AddRelativeForce( m_movementInput * ( m_accelerationForce - m_currentLoad ) );
        }
        else{
            
        }
    }
}
