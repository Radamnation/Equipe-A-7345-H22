using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Selectable[] defaultItem;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        // PanelToggle(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PanelToggle(int select)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == select);
            if (i == select)
            {
                StartCoroutine(SelectButton(i));
            }
        }
    }

    public void QuitMenu()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    private IEnumerator SelectButton(int i)
    {
        yield return null;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        FindObjectOfType<EventSystem>().SetSelectedGameObject(defaultItem[i].gameObject);
    }

    public void QuitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
