using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Motion : MonoBehaviour
{
    public float leftBound;
    public float rightBound;
    private float speed;

    private bool leftSide;
    private bool rightSide;
    private bool working;

    void Awake()
    {
        leftSide = true;
        rightSide = false;
        working = false;
    }

    private void Start()
    {
        float x = UnityEngine.Random.Range(leftBound, rightBound);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, GetComponent<RectTransform>().anchoredPosition.y, GetComponent<RectTransform>().anchoredPosition.x);

        float randomSpeed = UnityEngine.Random.Range(0.15f, 0.25f);
        speed = randomSpeed;
    }

    void Update()
    {
        Working();
    }

    private void Working()
    {
        if (working == false)
        {
            working = true;
        }

        if (working == true)
        {
            if (leftSide == true && rightSide == false)
            {
                if (GetComponent<RectTransform>().anchoredPosition.x < rightBound)
                {
                    GoToRight(speed);
                }
                else
                {
                    leftSide = false;
                    rightSide = true;
                    working = false;
                    Swap();
                }
            }
            else if (rightSide == true && leftSide == false)
            {
                if (GetComponent<RectTransform>().anchoredPosition.x > leftBound)
                {
                    GoToLeft(speed);
                }
                else
                {
                    leftSide = true;
                    rightSide = false;
                    working = false;
                    Swap();
                }
            }
            else
            {
                throw new System.Exception("Unavailable status for random bonus!");
            }
        }
    }

    private void Swap()
    {
        var x = GetComponent<RectTransform>().localScale.x;
        GetComponent<RectTransform>().localScale = new Vector3(-x, GetComponent<RectTransform>().localScale.y, GetComponent<RectTransform>().localScale.z);
    }

    private void GoToLeft(float speed)
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void GoToRight(float speed)
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}