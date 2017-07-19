using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button Click Effect
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{
    private Vector3 from = Vector3.one;
    public float factor = 0.9f;

    public float duration = 0.1f;

    bool isRunning = false;

    void Start()
    {
        from = GetComponent<RectTransform>().localScale;

        GetComponent<Button>().onClick.AddListener(() =>
        StartCoroutine(Scaling()));
      //  GetComponent<RectTransform>().localScale = from;
    }

    IEnumerator Scaling()
    {
        if (!isRunning)
        {
            isRunning = true;
            GetComponent<RectTransform>().localScale = from*factor;
            yield return new WaitForSeconds(duration);
            GetComponent<RectTransform>().localScale = from ;
            isRunning = false;
        }
    }
}