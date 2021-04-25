using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _maxBaseVelocity;
    [SerializeField] private float _minBaseVelocity;

    [SerializeField] private float _maxBaseRotationSpeed;
    [SerializeField] private float _minBaseRotationSpeed;

    private Rigidbody2D _body;
    

    private  void Awake(){  
        _body = GetComponent<Rigidbody2D>();
        InitiatePeacefulExistence();
    }

    private void InitiatePeacefulExistence(){
        float velocity = Random.Range( _minBaseVelocity, _maxBaseVelocity);
        Vector2 direction = new Vector2( Random.Range( -1f, 1f), Random.Range( -1f, 1f) );
        _body.AddForce( direction * velocity );

        _body.AddTorque(Random.Range( _minBaseRotationSpeed, _maxBaseRotationSpeed));
    }
    
}