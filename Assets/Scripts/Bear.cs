using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Creatures
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
        objectOnMap.bears.Remove(this);
    }

    protected override void SearchOfFood()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        // Ближайшая корова
        var result = SearchClosestOfCow(objectOnMap.cows, closest, position, distance);
        // Ближайший кролик 
        result = SearchClosestOfRabbit(objectOnMap.rabbits, result.c, position, result.d);
        // Ближайший хищник
        result = SearchClosestOfPredator(objectOnMap.predators, result.c, position, result.d);
        // Ближайшай человек
        result = SearchClosestOfHuman(objectOnMap.people, result.c, position, result.d);
        closest = result.c;
    }

    protected override void PairSearch()
    {
        if (closest) return;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfBear(objectOnMap.bears, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (hunger == true && (coll.gameObject.CompareTag("Cow") || coll.gameObject.CompareTag("Rabbit") || coll.gameObject.CompareTag("Predator") || coll.gameObject.CompareTag("Human")))
        {
            coll.transform.GetComponent<Creatures>().DestroyThis();

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }


        else if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Bear"))
            {
                reproduction = false;
                objectOnMap.bears.Add(Instantiate(objectOnMap.bear, transform.position, Quaternion.identity).GetComponent<Bear>());
                closest.GetComponent<Bear>().reproduction = false;
            }
        }
    }
}
