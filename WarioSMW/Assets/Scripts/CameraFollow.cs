using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    float xMin, xMax;
    [SerializeField]
    float yMin, yMax;

    private Transform target;
    private Transform player;
    void Start()
    {
        
        player = GameObject.Find("Player").transform;
                
    }

    void Update()
    {
        Transform playerSmall = player.Find("Player Small");
        Transform playerBig = player.Find("Player Big");
        if (playerSmall.gameObject.activeSelf)
        {
            target = playerSmall;
        }
        else
        {
            target = playerBig;
        }
    }
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
    }

}
