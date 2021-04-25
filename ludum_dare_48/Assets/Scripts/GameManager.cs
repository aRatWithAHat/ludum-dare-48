using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_nbWrecksUntilBlackbox;
    [SerializeField] private GameObject m_salvagePrefab;
    private float m_distanceBetweenWrecks;
    private float m_distanceBetweenWrecksStep;
    private List<Salvage> m_salvagesTrail;
     private Salvage m_currentNextSalvage;

    private void Awake(){
        // Setting Salvage trail
        m_salvagesTrail[0] = GameObject.Find("FirstWreck").GetComponent<Salvage>();
        m_currentNextSalvage = m_salvagesTrail[0];
        for ( int i = 1; i < m_nbWrecksUntilBlackbox; i++ )
        {
            m_salvagesTrail[i] = Instantiate( m_salvagePrefab, DecideNextSalvagePosition( m_salvagesTrail[ i - 1 ].transform.position ), transform.rotation ).GetComponent<Salvage>();
            m_salvagesTrail[i].StopBlink( false );
        }

    }

    private Vector2 DecideNextSalvagePosition( Vector2 lastWreckPos ){
        return Vector2.zero;
    }
}
