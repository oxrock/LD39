using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class basicEnemyController : MonoBehaviour {

    public Transform NavTarget;
    public NavMeshAgent myAgent;
    public Vector3 targetPos = Vector3.zero;
    public gameController gController;
    public float attackDmg;
    public float attackCooldown;
    public float attackRange;
    public float attackTimer = 0.0f;
    public bool attackRdy = true;
    public bool attacking = false;

    virtual public void setTarget(Transform _target) {
        if (_target != null)
        {
            NavTarget = _target;
        }
        
    }


    virtual public void attackLogic() {
        if (!attackRdy)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCooldown)
            {
                attackTimer = 0.0f;
                attackRdy = true;
            }
        }
        else
        {
            if (NavTarget != null)
            {
                if (Vector3.Distance(transform.position, NavTarget.position) < attackRange)
                {
                    attacking = true;
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
                else {
                    attacking = false;
                }
            }
            else {
                findTarget();
            }
        }
    }

    virtual public void findTarget()
    {
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
        //Debug.Log("only null targets");
    }


    virtual public void movement()
    {
        if (myAgent.isOnNavMesh)
        {
            if (NavTarget != null)
            {
                if (targetPos != NavTarget.position)
                {
                    targetPos = NavTarget.position;
                }
                if (targetPos != myAgent.destination)
                {
                    myAgent.SetDestination(targetPos);
                }
            }
            else {
                findTarget();
            }
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(gameObject.transform.position, out hit, 5.0f, NavMesh.AllAreas))
            {
                gameObject.transform.position = hit.position;
            }
        }
    }

    virtual public void Start () {
        //temporary to get things moving...
        //GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
	}

    // Update is called once per frame
    virtual public void Update () {
        movement();
        attackLogic();
	}
}
