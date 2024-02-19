using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class ListCover : MonoBehaviour
    {
        private Cover[] coverLocations;
        

        private void Awake()
        {
            coverLocations = GetComponentsInChildren<Cover>();
            
        }

        public Cover UpdataNearestCover(Vector3 agentLocation)
        {
            Cover nearestCover = null;
            float minDistance = Mathf.Infinity;

            foreach (Cover cover in coverLocations)
            {
                float distance = Vector3.Distance(cover.transform.position, agentLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCover = cover;
                }
            }
            return nearestCover;
        }
    }
}


