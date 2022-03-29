using UnityEngine;

public class SetScriptableTransformRefOnStart : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private TransformSO myTransformRef;


    // SECTION - Method ===================================================================
    void Start()
    {
        myTransformRef.Transform = GetComponent<Transform>();
    }
}
