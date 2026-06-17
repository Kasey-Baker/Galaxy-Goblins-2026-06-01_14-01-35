using UnityEngine;

public class FaceTowardsPlayer : MonoBehaviour
{
    [SerializeField] Vector3 pointToReach;
    [SerializeField] Vector3 myInitDirection;

    public bool doBehavior;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doBehavior)
        {
            if (gameObject.GetComponent<CharacterController>() != null)
            {
                //gameObject.GetComponent<CharacterController>().enabled = false;
            }
            FaceTowardsPlayerFunc();
        }
    }

    void FaceTowardsPlayerFunc()
    {
        pointToReach = GameManager.instance.player.transform.position;
        myInitDirection = pointToReach - gameObject.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(myInitDirection.x, 0, myInitDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f);
    }
}
