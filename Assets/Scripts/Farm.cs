using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public TextMesh text;
    public int countFood
    {
        get
        {
            return _countFood;
        }
        set
        {
            _countFood = value;
            if(countFood > 0)
            text.text = countFood.ToString();
            else text.text = string.Empty;
        }
    }
    public int _countFood;


    private float curProduction;        // Текущее состояние произвоства

    public ObjectOnMap objectOnMap;             // Объекты на карте
    public SpriteRenderer spriteRenderer;       // Отрисовка дома
    public Human owner
    {
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;

            if (_owner == null)
            {
                Invoke("DestroyFarm", 10);
                enabled = false;
            }
        }
    }
    public Human _owner;                // Владелец

    public void Production()
    {
        curProduction += Time.deltaTime;

        if (curProduction >= 1)
        {
            countFood++;
            curProduction = 0;
        }
    }

    private void DestroyFarm()
    {
        objectOnMap.farms.Remove(this);
        Destroy(gameObject);
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
