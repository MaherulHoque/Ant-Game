using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    public static Follower instance;

    [SerializeField]
    private PathCreator myPath;
    

    float updatePos;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
      

            if (myPath != null)
            {
                updatePos += Time.deltaTime * 2;

                transform.position = myPath.path.GetPointAtDistance(updatePos);
                Quaternion rotation = myPath.path.GetRotationAtDistance(updatePos);
                rotation.x = 0;
                rotation.y = 0;
                transform.rotation = rotation;

            }
    
  

    }

    public void GetPath(PathCreator newPath)
    {
        myPath = newPath;
    }

   
}
