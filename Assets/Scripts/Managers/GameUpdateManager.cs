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

    private List<IUpdateable> dirtyAddUpdatables;
    private List<IUpdateable> dirtyRemoveUpdatable;
    private List<IUpdateable> currentUpdateables;
    private bool isDirtyAdd;
    private bool isDirtyRemove;

    public void Initialize()
    {
        dirtyAddUpdatables = new List<IUpdateable>();
        dirtyRemoveUpdatable = new List<IUpdateable>();
        currentUpdateables = new List<IUpdateable>();
    }

    public void SetUpdateable(IUpdateable updateable)
    {
        if (dirtyAddUpdatables == null)
        {
            dirtyAddUpdatables = new List<IUpdateable>();
        }

        dirtyAddUpdatables.Add(updateable);
        isDirtyAdd = true;
    }

    public void RemoveUpdateable(IUpdateable updateable)
    {
        if (dirtyRemoveUpdatable == null)
        {
            dirtyRemoveUpdatable = new List<IUpdateable>();
        }

        dirtyRemoveUpdatable.Add(updateable);
        isDirtyRemove = true;
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
                IUpdateable updateable = currentUpdateables[i];
                if (updateable != null)
                {
                    updateable.OnUpdate();
                }
            }
        }

        if (isDirtyAdd)
        {
            for (int i = 0; i < dirtyAddUpdatables.Count; i++)
            {
                currentUpdateables.Add(dirtyAddUpdatables[i]);
            }

            dirtyAddUpdatables.Clear();
            isDirtyAdd = false;
        }

        if (isDirtyRemove)
        {
            for (int i = 0; i < dirtyRemoveUpdatable.Count; i++)
            {
                currentUpdateables.Remove(dirtyRemoveUpdatable[i]);
            }

            dirtyRemoveUpdatable.Clear();
            isDirtyRemove = false;
        }
    }
}