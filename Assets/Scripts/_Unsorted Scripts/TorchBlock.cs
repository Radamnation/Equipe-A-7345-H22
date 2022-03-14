using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBlock : MonoBehaviour
{
    [SerializeField] private ParticleSystem myFire;

    // Start is called before the first frame update
    void Start()
    {
        myFire.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LitTorch()
    {
        myFire.Play();
    }

    public void UnlitTorch()
    {
        myFire.Stop();
    }
}