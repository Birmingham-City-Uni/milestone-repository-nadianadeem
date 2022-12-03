using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChecker : MonoBehaviour
{
    public Grid gridComp;
    public List<GameObject> unwalkableObjects;
    public List<Vector3> unwalkableObjectsTransform;

    public void Start()
    {
        foreach(GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if(obj.layer == 6)
            {
                unwalkableObjects.Add(obj);
                unwalkableObjectsTransform.Add(obj.transform.position);
            }
        }
        InvokeRepeating("UpdateMap", 0, 1f);
    }

    public void UpdateMap()
    {
        bool HasChanged = false;
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 6)
            {
                if (!unwalkableObjects.Contains(obj))
                {
                    unwalkableObjects.Add(obj);
                    unwalkableObjectsTransform.Add(obj.transform.position);
                    gridComp.CreateGrid();
                    HasChanged = true;
                }
                else
                {
                    if (unwalkableObjectsTransform[unwalkableObjects.IndexOf(obj)] != obj.transform.position)
                    {
                        unwalkableObjectsTransform[unwalkableObjects.IndexOf(obj)] = obj.transform.position;
                        gridComp.CreateGrid();
                        HasChanged = true;
                    }
                }
            }
        }

        for(int i = 0; i < unwalkableObjects.Count; i++)
        {
            if (unwalkableObjects[i] == null)
            {
                unwalkableObjects.RemoveAt(i);
                unwalkableObjectsTransform.RemoveAt(i);
                gridComp.CreateGrid();
                HasChanged = true;
            }
        }

        if (HasChanged)
        {
            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
            {
                agent.GetComponent<Agent>().gridHasChanged = true;
            }
        }
    }
}
