using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public ObjectOnMap objectOnMap;             // Объекты на карте
    public SpriteRenderer spriteRenderer;       // Отрисовка дома
    public List<Human> residents;               // Список жителей

    public int residentsInHouse 
    {
        get
        {
            return _residentsInHouse;
        }
        set
        {
            _residentsInHouse = value;

            if (_residentsInHouse < 0)
                residentsInHouse = 0;
            else if (_residentsInHouse > 2)
                residentsInHouse = 2;
        }
    }
    public int _residentsInHouse;               // Количество членов семьи дома

    public int timeBeforeBirth = 5;             // Время на рождение
    private float curTimeBeforeBirth;

    public bool free;                           // Свободный ли дом

    public void RemoveResidents(Human human)    // Удалить жителя из дома
    {
        residents.Remove(human);

        curTimeBeforeBirth = 0;

        if (residents.Count == 0)
            free = true;
        else 
            free = false;
    }

    private void Update()
    {
        if (residentsInHouse == 2)
        {
            curTimeBeforeBirth += Time.deltaTime;

            if (curTimeBeforeBirth >= timeBeforeBirth)
            {
                objectOnMap.people.Add(Instantiate(objectOnMap.human, transform.position, Quaternion.identity).GetComponent<Human>());
                curTimeBeforeBirth = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)      // Уничтожение деревьев в доме
    {
        if (coll.transform.CompareTag("Tree"))
        {
            objectOnMap.trees.Remove(coll.transform);
            Destroy(coll.gameObject);
        }

        if (coll.transform.CompareTag("PoisonTree"))
        {
            objectOnMap.poisonTrees.Remove(coll.transform);
            Destroy(coll.gameObject);
        }

        if (coll.transform.CompareTag("Bush"))
        {
            objectOnMap.bushes.Remove(coll.transform);
            Destroy(coll.gameObject);
        }

        if (coll.transform.CompareTag("Flower"))
        {
            objectOnMap.flowers.Remove(coll.transform);
            Destroy(coll.gameObject);
        }

    }
}
