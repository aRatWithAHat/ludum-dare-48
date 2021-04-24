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

    private void Awake(){

        GenerateLifeline();
    }

    private void SetHooks( Rigidbody2D originHook, Rigidbody2D targetHook){
        m_startHook = originHook;
        m_endHook = targetHook;
    }

    private void GenerateLifeline(){
        Rigidbody2D previousBod = m_startHook;
        for( int i = 0; i < m_numLinks; i++ ){
            GameObject newSegment = Instantiate( m_lifelineSegPrefab );
            newSegment.name = "Seg " + i;
            m_lifelineSegList.Add( newSegment.GetComponent<HingeJoint2D>() );
            newSegment.transform.parent = transform;
            newSegment.transform.parent.position = transform.position;
            HingeJoint2D hinge = newSegment.GetComponent<HingeJoint2D>();
            hinge.connectedBody = previousBod;
            previousBod = newSegment.GetComponent<Rigidbody2D>();
        }

        m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_lifelineSegList[ m_numLinks - 1 ].GetComponent<Rigidbody2D>();
        m_lifelineSegList[  m_numLinks - 1 ].GetComponent<SpriteRenderer>().enabled = false;
    }
}
