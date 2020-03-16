using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Creatures
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
        objectOnMap.wolves.Remove(this);
    }

    protected override void SearchOfFood()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        var result = SearchClosestOfRabbit(objectOnMap.rabbits, closest, position, distance);

        result = SearchClosestOfBear(objectOnMap.bears, result.c, position, result.d);
        closest = result.c;
    }

    protected override void PairSearch()
    {
        if (closest) return;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfWold(objectOnMap.wolves, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (hunger == true && (coll.gameObject.CompareTag("Bear") || coll.gameObject.CompareTag("Rabbit")))
        {
            coll.transform.GetComponent<Creatures>().DestroyThis();

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }


        else if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Wolf"))
            {
                reproduction = false;
                objectOnMap.wolves.Add(Instantiate(objectOnMap.wolf, transform.position, Quaternion.identity).GetComponent<Wolf>());
                closest.GetComponent<Wolf>().reproduction = false;
            }
        }
    }
}
