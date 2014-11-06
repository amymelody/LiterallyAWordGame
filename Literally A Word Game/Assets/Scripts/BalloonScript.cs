using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour
{
    Quaternion rotation;

    void Start()
    {
        gameObject.rigidbody.freezeRotation = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Floor"))
        {
            GameObject.Find("Lee").GetComponent<LeeScript>().setYVelocity(0);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Floor"))
        {
            LeeScript ls = GameObject.Find("Lee").GetComponent<LeeScript>();
            ls.setYVelocity(ls.movementSpeed);
        }
    }
}
