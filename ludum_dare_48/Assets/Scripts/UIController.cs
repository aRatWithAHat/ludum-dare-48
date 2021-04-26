using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController inst;
    [SerializeField] public TextMeshProUGUI DroneNameDisplay;
    [SerializeField] public GameObject Pause;
    [SerializeField] public GameObject Startup;
    [SerializeField] public TextMeshProUGUI FlareDisplay;
    [SerializeField] public TextMeshProUGUI LifelineText;
    [SerializeField] public TextMeshProUGUI LifelineSubText;
    [SerializeField] public Slider LifeLineSlider;


    private void Awake() {
        inst = this;
        Pause.SetActive( false );
    }

    private void Update() {
        if( Input.GetKeyDown( KeyCode.Escape ) || Input.GetKeyDown(KeyCode.P ) ){
            if( Pause.activeInHierarchy ){
                Time.timeScale = 1;
                Pause.SetActive( false );
            }
            else{
                Time.timeScale = 0;
                Pause.SetActive( true );
            }
        }
    }

    public void Resume(){
        Time.timeScale = 1;
        Pause.SetActive( false );
    }
    public void Quit(){
        Application.Quit();
    }
}
