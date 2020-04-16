using UnityEngine;

public class HpSpawnerSystem : MonoBehaviour 
{
    //################
    //##  INSPCTOR  ##
    //################

    public Transform PlayerPosition;
    public Transform[] SpawnPoints;

    public int MinAvailableHpPads;
    public int MaxAvailableHpPads;

    public GameObject HpSpawnerPrefab;

    //############
    //##  MONO  ##
    //############

    private void Start() 
    {
        foreach(Transform t in SpawnPoints)
        {
            //todo
            //t.gameObject.
        }   
    }



}