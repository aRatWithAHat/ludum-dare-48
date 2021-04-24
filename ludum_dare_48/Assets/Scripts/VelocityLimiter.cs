using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityLimiter : MonoBehaviour
{
    [SerializeField] private float m_baseMaxVelocity;
    private float m_currentBaseMaxVelocity;
    private float m_sqrMaxVelocity;
    private Rigidbody2D m_body;
    private void Awake(){
        m_body = GetComponent<Rigidbody2D>();
        m_sqrMaxVelocity = m_baseMaxVelocity * m_baseMaxVelocity;
    }

    private void FixedUpdate(){
        if( m_body.velocity.sqrMagnitude > m_sqrMaxVelocity ){
            
            float currentVelocity = m_body.velocity.magnitude;
            float clampForce = currentVelocity - m_currentBaseMaxVelocity;
            m_body.AddRelativeForce( ( m_body.velocity.normalized ) * clampForce );
            /* Debug.Log( gameObject + " needs clamping" );
            Debug.Log( "Current speed > " + currentVelocity );
            Debug.Log( "Brake is > " + clampForce ); */
        }
    }

    private void ModifyMaxVelocity( float newMaxVelocity ){
        m_currentBaseMaxVelocity = newMaxVelocity;
        m_sqrMaxVelocity = m_currentBaseMaxVelocity * m_currentBaseMaxVelocity;
    }
    private void ResetMaxVelocity( ){
        ModifyMaxVelocity( m_baseMaxVelocity );
    }
}
