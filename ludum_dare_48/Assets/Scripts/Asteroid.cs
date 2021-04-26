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
    [SerializeField] private float m_minRespawnDistanceFromPlayer;

    [SerializeField] private Rigidbody2D _body; 

    private float m_baseMass = 50;

    private  void Awake(){  
        _body = GetComponent<Rigidbody2D>();
        Respawn( GameManager.instance.Player.transform.position );
    }

    private void InitiatePeacefulExistence(){
        float random = Random.Range( 0.4f, 1.4f );
        transform.localScale = new Vector2( random, random );
        _body.mass = m_baseMass * random;
        float velocity = Random.Range( m_minBaseVelocity, m_maxBaseVelocity);
        Vector2 direction = new Vector2( Random.Range( -1f, 1f), Random.Range( -1f, 1f) );
        _body.AddForce( direction * velocity * ( Random.Range( 0.1f, 2f) * 5 ), ForceMode2D.Impulse );

        _body.AddTorque(Random.Range( m_minBaseRotationSpeed, m_maxBaseRotationSpeed));
    }

    private void Update() {

        if( Vector2.Distance( transform.position, GameManager.instance.Player.transform.position ) > m_maxDistanceBeforeRespawn ){
            Respawn( GameManager.instance.Player.transform.position );
        }
    }
    
    private void Respawn( Vector2 playerPos ){
        _body.velocity = Vector2.zero;
        Vector2 randomCoords = Vector2.zero;
        do{
            float randomCoordsX = Random.Range( m_minRespawnDistance, m_maxRespawnDistance );

            float randomCoordsY = Random.Range( m_minRespawnDistance, m_maxRespawnDistance );

            randomCoords = new Vector2( randomCoordsX, randomCoordsY ) + playerPos;
        }while( Vector2.Distance( transform.position, GameManager.instance.Player.transform.position ) < m_minRespawnDistanceFromPlayer );

        transform.position = randomCoords;
        InitiatePeacefulExistence();
    }
}