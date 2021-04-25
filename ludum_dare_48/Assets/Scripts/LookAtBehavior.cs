using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LookAtBehavior : MonoBehaviour
{
    private bool m_lookAt = false;
    private Transform m_target;

    public IEnumerator ForceLookAt( Transform target ){
        if( m_lookAt ) m_lookAt = false;
        float angle = EntityUtils.GetAngleBetweenPositions( transform.position, target.position );
        yield return transform.DORotate( new Vector3( 0, 0, angle - 90 ), 20 ).SetSpeedBased( true ).WaitForCompletion();
        m_target = target;
        m_lookAt = true; 
    }

    private void Update() {
        if( m_lookAt ){
            float angle = EntityUtils.GetAngleBetweenPositions( transform.position, m_target.position );
            transform.rotation = Quaternion.Euler( 0, 0, angle - 90 );
        }
    }
}
