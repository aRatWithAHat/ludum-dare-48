using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lifeline : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_startHook;
    [SerializeField] private Rigidbody2D m_endHook;
    
    [SerializeField] private GameObject m_lifelineSegPrefab;
    [SerializeField] private int m_numLinks;
    [SerializeField] private AudioClip m_changeLengthSound;

    [SerializeField] private List<LifelineSegment> m_activeLifelineSegList = new List<LifelineSegment>();
    [SerializeField] private List<LifelineSegment> m_hiddenLifelineSegList = new List<LifelineSegment>();

    public void GenerateLifeline( Rigidbody2D start, Rigidbody2D end, int SetLengthTo = -1 ){
        m_startHook = start;
        m_endHook = end;
        Rigidbody2D previousBod = m_startHook;
        for( int i = 0; i <= m_numLinks; i++ ){
            GameObject newSegment = Instantiate( m_lifelineSegPrefab );
            newSegment.name = "Seg " + i;

            newSegment.transform.parent = transform;
            newSegment.transform.parent.position = transform.position;

            if( SetLengthTo == -1 || ( SetLengthTo > 0 && i < SetLengthTo ) ){
                m_activeLifelineSegList.Add( newSegment.GetComponent<LifelineSegment>() );
                m_activeLifelineSegList[i].Hinge.connectedBody = previousBod;
                previousBod = newSegment.GetComponent<Rigidbody2D>();
            }
            else if( SetLengthTo > 0 && i >= SetLengthTo ) {
                m_hiddenLifelineSegList.Add( newSegment.GetComponent<LifelineSegment>() );
                newSegment.gameObject.SetActive( false );
            }
        }

        m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<Rigidbody2D>();
        m_endHook.GetComponent<PlayerController>().CurrentLifeline = this;

        m_activeLifelineSegList[0].Lightsource.enabled = false;
        SetupLastLifelineSegment();

        transform.parent = m_endHook.transform;
    }

    public void TryAddLifelineSegment(){
        if( m_activeLifelineSegList.Count != m_numLinks ){
            AudioSource.PlayClipAtPoint( m_changeLengthSound, m_endHook.transform.position );
            m_activeLifelineSegList.Add( m_hiddenLifelineSegList[0] );
            m_hiddenLifelineSegList.RemoveAt( 0 );
            m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].transform.rotation =  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 2 ].transform.rotation;
            m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].Hinge.connectedBody = m_activeLifelineSegList[ m_activeLifelineSegList.Count - 2 ].GetComponent<Rigidbody2D>();

            m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].gameObject.SetActive( true );
            ReactivateLifelineSegmentVisuals(  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 2 ] );
            SetupLastLifelineSegment();

            m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<Rigidbody2D>();
        }
        else{
            Debug.Log( "Max Length Achieved" );
        }
    }
    public void TryRemoveLifelineSegment(){
        if( m_activeLifelineSegList.Count >= 3 ){
            AudioSource.PlayClipAtPoint( m_changeLengthSound, m_endHook.transform.position );
            m_hiddenLifelineSegList.Insert( 0, m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ] );
            m_activeLifelineSegList.Remove( m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ] );

            m_hiddenLifelineSegList[ 0 ].gameObject.SetActive( false );
            SetupLastLifelineSegment();

            m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<Rigidbody2D>();
        }
        else{
            Debug.Log( "Min Length Achieved" );
        }
    }

    public void SetupLastLifelineSegment(){
        m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<Collider2D>().isTrigger = true;
        m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].Lightsource.enabled = false;
        m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<SpriteRenderer>().enabled = false;
    }
    
    private void ReactivateLifelineSegmentVisuals( LifelineSegment seg ){
        seg.Lightsource.enabled = true;
        seg.GetComponent<SpriteRenderer>().enabled = true;
    }
    private void DesactivateLifelineSegmentVisuals( LifelineSegment seg ){
        seg.Lightsource.enabled = false;
        seg.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetNewHooks( Rigidbody2D originHook, Rigidbody2D targetHook){
        if( originHook ){
            m_startHook.GetComponent<HingeJoint2D>().connectedBody = null;
            m_startHook = originHook;
            m_activeLifelineSegList[ 0 ].Hinge.connectedBody = m_startHook;
        }
        if( targetHook ){
            m_endHook.GetComponent<HingeJoint2D>().connectedBody = null;
            m_endHook = targetHook;
            m_endHook.GetComponent<HingeJoint2D>().connectedBody =  m_activeLifelineSegList[ m_activeLifelineSegList.Count - 1 ].GetComponent<Rigidbody2D>();

            if( m_endHook.GetComponent<PlayerController>() ){
                m_endHook.GetComponent<PlayerController>().CurrentLifeline = this;
            }
        }
    }
}
