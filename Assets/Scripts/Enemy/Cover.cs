using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Enemy
{
    public class Cover : MonoBehaviour
    {
        private coverMask[] coverMasks;
        public Transform coverPos;
        

        private void Awake()
        {
          
            coverMasks = GetComponentsInChildren<coverMask>();
            coverPos = gameObject.transform;
        }

        public Transform UpdataNearestCover(Vector3 agentLocation)
        {
            Transform nearestCover = null;
            float minDistance = Mathf.Infinity;
            

            foreach (coverMask cover in coverMasks)
            {
                if(cover.occupied == false)
                {
                    float distance = Vector3.Distance(cover.transform.position, agentLocation);
                    if ((distance < minDistance))
                    {
                        minDistance = distance;
                        nearestCover = cover.transform;
                    }
                }
                
            }
            
            Debug.Log(name+"cover" + nearestCover.name);
            return nearestCover;
        }

    }

    
}

