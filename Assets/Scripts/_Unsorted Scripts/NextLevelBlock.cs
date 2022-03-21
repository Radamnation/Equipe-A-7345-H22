using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelBlock : MonoBehaviour
{
    [SerializeField] private bool isHub = false;
    [SerializeField] private BuildingBlock[] floorBlocks;
    [SerializeField] private BuildingBlock[] holeBlocks;

    [SerializeField] private GameObject control;

    // Start is called before the first frame update
    void Start()
    {
        control.SetActive(isHub);
        foreach (BuildingBlock floorBlock in floorBlocks)
        {
            floorBlock.gameObject.SetActive(!isHub);
        }
        foreach (BuildingBlock holeBlock in holeBlocks)
        {
            holeBlock.gameObject.SetActive(isHub);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton()
    {
        if (isHub)
        {
            InputSeed();
            return;
        }

        OpenFloor();
    }

    private void InputSeed()
    {

    }

    private void OpenFloor()
    {
        foreach (BuildingBlock floorBlock in floorBlocks)
        {
            floorBlock.gameObject.SetActive(false);
        }
        foreach (BuildingBlock holeBlock in holeBlocks)
        {
            holeBlock.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isHub)
            {
                GameManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, true);
            }
            else
            {
                GameManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, true);
            }
        }
    }

    public void ShowButton()
    {
        control.SetActive(true);
    }
}
