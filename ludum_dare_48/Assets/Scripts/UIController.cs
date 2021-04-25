using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController inst;
    [SerializeField] public TextMeshProUGUI DroneNameDisplay;

    private void Awake() {
        inst = this;
    }

    public static void SetText( TextMeshProUGUI display, string text ){
        display.text = text;
    } 
}
