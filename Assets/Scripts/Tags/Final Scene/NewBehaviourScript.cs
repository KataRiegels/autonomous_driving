using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cars
{

public List<Cars> carObjects;

    public Cars()
    {
        carObjects = new List<Cars>();
    }
}
public class NewBehaviourScript : MonoBehaviour
{
    public List<Cars> carList = new List<Cars>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

public Cars GetCarsByName(string name)
    {

        return null;
    }
}
