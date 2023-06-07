using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class HUD : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text timerText;

    void Update()
    {
        if(timerText != null && timer != null) {
            timerText.text = timer.GetString();
        }    
    }
}
