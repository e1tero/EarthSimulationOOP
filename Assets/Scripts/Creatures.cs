using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creatures : MonoBehaviour
{
    public ObjectOnMap objectOnMap;                   // Объекты на карте
    protected Transform closest;                      // Ближайший объект для интератива

    [Header("Движение")]
    public float moveSpeed;                           // Скорость передвижения
    private Rigidbody2D myRigidbody;                  // Физика
    public bool isWalking;
    public float walkTime;
    public float walkCounter;
    public float waitTime;
    public float waitCounter;
    public int WalkDirection;

    [Header("Размножение")]
    public float minTimeReproduction;           // мин время до спаривания
    public float maxTimeReproduction;           // макс время до спаривания
    private float reproductionTime;             // Время до спаривания
    public bool reproduction
    {
        get
        {
            return _reproduction;
        }
        set
        {
            _reproduction = value;

            if (!_reproduction)

            {
                Invoke("ReproductionActive", reproductionTime);
            }
        }
    }
    public bool _reproduction;

    [Header("Голод")]
    public float hungerTime;                    // Время до голода
    public float timeToDeath;                   // Время до смерти
    protected float curTimeToDeath;             // текущее время до смерти
    protected bool hunger
    {
        get
        {
            return _hunger;
        }
        set
        {
            _hunger = value;

            if (!_hunger)
            {
                Invoke("HungerActive", hungerTime);
                curTimeToDeath = 0;
            }
        }
    }
    protected bool _hunger;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        hunger = false;

        reproductionTime = Random.Range(minTimeReproduction, maxTimeReproduction);
        reproduction = false;

        ChooseDirection();
    }

    protected abstract void SearchOfFood();     // Поиск еды

    protected abstract void PairSearch();       // Поиск пары

    private void ReproductionActive()           // Включение желания размножаться
    {
        reproduction = true;
    }

    protected bool Reproduction()               // есть ли пара для размножения
    {
        if (reproduction)
        {
            if (!closest)
                PairSearch();

            if (closest)
            {
                MoveToPoint();
                return true;
            }
        }
        return false;
    }

    private void HungerActive()                 // Включение желания поесть
    {
        hunger = true;
    }
    protected bool Hunger()                     // Голод
    {
        if (hunger)
        {
            curTimeToDeath += Time.deltaTime;

            if (curTimeToDeath >= timeToDeath)
            {
                DestroyThis();
                Destroy(gameObject);
            }

            SearchOfFood();

            if (closest != null)
            {
                MoveToPoint();
                if (closest.CompareTag("Farm"))
                    if (closest.GetComponent<Farm>().countFood <= 0)
                        closest = null;
                return true;
            }
        }
        return false;
    }

    protected void MoveToPoint()                // Двигаться к точке
    {
        if (!closest) return;

        transform.position = Vector3.MoveTowards(transform.position, closest.position, Time.deltaTime * moveSpeed);
        myRigidbody.velocity = Vector2.zero;
    }

    public abstract void DestroyThis();         // Абстракция метода уничтожения

    protected void ChooseDirection()            // Выбор направления движения
    {
        WalkDirection = UnityEngine.Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    protected void Motion()                     // Передвижение
    {
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

            switch (WalkDirection)
            {
                case 0:
                    myRigidbody.velocity = new Vector2(0, moveSpeed);
                    break;
                case 1:
                    myRigidbody.velocity = new Vector2(moveSpeed, 0);
                    break;
                case 2:
                    myRigidbody.velocity = new Vector2(0, -moveSpeed);
                    break;
                case 3:
                    myRigidbody.velocity = new Vector2(-moveSpeed, 0);
                    break;
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

        }

        else
        {
            waitCounter = -Time.deltaTime;

            myRigidbody.velocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
            }
        }
    }

    // Поиск еды
    protected (Transform c, float d) SearchClosestOfPlant(List<Transform> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Transform item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfCow(List<Cow> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Cow item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfRabbit(List<Rabbit> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Rabbit item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfBear(List<Bear> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Bear item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfPredator(List<Predator> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Predator item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfHuman(List<Human> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Human item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfFarm(List<Farm> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Farm item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfWold(List<Wolf> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Wolf item in food)
        {
            if (item.transform == transform) continue;

            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    protected (Transform c, float d) SearchClosestOfGiraffe(List<Giraffe> food, Transform closest, Vector3 position, float distance)
    {
        foreach (Giraffe item in food)
        {
            if (item.transform == transform) continue;
            
            var result = SearchItem(item.transform, distance);
            closest = result.c;
            distance = result.d;
        }
        return (closest, distance);
    }
    private (Transform c, float d) SearchItem(Transform itemTransform, float distance)
    {
        Vector3 diff = itemTransform.position - transform.position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
            closest = itemTransform;
            distance = curDistance;
        }

        return (closest, distance);
    }

}
