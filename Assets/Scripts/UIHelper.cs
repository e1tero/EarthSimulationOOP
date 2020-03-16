using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{

    public ObjectOnMap objectOnMap;

    [SerializeField] Text humanCount;
    [SerializeField] Text cowsCount;
    [SerializeField] Text predatorsCount;

    void Update()
    {
        humanCount.text = objectOnMap.people.Count.ToString();
        cowsCount.text = objectOnMap.cows.Count.ToString();
        predatorsCount.text = objectOnMap.predators.Count.ToString();
    }
}
