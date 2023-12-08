using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLight : MonoBehaviour
{
    public GameObject RedWall; // Reference to the RedWall child object
    public GameObject YellowWall; // Reference to the YellowWall child object
    private bool isRed = false; // Tracks the state of the light
    public float greenTime = 20;
    public float redTime = 5;
    public float yellowTime = 2;
    private Coroutine lightControlCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        lightControlCoroutine = StartCoroutine(LightControl());
    }

    IEnumerator LightControl()
    {
        while (true) // Infinite loop
        {
            if (isRed)
            {
                // Turn off RedWall
                RedWall.SetActive(false);

                // Turn on YellowWall for the period before green
                YellowWall.SetActive(true);
                yield return new WaitForSeconds(yellowTime);

                // Turn off YellowWall and keep for green duration
                YellowWall.SetActive(false);
                yield return new WaitForSeconds(greenTime - yellowTime); // Adjust green time

                isRed = false;
            }
            else
            {
                // Turn on YellowWall for the period before red
                YellowWall.SetActive(true);
                yield return new WaitForSeconds(yellowTime);

                // Turn off YellowWall and turn on RedWall
                YellowWall.SetActive(false);
                RedWall.SetActive(true);
                yield return new WaitForSeconds(redTime + yellowTime); // Adjust red time to include yellow phase

                isRed = true;
            }
        }
    }
    public void ResetStopLight()
    {
        if (lightControlCoroutine != null)
        {
            StopCoroutine(lightControlCoroutine);
        }
        isRed = false;
        RedWall.SetActive(false);
        YellowWall.SetActive(false);
        lightControlCoroutine = StartCoroutine(LightControl());
    }
}