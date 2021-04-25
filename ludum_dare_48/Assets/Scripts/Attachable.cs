using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    private Rigidbody2D m_body;
    public Rigidbody2D Body { get => m_body; set => m_body = value; }
    private HingeJoint2D m_hinge;
    public HingeJoint2D Hinge { get => m_hinge; set => m_hinge = value; }
    private bool m_isAttachable;
    public bool IsAttachable { get => m_isAttachable; set => m_isAttachable = value; }

    void Start()
    {
        IsAttachable = true;
        Body = GetComponentInParent<Rigidbody2D>();
        Hinge = GetComponentInParent<HingeJoint2D>();
        Hinge.connectedBody = Body;
    }

    private void OnTriggerEnter2D( Collider2D col ) {
        Debug.Log( "Can attach" );
        if( col.name == "AttachRange" ){
            col.GetComponentInParent<PlayerController>().AttachableInRange = this;
        }
    }

    private void OnTriggerExit2D( Collider2D col ) {
        Debug.Log( "Cannot attach" );
        if( col.name == "AttachRange" ){
            col.GetComponentInParent<PlayerController>().AttachableInRange = null;
        }
    }

    private void SetAttached(){
        IsAttachable = true;
        gameObject.SendMessageUpwards( "SetNewLights" );
    }
}
