using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool m_expeditionStarted = false;
    private int tries = 1;
    [SerializeField] private int m_nbWrecksUntilBlackbox;
    [SerializeField] private GameObject m_salvagePrefab;
    [SerializeField] private GameObject m_blackBoxPrefab;
    [SerializeField] private float m_distanceBetweenWrecksBase;
    [SerializeField] private float m_distanceBetweenWrecksStep;
    [SerializeField] private List<Salvage> m_salvagesTrail;
    [SerializeField] private List<Salvage> m_linkedSalvage;
    [SerializeField] public Salvage CurrentNextSalvage;
    public bool ExpeditionStarted { get => m_expeditionStarted; set => m_expeditionStarted = value; }

    public PlayerController Player;

    private void Start(){

        instance = this;

        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Setting Salvage trail
        m_salvagesTrail = new List<Salvage>();
        m_linkedSalvage = new List<Salvage>();
        m_salvagesTrail.Add( GameObject.Find( "FirstWreck" ).GetComponent<Salvage>() );
        CurrentNextSalvage = m_salvagesTrail[0];
        for ( int i = 1; i < m_nbWrecksUntilBlackbox; i++ )
        {
            m_salvagesTrail.Add( Instantiate( m_salvagePrefab, Vector2.zero, transform.rotation ).GetComponent<Salvage>() );
            m_salvagesTrail[i].StopBlink( false );
            m_salvagesTrail[i].name = "salvage_" + i;
            m_salvagesTrail[i].transform.position = DecideNextSalvagePosition( m_salvagesTrail[ i - 1 ].transform.position, i );
        }

        m_salvagesTrail.Add( Instantiate( m_blackBoxPrefab, DecideNextSalvagePosition( m_salvagesTrail[ m_salvagesTrail.Count - 1 ].transform.position, m_salvagesTrail.Count ), transform.rotation ).GetComponent<Salvage>() );

        UIController.inst.LifelineText.SetText( "LIFELINE //: <color=#ffc216>[MOUSE WHEEL]</color>" );
        UIController.inst.LifelineSubText.SetText( "DISCONNECT //: <color=#ffc216>[SPACEBAR]</color>" );

        // Flavor

        UIController.inst.Startup.SetActive( true );
        MissionControlAlertController.instance.OverrideDisplayedMessage( "//: BOOT UP" );
        MissionControlAlertController.instance.QueueNewAlert( "//: DRN.OS BOOTING UP COMPLETE" );
        MissionControlAlertController.instance.QueueNewAlert( "//: AWAITING PILOT CONFIRMATION" );

    }

    private void StartupPlayerShip(){
        MissionControlAlertController.instance.OverrideDisplayedMessage("//: LAUNCHING DRN-" + tries, true );
        MissionControlAlertController.instance.QueueNewAlert("//: NEW DIRECTIVE: RETRIEVE MIA BLACK BOX");
        MissionControlAlertController.instance.QueueNewAlert("//: PROPOSAL: FOLLOW DRN UNITS WRECKAGES TO ESTIMATED TARGET LOCATION");

        Player.enabled = true;
        Player.SetupNewRun( tries );
    }

    private Vector2 DecideNextSalvagePosition( Vector2 lastWreckPos, int currentIndex ){
        float angle = Random.Range( -60, 60 );
        float radians = angle * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);

        float distance = m_distanceBetweenWrecksBase + ( currentIndex * m_distanceBetweenWrecksStep );

        return lastWreckPos + ( new Vector2( x, y ) * distance );
    }

    public void ChangeCurrentObjective( Salvage oldObj ){
        m_salvagesTrail.Remove( oldObj );
        m_linkedSalvage.Add( oldObj );

        if( oldObj.IsBlackBox ){
            MissionControlAlertController.instance.OverrideDisplayedMessage("//: BLACK BOX SECURED BY LIFELINE");
            MissionControlAlertController.instance.QueueNewAlert("//: DEEP DIVE COMPLETED");
            
        }
        else{
            CurrentNextSalvage = m_salvagesTrail[0];
        }
    }

    private void Update(){
        if( !ExpeditionStarted && Input.GetKeyDown( KeyCode.Space) ){
            ExpeditionStarted = true;
            UIController.inst.Startup.SetActive( false );
            StartupPlayerShip();
        }
    }
}
