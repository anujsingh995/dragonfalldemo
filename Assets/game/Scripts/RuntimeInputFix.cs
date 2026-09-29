using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

/// <summary>Ensures the scene uses the new Input System UI module at runtime.</summary>
public static class RuntimeInputFix
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void FixEventSystem()
    {
        EventSystem eventSystem = Object.FindAnyObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject go = new GameObject("EventSystem");
            eventSystem = go.AddComponent<EventSystem>();
        }

        StandaloneInputModule legacy = eventSystem.GetComponent<StandaloneInputModule>();
        if (legacy != null)
            legacy.enabled = false;

        if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
    }
}
