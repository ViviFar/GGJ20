using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button musicButton, gDButton, qAButton, replayButton;

    private void OnEnable()
    {
        musicButton.interactable = !StateMachine.Instance.MusicRepaired;
        gDButton.interactable = !StateMachine.Instance.GDRepaired;
        qAButton.interactable = !StateMachine.Instance.QARepaired;
        replayButton.interactable = StateMachine.Instance.MusicRepaired && StateMachine.Instance.GDRepaired && StateMachine.Instance.QARepaired;
    }
}
