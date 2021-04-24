using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifeline : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_startHook;
    [SerializeField] private Rigidbody2D m_endHook;
    
    [SerializeField] private GameObject m_lifelineSegPrefab;
    [SerializeField] private int m_numLinks;

    [SerializeField] private List<HingeJoint2D> m_lifelineSegList = new List<HingeJoint2D>();

    public void GenerateLifeline( Rigidbody2D start, Rigidbody2D end ){
        m_startHook = start;
        m_endHook = end;
        Rigidbody2D previousBod = m_startHook;
        for( int i = 0; i <= m_numLinks; i++ ){
            GameObject newSegment = Instantiate( m_lifelineSegPrefab );
            newSegment.name = "Seg " + i;
            m_lifelineSegList.Add( newSegment.GetComponent<HingeJoint2D>() );
            newSegment.transform.parent = transform;
            newSegment.transform.parent.position = transform.position;
            HingeJoint2D hinge = newSegment.GetComponent<HingeJoint2D>();
            hinge.connectedBody = previousBod;
            previousBod = newSegment.GetComponent<Rigidbody2D>();
        }

        m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_lifelineSegList[ m_numLinks ].GetComponent<Rigidbody2D>();
        m_endHook.GetComponent<PlayerController>().CurrentLifeline = this;

        transform.parent = m_endHook.transform;
        m_lifelineSegList[  m_numLinks ].GetComponent<SpriteRenderer>().enabled = false;
        m_lifelineSegList[  m_numLinks ].GetComponent<Collider2D>().isTrigger = true;
    }

    public void SetNewHooks( Rigidbody2D originHook, Rigidbody2D targetHook){
        if( originHook ){
            m_startHook.GetComponent<HingeJoint2D>().connectedBody = null;
            m_startHook = originHook;
            m_lifelineSegList[ 0 ].connectedBody = m_startHook;
        }
        if( targetHook ){
            m_endHook.GetComponent<HingeJoint2D>().connectedBody = null;
            m_endHook = targetHook;
            m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_lifelineSegList[ m_numLinks ].GetComponent<Rigidbody2D>();

            if( m_endHook.GetComponent<PlayerController>() ){
                m_endHook.GetComponent<PlayerController>().CurrentLifeline = this;
            }
        }
    }
}
