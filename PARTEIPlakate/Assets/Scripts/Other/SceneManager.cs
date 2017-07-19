using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneManager : MonoBehaviour
{

    [SerializeField]
    int lastAmount = 0;

    [SerializeField]
    GameObject visualPrefab;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SlowUpdate());
    }


    IEnumerator SlowUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            UpdateTargets();
        }
    }

    private void UpdateTargets()
    {
        ImageTargetBehaviour[] targets = FindObjectsOfType<ImageTargetBehaviour>();

        if (targets.Length == lastAmount)
        {
            return;
        }
        lastAmount = targets.Length;

        foreach (var o in targets)
        {
            if (o.GetComponent<EventHandler>())
            {
                continue;
            }
            SetupTarget(o.gameObject);
        }
    }

    private void SetupTarget(GameObject target)
    {
        GameObject vis = Instantiate(visualPrefab, target.transform);
        target.AddComponent<EventHandler>();
        vis.transform.localScale = new Vector3(0.067f, 1, 0.1f);
    }
}
