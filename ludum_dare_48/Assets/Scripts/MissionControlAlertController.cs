using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MissionControlAlertController : MonoBehaviour
{
    public static MissionControlAlertController instance;
    private TextMeshProUGUI m_alertDisplay;
    private List<string> m_queuedMessages;
    private string _currentDisplayText;
    private bool _displayingMessage;
    private IEnumerator _currentMessageIEnumerator;
    private float _delay = 0.005f;

    private void Awake(){
        instance = this;
        m_alertDisplay = GetComponent<TextMeshProUGUI>();
        m_alertDisplay.SetText( "" );
        m_queuedMessages = new List<string>();
    }
    public void QueueNewAlert( string message ){
        m_queuedMessages.Add( message );
        if( !_displayingMessage ){
            StartCoroutine( DisplayMessage( message ) );
        }
    }

    public void OverrideDisplayedMessage( string message, bool destroyQueue = false ){
        StopAllCoroutines();
        /* Debug.Log("Old _queuedMessages");
        for( var i = 0; i < _queuedMessages.Count; i++ ){
            Debug.Log(i + " : " + _queuedMessages[i]);
        } */
        List<string> backupList =  new List<string>( m_queuedMessages );
        if( backupList.Count > 0 ){
            backupList.RemoveAt(0);
        } 
        m_queuedMessages.Clear();
        m_queuedMessages.Add( message );
        if( !destroyQueue ){
            m_queuedMessages.AddRange( backupList );
        }
        /* Debug.Log("New _queuedMessages");
        for( var i = 0; i < _queuedMessages.Count; i++ ){
            Debug.Log(i + " : " + _queuedMessages[i]);
        } */
        StartCoroutine( DisplayMessage( m_queuedMessages[0] ) );
    }

    private IEnumerator DisplayMessage( string baseText ){
        m_alertDisplay.SetText( "" );
        _displayingMessage = true;
        for(var i = 0; i <= baseText.Length; i++ ){
            _currentDisplayText = baseText.Substring( 0, i );
            m_alertDisplay.SetText( _currentDisplayText );
            yield return new WaitForSeconds( _delay );
        }
        yield return new WaitForSeconds( 4f );
        if( m_queuedMessages.Count == 1 ){
            yield return m_alertDisplay.DOColor( Color.clear, 0.75f ).WaitForCompletion();
        }
        m_alertDisplay.SetText( "" );
        m_queuedMessages.RemoveAt(0);
        _displayingMessage = false;
        m_alertDisplay.color = Color.white;
        /*for( var i = 0; i < m_queuedMessages.Count; i++ ){
            Debug.Log(i + " : " + m_queuedMessages[i]);
        }*/
        if(m_queuedMessages.Count > 0 ){
            StartCoroutine( DisplayMessage( m_queuedMessages[0] ) );
        }
    }
}
