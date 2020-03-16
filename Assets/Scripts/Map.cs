using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Отвечает за текущие объекты на сцене
    public ObjectOnMap objectOnMap;

    [Header("Cow")]
    public int countCow;

    [Header("Rabbit")]
    public int countRabbit;

    [Header("Giraffe")]
    public int countGiraffe;

    [Header("Predator")]
    public int countPredator;

    [Header("Bear")]
    public int countBear;

    [Header("Wolf")]
    public int countWolf;

    [Header("Tree")]
    public int countTree;

    [Header("Bush")]
    public int countBush;

    [Header("Flower")]
    public int countFlower;

    [Header("PoisonTree")]
    public int countPoisonTree;

    [Header("Human")]
    public int countHuman;

    [Header("Settings")]
    public Vector2 mapSize = new Vector2(100, 100);
    public float dinstanceTree = 20; // Дистанция между деревьями
    public float distanceFlower = 5;
    public int NumberOfAttempts = 10; // Количество попыток на установку дерева

    void Start()
    {
        generateMap();
        InvokeRepeating("respawnPlants", 60f, 60f); //Было 40 если чо
    }

    void generateMap()
    {
        // Очистить листы с объектами в начале и заполнить их данными в последствии
        objectOnMap.trees.Clear();
        objectOnMap.poisonTrees.Clear();
        objectOnMap.cows.Clear();
        objectOnMap.rabbits.Clear();
        objectOnMap.predators.Clear();
        objectOnMap.giraffes.Clear();
        objectOnMap.flowers.Clear();
        objectOnMap.wolves.Clear();
        objectOnMap.bears.Clear();
        objectOnMap.bushes.Clear();
        objectOnMap.people.Clear();
        objectOnMap.houses.Clear();
        objectOnMap.farms.Clear();

        respawnPlants();

        for (int i = 0; i < countCow; i++)
        {
            objectOnMap.cows.Add(Instantiate(objectOnMap.cow, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.cow.transform.rotation).GetComponent<Cow>());
        }
        for (int i = 0; i < countPredator; i++)
        {
            objectOnMap.predators.Add(Instantiate(objectOnMap.predator, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.predator.transform.rotation).GetComponent<Predator>());
        }

        for (int i = 0; i < countHuman; i++)
        {
            objectOnMap.people.Add(Instantiate(objectOnMap.human, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.human.transform.rotation).GetComponent<Human>());
        }

        for (int i = 0; i < countRabbit; i++)
        {
            objectOnMap.rabbits.Add(Instantiate(objectOnMap.rabbit, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.rabbit.transform.rotation).GetComponent<Rabbit>());
        }

        for (int i = 0; i < countGiraffe; i++)
        {
            objectOnMap.giraffes.Add(Instantiate(objectOnMap.giraffe, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.giraffe.transform.rotation).GetComponent<Giraffe>());
        }

        for (int i = 0; i < countBear; i++)
        {
            objectOnMap.bears.Add(Instantiate(objectOnMap.bear, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.bear.transform.rotation).GetComponent<Bear>());
        }

        for (int i = 0; i < countWolf; i++)
        {
            objectOnMap.wolves.Add(Instantiate(objectOnMap.wolf, new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)), objectOnMap.wolf.transform.rotation).GetComponent<Wolf>());
        }
    }


    public void respawnPlants()
    {
        //Спавн деревьев
        for (int i = 0; i < countTree; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)); // Создаю рандомную точку для персого дерева

            if (objectOnMap.trees.Count > 0)    // Проверяю количество деревьев
            {
                for (int j = 0; j < NumberOfAttempts; j++)    // Колиство попыток на установку дерева
                {
                    spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
                    bool done = true;

                    for (int k = 0; k < objectOnMap.trees.Count; k++)   // Перебираю все деревья и дистанции к ним
                    {
                        if (Vector3.Distance(spawnPos, objectOnMap.trees[k].transform.position) < dinstanceTree)
                        {
                            done = false;
                            break;
                        }
                    }

                    if (done)   // Если хорошо, устанавливаю и перехожу к след дереву
                    {
                        objectOnMap.trees.Add(Instantiate(objectOnMap.tree, spawnPos, objectOnMap.tree.transform.rotation).transform);
                        break;
                    }
                }
            }
            else
                objectOnMap.trees.Add(Instantiate(objectOnMap.tree, spawnPos, objectOnMap.tree.transform.rotation).transform);
        }

        //Спавн отравленных деревьев
        for (int i = 0; i < countPoisonTree; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)); // Создаю рандомную точку для персого дерева

            if (objectOnMap.poisonTrees.Count > 0)    // Проверяю количество деревьев
            {
                for (int j = 0; j < NumberOfAttempts; j++)    // Колиство попыток на установку дерева
                {
                    spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
                    bool done = true;

                    for (int k = 0; k < objectOnMap.poisonTrees.Count; k++)   // Перебираю все деревья и дистанции к ним
                    {
                        if (Vector3.Distance(spawnPos, objectOnMap.poisonTrees[k].transform.position) < dinstanceTree)
                        {
                            done = false;
                            break;
                        }
                    }

                    if (done)   // Если хорошо, устанавливаю и перехожу к след дереву
                    {
                        objectOnMap.poisonTrees.Add(Instantiate(objectOnMap.poisonTree, spawnPos, objectOnMap.poisonTree.transform.rotation).transform);
                        break;
                    }
                }
            }
            else
                objectOnMap.poisonTrees.Add(Instantiate(objectOnMap.poisonTree, spawnPos, objectOnMap.poisonTree.transform.rotation).transform);
        }

        //Спавн цветов
        for (int i = 0; i < countFlower; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)); // Создаю рандомную точку для персого дерева

            if (objectOnMap.flowers.Count > 0)    // Проверяю количество деревьев
            {
                for (int j = 0; j < NumberOfAttempts; j++)    // Колиство попыток на установку дерева
                {
                    spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
                    bool done = true;

                    for (int k = 0; k < objectOnMap.flowers.Count; k++)   // Перебираю все деревья и дистанции к ним
                    {
                        if (Vector3.Distance(spawnPos, objectOnMap.flowers[k].transform.position) < distanceFlower)
                        {
                            done = false;
                            break;
                        }
                    }

                    if (done)   // Если хорошо, устанавливаю и перехожу к след дереву
                    {
                        objectOnMap.flowers.Add(Instantiate(objectOnMap.flower, spawnPos, objectOnMap.flower.transform.rotation).transform);
                        break;
                    }
                }
            }
            else
                objectOnMap.flowers.Add(Instantiate(objectOnMap.flower, spawnPos, objectOnMap.flower.transform.rotation).transform);
        }

        //Спавн кустов
        for (int i = 0; i < countBush; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)); // Создаю рандомную точку для персого дерева

            if (objectOnMap.flowers.Count > 0)    // Проверяю количество деревьев
            {
                for (int j = 0; j < NumberOfAttempts; j++)    // Колиство попыток на установку дерева
                {
                    spawnPos = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
                    bool done = true;

                    for (int k = 0; k < objectOnMap.bushes.Count; k++)   // Перебираю все деревья и дистанции к ним
                    {
                        if (Vector3.Distance(spawnPos, objectOnMap.bushes[k].transform.position) < distanceFlower)
                        {
                            done = false;
                            break;
                        }
                    }

                    if (done)   // Если хорошо, устанавливаю и перехожу к след дереву
                    {
                        objectOnMap.bushes.Add(Instantiate(objectOnMap.bush, spawnPos, objectOnMap.bush.transform.rotation).transform);
                        break;
                    }
                }
            }
            else
                objectOnMap.bushes.Add(Instantiate(objectOnMap.bush, spawnPos, objectOnMap.bush.transform.rotation).transform);
        }

    }
}
