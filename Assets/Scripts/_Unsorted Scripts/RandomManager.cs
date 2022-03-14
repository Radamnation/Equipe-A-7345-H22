using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    public static RandomManager instance;

    [SerializeField] private RandomSO roomGenerationRandom;
    [SerializeField] private RandomSO otherRandom;

    public RandomSO RoomGenerationRandom { get => roomGenerationRandom; set => roomGenerationRandom = value; }
    public RandomSO OtherRandom { get => otherRandom; set => otherRandom = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
