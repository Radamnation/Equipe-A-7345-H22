using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Data Structure/Array Linear Generic", fileName = "SO_myArrayLinearGeneric")]
public class ArrayLinearGeneric<T> : ScriptableObject
{
    // SECTION - Field ===================================================================
    private int count = 0;
    [SerializeField] private T[] myArray;


    // SECTION - Property ===================================================================
    public int Count { get => count; }
    public int Length => myArray.Length;

    public bool IsNull => myArray == null;
    public bool IsEmpty => count == 0;
    public bool IsFull => count == myArray.Length;



    // SECTION - Constructor ===================================================================
    private void OnEnable()
    {
        CalculateCount();
    }

    public ArrayLinearGeneric()
    {
        CalculateCount();
    }

    // SECTION - Method - Data Structure Specific ===================================================================
    public void AddLength(int length = 1)
    {
        T[] temp = new T[myArray.Length + length];

        for (int i = 0; i < myArray.Length; i++)
            temp[i] = myArray[i];

        myArray = temp;
    }


    public void Copy(T[] copyFrom)
    {
        if (copyFrom == null)
            return;

        count = 0;

        myArray = copyFrom;

        foreach (T item in myArray)
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

    public void Sort_BackToBack(ref int sortFrom)
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
                    myArray[sortFrom] = default(T);//null;
                }
            }
        }

        sortFrom++;
        Sort_BackToBack(ref sortFrom);
    }

    public T GetElement(int index)
    {
        return myArray[index];
    }

    public void Add(T item)
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

    public void AddAt(T item, int index)
    {
        if (index < myArray.Length)
            myArray[index] = item;
    }

    public void Remove()
    {
        if (myArray != null)
            myArray[count--] = default(T);//null;
    }

    public void RemoveAt(int removeAt, bool alsoSort = false)
    {
        if (removeAt < count)
        {
            if (!alsoSort)
            {
                myArray[removeAt] = default(T);//null;
                count--;
                return;
            }

            // Recursive not used: Bellow is faster
            T[] temp = new T[myArray.Length];

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
                myArray[i] = default(T);//null;
    }

    public void Debugger()
    {
        foreach (T item in myArray)
        {
            Debug.Log($"myArray.item.name = {item}");
        }
    }
}