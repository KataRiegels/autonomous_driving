using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLight : MonoBehaviour
{
    public GameObject RedWall; // Reference to the RedWall child object
    public GameObject YellowWall; // Reference to the YellowWall child object
    private enum LightState { Red, Yellow, Green }
    private LightState currentState;
    public float greenTime = 20;
    public float redTime = 5;
    public float yellowTime = 2;
    private Coroutine lightControlCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomInitialState();
        lightControlCoroutine = StartCoroutine(LightControl());
    }

    IEnumerator LightControl()
    {
        while (true) // Infinite loop
        {
            switch (currentState)
            {
                case LightState.Green:
                    // Green phase (both lights off)
                    RedWall.SetActive(false);
                    YellowWall.SetActive(false);
                    yield return new WaitForSeconds(greenTime);
                    currentState = LightState.Yellow;
                    break;

                case LightState.Yellow:
                    // Yellow phase
                    YellowWall.SetActive(true);
                    RedWall.SetActive(false);
                    yield return new WaitForSeconds(yellowTime);
                    currentState = LightState.Red;
                    break;

                case LightState.Red:
                    // Red phase
                    RedWall.SetActive(true);
                    YellowWall.SetActive(false);
                    yield return new WaitForSeconds(redTime);
                    currentState = LightState.Green;
                    break;
            }
        }
    }

    private void SetRandomInitialState()
    {
        float randomValue = Random.value;
        if (randomValue < 0.33f)
        {
            currentState = LightState.Green;
        }
        else if (randomValue < 0.66f)
        {
            currentState = LightState.Yellow;
        }
        else
        {
            currentState = LightState.Red;
        }
    }

    public void ResetStopLight()
    {
        if (lightControlCoroutine != null)
        {
            StopCoroutine(lightControlCoroutine);
        }
        SetRandomInitialState();
        lightControlCoroutine = StartCoroutine(LightControl());
    }
}
