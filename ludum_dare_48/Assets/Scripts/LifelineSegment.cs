using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifelineSegment : MonoBehaviour
{
    private GameObject previousSegConnection;
    private GameObject nextSegConnection;
    public GameObject PreviousSegConnection { get => previousSegConnection; set => previousSegConnection = value; }
    public GameObject NextSegConnection { get => nextSegConnection; set => nextSegConnection = value; }
    private HingeJoint2D m_hinge;

    private void Start()
    {
        m_hinge = GetComponent<HingeJoint2D>();
        PreviousSegConnection = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        LifelineSegment previousSeg = PreviousSegConnection.GetComponent<LifelineSegment>();

        if( previousSeg ){
            previousSeg.NextSegConnection = gameObject;
            float spriteBottom = previousSegConnection.GetComponent<SpriteRenderer>().bounds.size.y;
            m_hinge.connectedAnchor = new Vector2( 0, -spriteBottom );
        }
        else{
            m_hinge.connectedAnchor = Vector2.zero;
        }
    }
}
