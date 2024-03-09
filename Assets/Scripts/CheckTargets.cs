using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargets : MonoBehaviour
{
    public List<Target> targets;
    public SceneTransition nextLevel;

    private void Start()
    {
        GameObject[] t = GameObject.FindGameObjectsWithTag("Target");

        foreach(GameObject g in t)
        {
            targets.Add(g.GetComponent<Target>());
        }
    }


    private void Update()
    {
        bool change = true;

        foreach (var target in targets)
        {
            if(target.state != target.targetState)
            {
                change = false;
            }
        }

        if(change)
        {
            nextLevel.ChangeLevel();
        }
        
    }
}
