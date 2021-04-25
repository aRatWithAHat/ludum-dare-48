using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePromptController : MonoBehaviour
{
    private Dictionary<string, GameObject> m_prompts;

    private GameObject m_currentPrompt;

    private void Awake(){

        m_prompts = new Dictionary<string, GameObject>();
        SpriteRenderer[] allPrompts = GetComponentsInChildren<SpriteRenderer>();

        foreach ( SpriteRenderer prompt in allPrompts ){
            m_prompts.Add( prompt.gameObject.name, prompt.gameObject );
            prompt.gameObject.SetActive( false );
        }
    }

    public void SetPromptVisible( string key ){
        SetPromptInvisible();
        m_currentPrompt = m_prompts[ key ];
        m_prompts[ key ].SetActive( true );
    }
    public void SetPromptInvisible(){
        if( m_currentPrompt ){
            if( m_currentPrompt.activeInHierarchy ){
            m_currentPrompt.SetActive( false );
            m_currentPrompt = null;
        }
        }
    }
}
