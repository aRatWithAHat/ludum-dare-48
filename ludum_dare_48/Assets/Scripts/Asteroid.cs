using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float m_maxBaseVelocity;
    [SerializeField] private float m_minBaseVelocity;

    [SerializeField] private float m_maxBaseRotationSpeed;
    [SerializeField] private float m_minBaseRotationSpeed;

    [SerializeField] private float m_maxDistanceBeforeRespawn;
    [SerializeField] private float m_maxRespawnDistance;
    [SerializeField] private float m_minRespawnDistance;

    [SerializeField] private Rigidbody2D _body; 

    private  void Awake(){  
        _body = GetComponent<Rigidbody2D>();
        InitiatePeacefulExistence();
        
    }

    private void InitiatePeacefulExistence(){
        float velocity = Random.Range( m_minBaseVelocity, m_maxBaseVelocity);
        Vector2 direction = new Vector2( Random.Range( -1f, 1f), Random.Range( -1f, 1f) );
        _body.AddForce( direction * velocity );

        _body.AddTorque(Random.Range( m_minBaseRotationSpeed, m_maxBaseRotationSpeed));
    }

    private void Update() {
    }
    
    private void Respawn(){

    }
}