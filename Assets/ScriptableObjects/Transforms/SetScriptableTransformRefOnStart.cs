using UnityEngine;

public class SetScriptableTransformRefOnStart : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private TransformSO myTransformRef;


    // SECTION - Method ===================================================================
    void Awake()
    {
        myTransformRef.Transform = GetComponent<Transform>();
    }
}
