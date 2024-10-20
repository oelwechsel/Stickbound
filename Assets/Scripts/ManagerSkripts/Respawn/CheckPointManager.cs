using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints;

    private Checkpoint lastCheckpoint;

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        checkpoints.Add(checkpoint);
    }

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        if (lastCheckpoint != null)
        {
            return lastCheckpoint.transform.position;
        }
        else
        {
            // Falls es noch keinen erreichten Checkpoint gibt, gib eine Standard-Position zurück
           return Vector3.zero;
        }
    }

    public Checkpoint GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public bool IsCheckpointReached(Checkpoint checkpoint)
    {
        return checkpoint == lastCheckpoint;
    }
}
