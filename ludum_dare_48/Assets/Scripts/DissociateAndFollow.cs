using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissociateAndFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private void Start(){
        transform.SetParent( null );
        transform.rotation = Quaternion.Euler( Vector3.zero );
    }

    private void Update(){
        transform.position = targetTransform.position;
    }
}
