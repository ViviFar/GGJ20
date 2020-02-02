using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeStateButton : MonoBehaviour
{
    [SerializeField]
    private GameState stateToEnter;

    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { btnClicked(stateToEnter); });
    }

    private void btnClicked(GameState newState)
    {
        StateMachine.Instance.CurrentState = newState;
    }
}
