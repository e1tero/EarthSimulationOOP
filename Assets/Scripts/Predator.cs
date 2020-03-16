using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creatures
{
    void Update()
    {
        if (Hunger())
            return;

        if (Reproduction())
            return;

        else
            Motion();
    }

    public override void DestroyThis()
    {
        objectOnMap.predators.Remove(this);
    }

    protected override void SearchOfFood()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        var result = SearchClosestOfRabbit(objectOnMap.rabbits, closest, position, distance);

        result = SearchClosestOfHuman(objectOnMap.people, result.c, position, result.d);

        result = SearchClosestOfGiraffe(objectOnMap.giraffes, result.c, position, result.d);
        closest = result.c;
    }

    protected override void PairSearch()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfPredator(objectOnMap.predators, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Rabbit") || coll.gameObject.CompareTag("Giraffe"))
        {
            coll.transform.GetComponent<Creatures>().DestroyThis();

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }


        else if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Predator"))
            {
                reproduction = false;
                objectOnMap.predators.Add(Instantiate(objectOnMap.predator, transform.position, Quaternion.identity).GetComponent<Predator>());
                closest.GetComponent<Predator>().reproduction = false;
            }
        }
    }
}
