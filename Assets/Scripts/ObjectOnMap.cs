using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Object on map")]
public class ObjectOnMap : ScriptableObject
{
    [Header("Cow")]
    public Cow cow;                     // Префаб коровы
    public List<Cow> cows;              // Список коров

    [Header("Rabbit")]
    public Rabbit rabbit;
    public List<Rabbit> rabbits;

    [Header("Giraffe")]
    public Giraffe giraffe;
    public List<Giraffe> giraffes;

    [Header("Flower")]
    public Transform flower;
    public List<Transform> flowers;

    [Header("Bush")]
    public Transform bush;
    public List<Transform> bushes;

    [Header("Tree")]
    public Transform tree;              // Префаб дерева
    public List<Transform> trees;       // Список деревьев

    [Header("PoisonTree")]
    public Transform poisonTree;        // Префаб ядовитого дерева        
    public List<Transform> poisonTrees; // Список ядовитых деревьев

    [Header("Bear")]
    public Bear bear;
    public List<Bear> bears;

    [Header("Wolf")]
    public Wolf wolf;
    public List<Wolf> wolves;

    [Header("Predator")]
    public Predator predator;           // Префаб охотника
    public List<Predator> predators;    // Список охотников

    [Header("Human")]
    public Human human;                 // Префаб человека
    public List<Human> people;          // Список людей

    [Header("House")]
    public House house;                 // Префаб дома
    public List<House> houses;          // Список домов

    [Header("Farm")]
    public Farm farm;                   // Префаб фермы
    public List<Farm> farms;            // Список ферм
}
