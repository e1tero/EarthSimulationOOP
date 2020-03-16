using UnityEngine;
using System.Collections.Generic;
using System;

public class Rabbit : Creatures
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
        objectOnMap.rabbits.Remove(this);
    }

    protected override void SearchOfFood()
    {
        // Поиск  ближайшей еды

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        var result = SearchClosestOfPlant(objectOnMap.flowers, closest, position, distance);
        closest = result.c;

    }

    protected override void PairSearch()
    {
        if (closest) return;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfRabbit(objectOnMap.rabbits, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Rabbit"))
            {
                reproduction = false;
                objectOnMap.rabbits.Add(Instantiate(objectOnMap.rabbit, transform.position, Quaternion.identity).GetComponent<Rabbit>());
                closest.GetComponent<Rabbit>().reproduction = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)      
    {
        if (hunger == true && coll.gameObject.tag == "Flower")
        {
            objectOnMap.flowers.Remove(coll.transform); // Уничтожить съеденную еду из списка

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }

    }
}