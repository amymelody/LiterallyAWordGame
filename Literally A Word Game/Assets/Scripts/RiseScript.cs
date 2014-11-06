using UnityEngine;
using System.Collections;

public class RiseScript : MonoBehaviour
{
    bool rising;
    float speed;

	void Start ()
    {
        rising = false;
        speed = 0.02f;
	}

	void Update ()
    {
	    if(rising)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + speed, gameObject.transform.position.z);
            if (gameObject.transform.position.y > 9.98f && gameObject.transform.position.y < 10)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 10, gameObject.transform.position.z);
                rising = false;
            }
            else if (gameObject.transform.position.y > 14.98f && gameObject.transform.position.y < 15)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 15, gameObject.transform.position.z);
                rising = false;
            }
            else if (gameObject.transform.position.y > 19.98f && gameObject.transform.position.y < 20)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 20, gameObject.transform.position.z);
                rising = false;
            }
            
            //Hold the letters to the platform
            GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");
            foreach(GameObject l in letters)
            {
                float myY = gameObject.transform.position.y;
                float letterY = l.transform.position.y;
                float letterSizeY = l.transform.lossyScale.y;
                if (myY + letterSizeY >= letterY && myY < letterY)
                {
                    float myX = gameObject.transform.position.x;
                    float mySizeX = gameObject.transform.lossyScale.x;
                    float letterX = l.transform.position.x;
                    float letterSizeX = l.transform.lossyScale.x;
                    if (myX - mySizeX / 1.9f <= letterX && myX + mySizeX / 2f > letterX)
                    {
                        float newY = gameObject.transform.position.y + gameObject.transform.lossyScale.y / 2.0f + letterSizeY / 2.0f;
                        l.transform.position = new Vector3(l.transform.position.x, newY, l.transform.position.z);
                    }
                }
            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (gameObject.transform.position.y < 20)
            {
                rising = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            rising = false;
        }
    }
}
