using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LifelineSegment : MonoBehaviour
{
    private Light2D m_lightsource;
    public Light2D Lightsource { get => m_lightsource; set => m_lightsource = value; }
    private GameObject previousSegConnection;
    private GameObject nextSegConnection;
    public GameObject PreviousSegConnection { get => previousSegConnection; set => previousSegConnection = value; }
    public GameObject NextSegConnection { get => nextSegConnection; set => nextSegConnection = value; }
    private HingeJoint2D m_hinge;
    public HingeJoint2D Hinge { get => m_hinge; set => m_hinge = value; }

    private void Awake(){
        Lightsource = GetComponentInChildren<Light2D>();
        Hinge = GetComponent<HingeJoint2D>();
    }
    private void Start()
    {
        PreviousSegConnection = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        LifelineSegment previousSeg = PreviousSegConnection.GetComponent<LifelineSegment>();

        if( previousSeg ){
            previousSeg.NextSegConnection = gameObject;
            float spriteBottom = previousSegConnection.GetComponent<SpriteRenderer>().bounds.size.y;
            Hinge.connectedAnchor = new Vector2( 0, -spriteBottom );
        }
        else{
            Hinge.connectedAnchor = Vector2.zero;
        }
    }
}
