using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Random", fileName = "RandomSO")]
public class RandomSO : ScriptableObject
{
    [SerializeField] private bool randomizeOnStart = true;
    [SerializeField] private int seed;
    private System.Random systemRandom;

    public bool RandomizeOnStart { get => randomizeOnStart; set => randomizeOnStart = value; }
    public int Seed { get => seed; set => seed = value; }
    public System.Random SystemRandom { get => systemRandom; set => systemRandom = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (randomizeOnStart)
        {
            seed = Random.Range(0, 100000000);
        }
        systemRandom = new System.Random(seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
