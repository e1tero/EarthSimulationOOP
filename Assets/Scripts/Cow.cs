using UnityEngine;
using System.Collections.Generic;
using System;

public class Cow : Creatures
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
        objectOnMap.cows.Remove(this);
    }

    protected override void SearchOfFood()
    {
        // Поиск  ближайшей еды
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        // Поиск дерева
        var result = SearchClosestOfPlant(objectOnMap.trees, closest, position, distance);

        // Поиск отравленного дерева
        result = SearchClosestOfPlant(objectOnMap.poisonTrees, result.c, position, result.d);
        closest = result.c;
    }

    protected override void PairSearch()
    {

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfCow(objectOnMap.cows, closest, position, distance).c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (hunger == true && coll.gameObject.tag == "Tree")
        {
            objectOnMap.trees.Remove(coll.transform); // Уничтожить съеденную еду из списка

            Destroy(coll.gameObject);
            hunger = false;

            closest = null;
        }

        else if (hunger == true && coll.gameObject.tag == "PoisonTree")
        {

            transform.GetComponent<Cow>().DestroyThis();
            Destroy(gameObject);
            objectOnMap.poisonTrees.Remove(coll.transform);
            Destroy(coll.gameObject);
            Destroy(this);
        }

        else if (reproduction == true)
        {
            if (closest != null && coll.transform == closest && closest.CompareTag("Cow"))
            {
                reproduction = false;
                objectOnMap.cows.Add(Instantiate(objectOnMap.cow, transform.position, Quaternion.identity).GetComponent<Cow>());
                closest.GetComponent<Cow>().reproduction = false;
            }
        }
    }
}