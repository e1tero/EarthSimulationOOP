using UnityEngine;
using System.Collections.Generic;
using System;

public class Giraffe : Creatures
{
    void Update()
    {
        if (Hunger())
            return;

        if (Reproduction())
            return;

        Motion();
    }

    public override void DestroyThis()
    {
        objectOnMap.giraffes.Remove(this);
    }

    protected override void SearchOfFood()
    {
        // Поиск  ближайшей еды

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;


        var result = SearchClosestOfPlant(objectOnMap.bushes, closest, position, distance);
        closest = result.c;
    }

    protected override void PairSearch()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfGiraffe(objectOnMap.giraffes, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Giraffe"))
            {
                reproduction = false;
                objectOnMap.giraffes.Add(Instantiate(objectOnMap.giraffe, transform.position, Quaternion.identity).GetComponent<Giraffe>());
                closest.GetComponent<Giraffe>().reproduction = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (hunger == true && coll.gameObject.tag == "Bush")
        {
            objectOnMap.bushes.Remove(coll.transform); // Уничтожить съеденную еду из списка

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }

    }
}