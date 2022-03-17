using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable/Data Structure/Array Linear GameObjects", fileName = "SO_myArrayLinearGameObjects")]
public class ArrayLinearGameObjectSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    private int count = 0;
    [SerializeField] private GameObject[] myArray;


    // SECTION - Property ===================================================================
    public int Count { get => count; }
    public int Length => myArray.Length;

    public bool IsNull => myArray == null;
    public bool IsEmpty => count == 0;
    public bool IsFull => count == myArray.Length;



    // SECTION - Method - Unity Specific ===================================================================
    private void OnEnable()
    {
        CalculateCount();
    }


    // SECTION - Method - Data Structure Specific ===================================================================
    public void AddLength(int length = 1)
    {
        GameObject[] temp = new GameObject[myArray.Length + length];

        for (int i = 0; i < myArray.Length; i++)
            temp[i] = myArray[i];

        myArray = temp;
    }


    public void Copy(GameObject[] copyFrom)
    {
        if (copyFrom == null)
            return;

        count = 0;

        myArray = copyFrom;

        foreach (GameObject item in myArray)
            if (item != null)
                count++;
    }

    private void CalculateCount()
    {
        count = 0;

        if (myArray != null)
        {
            for (int i = 0; i < myArray.Length; i++)
                if (myArray[i] != null)
                    count++;
        }
    }

    public void Sort(ref int sortFrom)
    {
        if (myArray == null || sortFrom >= myArray.Length)
            return;

        if (myArray[sortFrom] != null)
        {
            for (int i = 0; i < count; i++)
            {
                if (myArray[i] == null)
                {
                    myArray[i] = myArray[sortFrom];
                    myArray[sortFrom] = null;
                }
            }
        }

        sortFrom++;
        Sort(ref sortFrom);
    }

    public GameObject GetElement(int index)
    {
        return myArray[index];
    }

    public void Add(GameObject item)
    {
        if (count < myArray.Length)
            for (int i = 0; i < myArray.Length; i++)
                if (myArray[i] == null)
                {
                    myArray[i] = item;
                    count++;
                    return;
                }
    }

    public void AddAt(GameObject item, int index)
    {
        if (index < myArray.Length)
            myArray[index] = item;
    }

    public void Remove()
    {
        if (myArray != null)
            myArray[count--] = null;
    }

    public void RemoveAt(int removeAt, bool alsoSort = false)
    {
        if (removeAt < count)
        {
            if (!alsoSort)
            {
                myArray[removeAt] = null;
                count--;
                return;
            }

            GameObject[] temp = new GameObject[myArray.Length];

            for (int i = 0; i < removeAt; i++)
                temp[i] = myArray[i];

            for (int i = removeAt + 1; i < myArray.Length; i++)
                temp[i] = myArray[i];

            myArray = temp;
            count--;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < myArray.Length; i++)
            if (myArray[i] != null)
                myArray[i] = null;
    }

    public void Debugger()
    {
        foreach (GameObject item in myArray)
        {
            Debug.Log($"myArray.item.name = {item}");
        }
    }
}
