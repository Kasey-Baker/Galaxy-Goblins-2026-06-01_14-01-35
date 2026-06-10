using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemySelfDestruct : MonoBehaviour
{
    [SerializeField] GameObject objectToTarget;
    [SerializeField] Vector3 pointDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectToTarget = GameObject.FindWithTag("Player"); //Replace this when game manager integrated

        pointDir = objectToTarget.transform.position - gameObject.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(pointDir.x, 0, pointDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
