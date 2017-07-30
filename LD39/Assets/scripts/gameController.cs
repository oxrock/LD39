using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameController : MonoBehaviour {
    
    public GameObject GO_Screen;
    public List<Transform> spawnPoints;
    public List<GameObject> monsterFabs;
    public GameObject spaceship;
    public float spawnrate = 6.0f;
    public bool GeneratorAlive = true;
    float timer = 0.0f;
    bool spawnRdy = true;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> friendlies;
    public Text clockText;
    public Text GO_text;
    public Text AnnouncementText;
    bool updateclock = false;
    int secondsAlive = 0;
    int minutesAlive = 0;
    float timeAlive = 0.0f;
    int score = 0;
    bool announcing = false;
    int fadeTime = 6;
    int monsterIndexCap = 1;
    float hpMultiplier = 1;

    /*
    GameObject go = new GameObject("Target");
    Vector3 sourcePostion = new Vector3(100, 20, 100);//The position you want to place your agent
    NavMeshHit closestHit;
if(NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1 ) ){
  go.transform.position = closestHit.position;
  go.AddComponent<NavMeshAgent>();
  //TODO
}
else{
  Debug.Log("...");
}
*/
    void AnnouncementFade() {
        if (secondsAlive == fadeTime) {
            AnnouncementText.enabled = false;
            announcing = false;
        }
    }

    void makeAnnouncement(string message) {
        AnnouncementText.text = message;
        AnnouncementText.enabled = true;
        announcing = true;

    }

    public void adjustScore(int Amount) {
        score += Amount;
    }
    void spawnMonster() {
        NavMeshHit hit;
        Transform spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        Vector3 desiredPosition = (spawn.transform.position);
        //if (NavMesh.SamplePosition(spawn.position, out hit, 7.0f, NavMesh.AllAreas))
        if (NavMesh.SamplePosition(desiredPosition, out hit, 500, 1))
        {
            GameObject chosenFab = monsterFabs[Random.Range(0, monsterIndexCap)];
            GameObject monster = Instantiate<GameObject>(chosenFab, hit.position,Quaternion.identity);
            if (chosenFab == spaceship)
            {
                monster.GetComponent<spaceshipController>().gController = this;
                monster.transform.position = new Vector3(hit.position.x, hit.position.y + 10, hit.position.z);
            }
            else {
                monster.GetComponent<basicEnemyController>().gController = this;
            }
            
            healthController monsterHC = monster.GetComponent<healthController>();
            monsterHC.gController = this;
            monsterHC.maxHealth =  Mathf.FloorToInt(monsterHC.maxHealth * hpMultiplier);
            monsterHC.health = monsterHC.maxHealth;
            spawnRdy = false;
            //spawnTime = Random.Range(minRate, maxRate);
            timer = 0.0f;
            enemies.Add(monster);
        }
        else {
            Debug.Log("epic fail!");
        }
    }

    public void restartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("default");
    }

    public void GameOver() {
        Cursor.lockState = CursorLockMode.None;
        GO_Screen.SetActive(true);
        GO_text.text = "You survived " + minutesAlive.ToString() + " minutes earning a total of " + score.ToString() + " points.";
        Time.timeScale = 0;
    }

    public void removeMonster(GameObject deadEnemy) {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i] = deadEnemy) {
                enemies.RemoveAt(i);
                return;
            }
        }
    }
    void difficultyIncrease() {
        switch (minutesAlive)
        {
            case 1:
                makeAnnouncement("A new alien joins the fight");
                monsterIndexCap += 1;
                break;
            case 2:
                makeAnnouncement("A new alien joins the fight");
                monsterIndexCap += 1;
                break;

            case 3:
                makeAnnouncement("Alien spawning speed increased!");
                spawnrate -= 1;
                break;

            case 4:
                makeAnnouncement("A new alien joins the fight");
                monsterIndexCap += 1;
                break;

            case 5:
                makeAnnouncement("Alien spawning speed increased!");
                spawnrate -= 1;
                break;
            
            case 6:
                makeAnnouncement("A new alien joins the fight");
                monsterIndexCap += 1;
                break;

            /*
            case 7:

            case 8:

            case 9:

            case 10:
            */


            default:

                if (spawnrate <= 1)
                {
                    makeAnnouncement("Alien HP increased by 10%");
                    hpMultiplier += .1f;
                }
                else {
                    makeAnnouncement("Alien spawning speed increased!");
                    spawnrate -= 1;
                }
                break;
        }
    }
    void updateTime()
    {
        updateclock = false;
        timeAlive += Time.deltaTime;
        if (timeAlive > 1)
        {
            secondsAlive++;
            timeAlive--;
            updateclock = true;
        }
        if (secondsAlive > 59)
        {
            secondsAlive = 0;
            minutesAlive++;
            updateclock = true;
            difficultyIncrease();
        }
        if (updateclock)
        {
            clockText.text = minutesAlive.ToString("00") + ":" + secondsAlive.ToString("00");
        }
    }
    public void removeFriendly(GameObject deadFriendly)
    {
        for (int i = 0; i < friendlies.Count; i++)
        {
            if (friendlies[i] == deadFriendly)
            {
                friendlies.RemoveAt(i);
                if (deadFriendly.tag == "turret") {
                    deadFriendly.GetComponent<laserController>().Death();
                }
                return;
            }
        }
    }

    void spawnTimerHandler() {
        if (!spawnRdy)
        {
            timer += Time.deltaTime;
            if (timer > spawnrate)
            {
                spawnRdy = true;
                spawnMonster();
            }
        }
        else {
            spawnMonster();
        }
    }

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        makeAnnouncement("Prepare yourself for the alien invasion!");
    }
	
	// Update is called once per frame
	void Update () {
        spawnTimerHandler();
        if (Input.GetButtonDown("Cancel")) {
            if (Cursor.lockState != CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        updateTime();
        if (announcing) {
            AnnouncementFade();
        }
	}
}
