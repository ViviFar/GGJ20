using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Menu,
    Credit,
    StoryText,
    CharacterChoice,
    SoundGame,
    DesignGame,
    QAGame,
    replayGame,
    LostGameScreen,
    GameWon,
}

public class StateMachine : MonoBehaviour
{
    #region singleton
    private static StateMachine instance;
    public static StateMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StateMachine>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<StateMachine>();
                    go.name = "StateMachine";
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private GameState currentState = GameState.Menu;
    public GameState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }
    private GameState oldState;
    [SerializeField]
    private Text nomStudio;
    [SerializeField]
    private Text winWave;
    [SerializeField]
    private AudioClip menuMusic, victoryMusic, defeatMusic;
    [SerializeField]
    private AudioClip brokenMusic, goodMusic, firstAouh;

    private AudioSource aSource;

    [SerializeField]
    private GameObject Menu, credits, storyText, characterChoice, soundGame, designGame, qAGame, replayGame, lostGameScreen, gameWonScreen, inGameCanvas;

    [SerializeField]
    private GameObject shootThemUpPrefab;

    private bool musicRepaired = false, qARepaired = false, gDRepaired = false;
    public bool MusicRepaired { get { return musicRepaired; } }
    public bool QARepaired { get { return qARepaired; } }
    public bool GDRepaired { get { return gDRepaired; } }

    private StuManager gameInstance;

    // Start is called before the first frame update
    void Start()
    {
        winWave.gameObject.SetActive(false);
        aSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        oldState = currentState;
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
        nomStudio.gameObject.SetActive(true);
        StartCoroutine(Demarage());
    }

    private IEnumerator Demarage()
    {
        aSource.clip = firstAouh;
        aSource.loop = false;
        aSource.Play();
        while (aSource.isPlaying)
        {
            yield return null;
        }
        nomStudio.gameObject.SetActive(false);
        onEnterMenuState();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldState != currentState)
        {
            switch (currentState)
            {
                case GameState.Menu:
                    onEnterMenuState();
                    break;
                case GameState.Credit:
                    onEnterCreditState();
                    break;
                case GameState.StoryText:
                    onEnterStoryTextState();
                    break;
                case GameState.CharacterChoice:
                    onEnterCharacterChoice();
                    break;
                case GameState.SoundGame:
                    onEnterSoundGameState();
                    break;
                case GameState.DesignGame:
                    onEnterDesignGameState();
                    break;
                case GameState.QAGame:
                    onEnterQAGameState();
                    break;
                case GameState.replayGame:
                    onEnterReplayGameState();
                    break;
                case GameState.LostGameScreen:
                    onEnterLostGameScreenState();
                    break;
                case GameState.GameWon:
                    onEnterGameWonState();
                    break;
                default:
                    break;
            }
            oldState = currentState;
        }
    }

    public void activateWinWaveTExt()
    {
        winWave.gameObject.SetActive(true);
    }

    public void deactivateWinWaveText()
    {
        winWave.gameObject.SetActive(true);
    }

    public void modifyWinWaveText(string newTExt)
    {
        winWave.text = newTExt;
    }

    #region enterStateFunction
    private void onEnterMenuState()
    {
        if (aSource.clip != menuMusic)
        {
            aSource.Stop();
            aSource.clip = menuMusic;
            aSource.loop = true;
            aSource.Play();
        }
        Menu.SetActive(true);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
    }
    private void onEnterReplayGameState()
    {
        aSource.Stop();
        aSource.clip = goodMusic;
        aSource.loop = true;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(true);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(true);

        GameObject go= Instantiate(shootThemUpPrefab, new Vector3(), new Quaternion(), replayGame.transform);
        gameInstance = go.GetComponent<StuManager>();
        gameInstance.StuffToRepair = StuffToRepair.none;
    }

    private void onEnterQAGameState()
    {
        aSource.Stop();
        if (musicRepaired)
        {
            aSource.clip = goodMusic;
        }
        else
        {
            aSource.clip = brokenMusic;
        }
        aSource.loop = true;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(true);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false); GameObject go = Instantiate(shootThemUpPrefab, new Vector3(), new Quaternion(), qAGame.transform);
        gameInstance = go.GetComponent<StuManager>();
        gameInstance.StuffToRepair = StuffToRepair.QA;
        inGameCanvas.SetActive(true);
        Debug.Log("qa repaired");
    }

    private void onEnterDesignGameState()
    {
        aSource.Stop();
        if (musicRepaired)
        {
            aSource.clip = goodMusic;
        }
        else
        {
            aSource.clip = brokenMusic;
        }
        aSource.loop = true;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(true);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        GameObject go = Instantiate(shootThemUpPrefab, new Vector3(), new Quaternion(), designGame.transform);
        gameInstance = go.GetComponent<StuManager>();
        gameInstance.StuffToRepair = StuffToRepair.GameDesign;
        inGameCanvas.SetActive(true);
        Debug.Log("gd repaired");
    }

    private void onEnterSoundGameState()
    {
        aSource.Stop();
        if (musicRepaired)
        {
            aSource.clip = goodMusic;
        }
        else
        {
            aSource.clip = brokenMusic;
        }
        aSource.loop = true;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(true);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        GameObject go = Instantiate(shootThemUpPrefab, new Vector3(), new Quaternion(), soundGame.transform);
        gameInstance = go.GetComponent<StuManager>();
        gameInstance.StuffToRepair = StuffToRepair.Music;
        inGameCanvas.SetActive(true);
        Debug.Log("zik repaired");
    }

    private void onEnterCharacterChoice()
    {
        if (aSource.clip != menuMusic)
        {
            aSource.Stop();
            aSource.clip = menuMusic;
            aSource.loop = true;
            aSource.Play();
        }
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(true);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
    }
    private void onEnterStoryTextState()
    {
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(true);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
    }

    private void onEnterCreditState()
    {
        if (aSource.clip != menuMusic)
        {
            aSource.Stop();
            aSource.clip = menuMusic;
            aSource.loop = true;
            aSource.Play();
        }
        Menu.SetActive(false);
        credits.SetActive(true);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
    }
    private void onEnterLostGameScreenState()
    {
        aSource.Stop();
        aSource.clip = defeatMusic;
        aSource.loop = false;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(true);
        gameWonScreen.SetActive(false);
        inGameCanvas.SetActive(false);
        GameObject temp = gameInstance.gameObject;
        gameInstance = null;
        Destroy(temp);
    }

    private void onEnterGameWonState()
    {
        aSource.Stop();
        aSource.clip = victoryMusic;
        aSource.loop = false;
        aSource.Play();
        Menu.SetActive(false);
        credits.SetActive(false);
        storyText.SetActive(false);
        characterChoice.SetActive(false);
        soundGame.SetActive(false);
        designGame.SetActive(false);
        qAGame.SetActive(false);
        replayGame.SetActive(false);
        lostGameScreen.SetActive(false);
        gameWonScreen.SetActive(true);
        inGameCanvas.SetActive(false);
        switch (gameInstance.StuffToRepair)
        {
            case StuffToRepair.Music:
                musicRepaired = true;
                break;
            case StuffToRepair.GameDesign:
                gDRepaired = true;
                break;
            case StuffToRepair.QA:
                qARepaired = true;
                break;
            default:
                break;
        }
        GameObject temp = gameInstance.gameObject;
        gameInstance = null;
        Destroy(temp);
    }
    #endregion

    #region updateStateFunction
    #endregion

    #region exitStateFunction()
    #endregion

}
