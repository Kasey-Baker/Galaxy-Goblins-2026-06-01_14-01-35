using UnityEngine;

public class DashToPoint : MonoBehaviour
{
    [SerializeField] Vector3 pointToReach;
    [SerializeField] float timeToReachPoint;

    Vector3 myInitDirection;

    Quaternion myFinalDirection;

    float waitTime;

    float baseMoveSpeed;

    bool moveSet;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime = 0;
        moveSet = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!moveSet)
        {
            waitTime += Time.deltaTime;
            if(waitTime >= timeToReachPoint)
            {
                BehaviorAfterPointReach();
            }
        }

    }

    public void SetPoint(Vector3 newPoint)
    {
        pointToReach = newPoint;
    }

    public void TargetPoint()
    {
        myInitDirection = pointToReach - gameObject.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, myInitDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 100f);


        float tempMoveSpeed = (Vector3.Distance(transform.position, pointToReach))/timeToReachPoint;
        gameObject.GetComponent<EnemyDashingShip>().SetMoveSpeed(tempMoveSpeed);


    }

    void BehaviorAfterPointReach()
    {
        transform.rotation = myFinalDirection;
        gameObject.GetComponent<EnemyDashingShip>().SetMoveSpeed(baseMoveSpeed);
    }

    public void SetFinalDirection(Quaternion finalDir)
    {
        myFinalDirection = finalDir;
    }

    public void CalculateBaseMoveSpeed()
    {
        baseMoveSpeed = gameObject.GetComponent<EnemyDashingShip>().GetMoveSpeed();
    }
}
