using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StuffToRepair
{
    Music,
    GameDesign,
    QA,
    none
}

public class StuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab, enemyPrefab, strongEnemyPrefab;

    [SerializeField]
    private Transform[] enemySpawns;

    [SerializeField]
    private int numberOfEnemiesBySpawn = 3, numberOfStrongEnemiesBySpawn = 1;
    private int enemiesAlive = 0;


    [SerializeField]
    private Transform playerStartPos;
    

    private int levelReplay = 0;
    public StuffToRepair StuffToRepair { get; set; }
    
    private GameObject playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive = 0;
        playerInstance = Instantiate(playerPrefab, playerStartPos.position, new Quaternion(), transform);
        for(int i = 0; i < enemySpawns.Length; i++)
        {
            for(int j = 0; j < numberOfEnemiesBySpawn +levelReplay; j++)
            {
                Vector3 position = enemySpawns[i].position;
                position.x += Random.Range(-10, 10);
                position.y += Random.Range(-10, 10);
                GameObject go = Instantiate(enemyPrefab, position, new Quaternion(), enemySpawns[i]);
                go.GetComponent<Enemy>().stu = this;
                enemiesAlive++;
            }
        }
        if (StateMachine.Instance.GDRepaired)
        {
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                for (int j = 0; j < numberOfStrongEnemiesBySpawn + (levelReplay/3); j++)
                {
                    Vector3 position = enemySpawns[i].position;
                    position.x += Random.Range(-5, 5);
                    position.y += Random.Range(-5, 5);
                    GameObject go = Instantiate(strongEnemyPrefab, position, new Quaternion(), enemySpawns[i]);
                    go.GetComponent<Enemy>().stu = this;
                    enemiesAlive++;
                }
            }
        }
    }

    public void adjustNumberOfEnemies()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            switch (StuffToRepair)
            {
                case StuffToRepair.Music:
                    StateMachine.Instance.CurrentState = GameState.GameWon;
                    break;
                case StuffToRepair.GameDesign:
                    StateMachine.Instance.CurrentState = GameState.GameWon;
                    break;
                case StuffToRepair.QA:
                    StateMachine.Instance.CurrentState = GameState.GameWon;
                    break;
                case StuffToRepair.none:
                    levelReplay++;
                    StartCoroutine(waitBeforeRestart());
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator waitBeforeRestart()
    {
        yield return new WaitForSeconds(1);
        GameObject temp = playerInstance;
        playerInstance = null;
        Destroy(temp);
        yield return new WaitForSeconds(2);
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
