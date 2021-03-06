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
    public bool IsStandalone { get => m_isStandalone; set => m_isStandalone = value; }

    [SerializeField] private AudioClip m_attachSound;
    [SerializeField] private AudioClip m_activationSound;

    [SerializeField] private FakePromptController m_prompts;
    [SerializeField] private bool m_isStandalone;

    void Awake()
    {
        IsAttachable = true;
        Body = GetComponentInParent<Rigidbody2D>();
        Hinge = GetComponentInParent<HingeJoint2D>();
        Hinge.connectedBody = Body;
    }

    private void OnTriggerEnter2D( Collider2D col ) {
        if( col.name == "AttachRange" && IsAttachable ){
            m_prompts.SetPromptVisible( "promptAttach" );
            UIController.inst.LifelineSubText.enabled = false;
            col.GetComponentInParent<PlayerController>().AttachableInRange = this;
        }
        else{
            //m_prompts.SetPromptVisible( "promptLaunchFlare" );
        }
    }

    private void OnTriggerExit2D( Collider2D col ) {
        if( col.name == "AttachRange" ){
            m_prompts.SetPromptInvisible();
            if( GameManager.instance.Player.AttachedToMainLifeline ){
                UIController.inst.LifelineSubText.enabled = true;
            }
            
            col.GetComponentInParent<PlayerController>().AttachableInRange = null;
        }
    }

    public void SetAttached(){
        IsAttachable = false;
        AudioSource.PlayClipAtPoint( m_attachSound, transform.position + new Vector3( 0, 0, -5 ) );
        MissionControlAlertController.instance.QueueNewAlert( "//: LIFELINE CONNECTION SUCCESSFUL" );
        m_prompts.SetPromptInvisible();

        if( !IsStandalone ){
            AudioSource.PlayClipAtPoint( m_activationSound, transform.position + new Vector3( 0, 0, -5 ) );
            gameObject.SendMessageUpwards( "SetAttachedToMainLifeline" );
        }
        
        
        
    }
}
