using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/AI/Token Data Structure", fileName = "SO _ Token Data Structure")]
public class AITokenHandlerSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [SerializeField] private bool isDebugOn = false;

    // Base token quantity is 5
    private bool[] tokens =
        {
            true,
            true,
            true,
            true,
        };

    private short currentToken = 0;


    // SECTION - Field ===================================================================
    public int Length { get => tokens.Length; }


    // SECTION - Method ===================================================================
    public bool TryGetToken()
    {
        if (currentToken == Length || tokens[currentToken] == false)
            return false;

        tokens[currentToken] = false;

        if (currentToken < Length - 1)
            currentToken++;

        StaticDebugger.SimpleDebugger(isDebugOn, $"Token Acquired");

        return true;
    }

    public void ReturnToken(bool tokenDoubleCheck)
    {
        if (!tokenDoubleCheck)
            return;

        tokens[currentToken] = true;

        if (currentToken != 0)
            currentToken--;

        StaticDebugger.SimpleDebugger(isDebugOn, $"Token replaced | Current token at {currentToken}: {tokens[currentToken]}");
    }

    public void ResetTokens()
    {
        for (int index = 0; index < Length; index++)
            tokens[index] = true;

        currentToken = 0;

        StaticDebugger.SimpleDebugger(isDebugOn, $"Token Reseted");
    }

    public void SetSize(int size, bool setAllTrue = false)
    {
        bool[] temp = new bool[size];

        for (int index = 0; index < temp.Length; index++)
        {
            if (setAllTrue)                     // Set all true if specified as such
                temp[index] = true;
            else if (index < Length)            // Copy values until Length is reached
                temp[index] = tokens[index];
            else if (index > Length)            // Set any leftover as true
                temp[index] = true;
        }

        tokens = temp;
    }
}
