using UnityEngine;
using System.Collections.Generic;

public class GameUpdateManager : MonoBehaviour
{
    private bool isUpdating;
    public bool IsUpdating
    {
        get
        {
            return isUpdating;
        }
    }

    private List<IUpdateable> dirtyUpdatables;
    private List<IUpdateable> currentUpdateables;
    private bool isDirty;

    public void Initialize()
    {
        dirtyUpdatables = new List<IUpdateable>();
        currentUpdateables = new List<IUpdateable>();
    }

    public void SetUpdateable(IUpdateable updateable)
    {
        if (dirtyUpdatables == null)
        {
            dirtyUpdatables = new List<IUpdateable>();
        }

        dirtyUpdatables.Add(updateable);
        isDirty = true;
    }

    public void Pause()
    {
        isUpdating = false;
    }

    public void Play()
    {
        isUpdating = true;
    }

    private void Update()
    {
        if (isUpdating)
        {
            for (int i = 0; i < currentUpdateables.Count; i++)
            {
                currentUpdateables[i].OnUpdate();
            }
        }

        if (isDirty)
        {
            for (int i = 0; i < dirtyUpdatables.Count; i++)
            {
                currentUpdateables.Add(dirtyUpdatables[i]);
            }

            dirtyUpdatables.Clear();
            isDirty = false;
        }
    }
}