using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPrompt : MonoBehaviour
{
    private Attachable m_linkedAttachable;
    private bool m_isAttachable;
    public bool IsAttachable { get => m_linkedAttachable.IsAttachable; }

    private void Awake(){
       //m_linkedAttachable = GetComponent<Attachable>();
   }

    private void OnTriggerEnter2D( Collider2D col ) {
        Debug.Log( "Can attach" );
    }

    private void OnTriggerExit2D( Collider2D col ) {
        Debug.Log( "Cannot attach" );
    }
}
