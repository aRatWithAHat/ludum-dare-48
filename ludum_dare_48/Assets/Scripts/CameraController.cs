using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Self references
    public static CameraController instance;
    private Vector3 m_basePosition;

    [Header("Follow cam")]
    [SerializeField] private Transform m_followTarget;
    [SerializeField] private float m_smoothSpeed = 10;
    private Vector2 m_velocity = Vector2.zero;

    private void Awake(){
        instance = this;
        m_basePosition = transform.position;
    }

    private void LateUpdate(){
        transform.position = new Vector3( m_followTarget.position.x,m_followTarget.position.y, m_basePosition.z );
    }

    public void FollowNewTarget( Transform newTarget = null ){
        m_followTarget = newTarget;
    }
}