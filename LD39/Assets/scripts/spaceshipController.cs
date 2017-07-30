using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceshipController : MonoBehaviour {
    public Transform startPoint;
    //public Transform endPoint;
    public LineRenderer laserLine;
    public float attackDmg;
    public float reloadtime;
    public gameController gController;
    public AudioSource humm;
    float timer;
    public Transform NavTarget;
    public Vector3 targetPos;
    public float attackRange;
    public bool attacking = true;
    public float spd = 30;
    public bool attackRdy = true;

    virtual public void moveLogic() {
        if (NavTarget != null)
        {
            targetPos = NavTarget.position;
            if (Vector3.Distance(transform.position, targetPos) < attackRange)
            {
                attacking = true;
                
            }
            else {
                attacking = false;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x,targetPos.y+4,targetPos.z), spd * Time.deltaTime);
                
            }
        }
        else {
            findTarget();
            /*
            if (NavTarget == null) {
                Debug.Log("no target");
            }
            */
        }
    }
    virtual public void rotate() {
        transform.Rotate(new Vector3(0,Time.deltaTime*spd*15,0));
    }

    virtual public void laserLogic() {
        if (attacking)
        {
            if (humm.isPlaying == false)
            {
                humm.Play();
            }
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, targetPos);

            if (attackRdy)
            {
                if (NavTarget.tag == "Generator")
                {
                    NavTarget.GetComponent<generatorController>().alterHealth(-attackDmg);
                    attackRdy = false;
                }
                else if (NavTarget.tag == "Player")
                {
                    NavTarget.GetComponent<playerStats>().alterHealth(-attackDmg);
                    attackRdy = false;
                }
                else if (NavTarget.tag == "turret")
                {
                    NavTarget.GetComponent<laserController>().changeHealth(-attackDmg);
                    attackRdy = false;
                }
                else
                {
                    Debug.Log("my target isn't listed: " + NavTarget.tag);
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > reloadtime)
                {
                    attackRdy = true;
                    timer = 0.0f;

                }

            }
        }
        else
        {
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, startPoint.position);
            if (humm.isPlaying)
            {
                humm.Pause();
            }
        }
    }


    virtual public void findTarget()
    {
        //Debug.Log("find target running?");
        GameObject temp = null;
        float closest = Mathf.Infinity;
        float tempDistance;
        foreach (GameObject E in gController.friendlies)
        {
            if (E != null)
            {
                tempDistance = Vector3.Distance(transform.position, E.transform.position);
                if (tempDistance < closest)
                {
                    closest = tempDistance;
                    temp = E;
                }
            }
        }
        if (temp != null)
        {
            NavTarget = temp.transform;
            targetPos = NavTarget.position;
        }
    }
    virtual public void Start() {
        laserLine.startWidth = .4f;
        laserLine.endWidth = .4f;
    }

    virtual public void Update() {
        moveLogic();
        rotate();
        laserLogic();
    }


}
