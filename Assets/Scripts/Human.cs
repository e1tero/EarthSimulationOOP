using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creatures
{
    public House house;                                 // Текущий дом

    public Farm farm
    {
        get
        {
            return _farm;
        }
        set
        {
            _farm = value;

            closest = _farm.transform;
        }
    }
    public Farm _farm;                                  // Текущая ферма

    private bool onAFarm;                               // Находится ли персонаж на ферме

    public Human family;                                // Семья / объект для размножения

    private int timeToFindPair;                         // Время на поиск пары
    public int radiusFindHouse;

    public int distanceHouse;                           // Дистанция на растояние между домами 
    public int radiusFindFood;                          // Радиус поиска еды

    private void Update()
    {
        if (hunger)
        {
            if (farm)
            {
                curTimeToDeath += Time.deltaTime;

                if (curTimeToDeath >= timeToDeath)
                {
                    DestroyThis();
                    Destroy(gameObject);
                }

                if (onAFarm)
                {
                    if (farm.countFood > 0)
                    {
                        hunger = false;
                        curTimeToDeath = 0;
                        farm.countFood--;
                    }
                }
            }
            else if (!farm && Hunger())   // Если голодный, то идти есть
                return;
        }

        if (farm)
        {
            if (onAFarm)
            {
                MoveToPoint();
                farm.Production();
            }
            else
            {
                MoveToPoint();
            }
        }
        else if (house)  // Если есть дом, то идти в него
        {
            closest = house.transform;
            MoveToPoint();
        }
        else if (!Reproduction())   // Если нет дома и фермы, то просто ходить
            Motion();
    }

    public override void DestroyThis()                  // Смерть 
    {
        objectOnMap.people.Remove(this);

        if (house != null)
        {
            house.RemoveResidents(this);
        }

        if (farm != null)
            farm.owner = null;
    }

    protected override void SearchOfFood()              // Поиск еды
    {
        // Ближайшая корова
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        var result = SearchClosestOfCow(objectOnMap.cows, closest, position, distance);

        // Ближайшая ферма
        result = SearchClosestOfFarm(objectOnMap.farms, result.c, position, result.d);

        // Ближайшей хищник
        result = SearchClosestOfPredator(objectOnMap.predators, result.c, position, result.d);

        // Ближайший волк
        result = SearchClosestOfWold(objectOnMap.wolves, result.c, position, result.d);

        // Ближайшее дерево
        result = SearchClosestOfPlant(objectOnMap.trees, result.c, position, result.d);

        // Ближайшее ядовитое дерево
        result = SearchClosestOfPlant(objectOnMap.poisonTrees, result.c, position, result.d);
        closest = result.c;

        if ((Vector3.Distance(transform.position, closest.position) > radiusFindFood) || closest.position == null)
        {
            closest = null;

            FindAPlaceToBuildFarm();
        }
    }

    protected override void PairSearch()                // Поиск пары
    {
        if (closest || family != null) return;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        closest = SearchClosestOfHuman(objectOnMap.people, closest, position, distance).c;
    }

    private void OnCollisionEnter2D(Collision2D coll)   // Голод и размножение
    {
        if (hunger == true && (coll.gameObject.CompareTag("Predator") || coll.gameObject.CompareTag("Cow") || coll.gameObject.CompareTag("Tree") || coll.gameObject.CompareTag("PoisonTree") || coll.gameObject.CompareTag("Wolf")))
        {
            if (coll.gameObject.CompareTag("Predator"))
                coll.transform.GetComponent<Predator>().DestroyThis();

            else if (coll.gameObject.CompareTag("Cow"))
                coll.transform.GetComponent<Cow>().DestroyThis();

            else if (coll.gameObject.CompareTag("Tree"))
                objectOnMap.trees.Remove(coll.transform);

            else if (coll.gameObject.CompareTag("Wolf"))
                coll.transform.GetComponent<Wolf>().DestroyThis();

            else if (coll.gameObject.CompareTag("PoisonTree"))
            {
                objectOnMap.poisonTrees.Remove(coll.transform);
                transform.GetComponent<Human>().DestroyThis();
                Destroy(coll.gameObject);
                Destroy(gameObject);
                Destroy(this);
            }

            closest = null;
            Destroy(coll.gameObject);
            hunger = false;
        }
        else if (hunger && coll.gameObject.CompareTag("Farm"))
        {
            Farm _farm = coll.transform.GetComponent<Farm>();

            if (_farm.countFood > 0)
            {
                _farm.countFood--;
                hunger = false;
            }
        }
        else if (!hunger && family == null && reproduction == true)
        {
            if (closest != null && coll.transform == closest)
            {
                reproduction = false;

                family = coll.gameObject.GetComponent<Human>();
                family.family = this;
                family.reproduction = false;

                if (house == null && family.house == null)
                    FindAPlaceToBuildHouse();
                else if (house == null && family.house != null)
                {
                    house = family.house;
                    house.residents.Add(this);
                }
                else if (house != null && family.house == null)
                {
                    family.house = house;
                    house.residents.Add(family);
                }
                else if (house != null && family.house != null)
                {
                    family.house.residents.Remove(family);
                    family.house = house;
                    house.residents.Add(family);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)      // Если вошел в дом или ферму
    {
        if (house != null && coll.transform == house.transform)
        {
            house.residentsInHouse++;
            house.spriteRenderer.enabled = true;
        }
        else if (farm != null && coll.transform == farm.transform)
        {
            onAFarm = true;
            farm.spriteRenderer.enabled = true;
        }
        else if (hunger && coll.transform == closest)
        {
            if (coll.transform.GetComponent<Farm>().countFood > 0)
            {
                hunger = false;
                curTimeToDeath = 0;
                coll.transform.GetComponent<Farm>().countFood--;
                closest = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)       // Если вышел из дома
    {
        if (house != null && coll.transform == house.transform)
            house.residentsInHouse--;
    }

    private void FindAPlaceToBuildHouse()               // Нахождение места для дома
    {
        if (objectOnMap.houses.Count > 0)   // Проверка, есть ли свободные дома
        {
            for (int i = 0; i < objectOnMap.houses.Count; i++)  // Проверить, есть ли свободные дома что бы туда заселится
            {
                if (objectOnMap.houses[i].free)
                {
                    house = objectOnMap.houses[i];
                    family.house = house;

                    house.residents.Add(this);
                    house.residents.Add(family);
                    objectOnMap.houses[i].free = false;
                    return;
                }
            }

            bool findHouse = false;

            for (int i = 0; i < objectOnMap.houses.Count; i++)  // Проверить, есть ли дома в радиусе
            {
                if (Vector3.Distance(objectOnMap.houses[i].transform.position, transform.position) <= radiusFindHouse) // Проверка наличия домов в радиусе
                {
                    findHouse = true;
                    break;
                }
            }

            if (!findHouse) // Если нет домов в радиусе, проверить, есть ли фермы в радиусе
            {
                for (int i = 0; i < objectOnMap.farms.Count; i++)
                {
                    if (Vector3.Distance(objectOnMap.farms[i].transform.position, transform.position) <= radiusFindHouse) // Проверка наличия домов в радиусе
                    {
                        findHouse = true;
                        break;
                    }
                }
            }

            if (!findHouse) // Если нечего нет, то начать строится
            {
                spawnHouse(transform.position); // Строительство на текущем месте
                return;
            }

            List<Transform> building = new List<Transform>();

            for (int i = 0; i < objectOnMap.houses.Count; i++)
            {
                building.Add(objectOnMap.houses[i].transform);
            }
            for (int i = 0; i < objectOnMap.farms.Count; i++)
            {
                building.Add(objectOnMap.farms[i].transform);
            }

            for (int i = 0; i < building.Count; i++) // Выбор места для строительтсва дома
            {
                for (int j = 0; j < 10; j++)    // Колиство попыток на установку дома
                {
                    int angle = Random.Range(0, 20);
                    Vector3 spawnPos = building[i].transform.position + new Vector3((distanceHouse + 1) * Mathf.Cos(angle), (distanceHouse + 1) * Mathf.Sin(angle), 0);

                    bool done = true;

                    for (int k = 0; k < building.Count; k++)   // Перебираю все дома и дистанции к ним
                    {
                        if (Vector3.Distance(spawnPos, building[k].transform.position) < distanceHouse)
                        {
                            done = false;
                            break;
                        }
                    }

                    if (done)   // Если хорошо, устанавливаю точку для дома
                    {
                        spawnHouse(spawnPos);
                        return;
                    }
                }
            }
        }
        else
        {
            spawnHouse(transform.position); // Строительство на текущем месте
        }
    }

    private void FindAPlaceToBuildFarm()
    {
        bool findFarm = false;

        for (int i = 0; i < objectOnMap.houses.Count; i++)
        {
            if (Vector3.Distance(objectOnMap.houses[i].transform.position, transform.position) <= radiusFindHouse) // Проверка наличия домов в радиусе
            {
                findFarm = true;
                break;
            }
        }

        for (int i = 0; i < objectOnMap.farms.Count; i++)
        {
            if (Vector3.Distance(objectOnMap.farms[i].transform.position, transform.position) <= radiusFindHouse) // Проверка наличия ферм в радиусе
            {
                findFarm = true;
                break;
            }
        }

        if (!findFarm)
        {
            spawnFarm(transform.position); // Строительство на текущем месте
            return;
        }

        List<Transform> building = new List<Transform>();

        for (int i = 0; i < objectOnMap.houses.Count; i++)
        {
            building.Add(objectOnMap.houses[i].transform);
        }
        for (int i = 0; i < objectOnMap.farms.Count; i++)
        {
            building.Add(objectOnMap.farms[i].transform);
        }

        for (int i = 0; i < building.Count; i++)
        {
            for (int j = 0; j < 10; j++)    // Колиство попыток на установку дома
            {
                int angle = Random.Range(0, 20);
                Vector3 spawnPos = building[i].transform.position + new Vector3((distanceHouse + 1) * Mathf.Cos(angle), (distanceHouse + 1) * Mathf.Sin(angle), 0);

                bool done = true;

                for (int k = 0; k < building.Count; k++)   // Перебираю все дома и дистанции к ним
                {
                    if (Vector3.Distance(spawnPos, building[k].transform.position) < distanceHouse)
                    {
                        done = false;
                        break;
                    }
                }

                if (done)   // Если хорошо, устанавливаю точку для дома
                {
                    spawnFarm(spawnPos);
                    return;
                }
            }
        }
    }

    private void spawnHouse(Vector3 position)           // спавн дома
    {
        objectOnMap.houses.Add(Instantiate(objectOnMap.house, position, Quaternion.identity).GetComponent<House>());

        house = objectOnMap.houses[objectOnMap.houses.Count - 1];

        family.house = house;

        house.residents.Add(this);
        house.residents.Add(family);

        house.free = false;
    }

    private void spawnFarm(Vector3 position)
    {
        farm = Instantiate(objectOnMap.farm, position, Quaternion.identity).GetComponent<Farm>();

        objectOnMap.farms.Add(farm);

        farm.owner = this;
    }
}
