using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class NavPathRenderer :MonoSingleton<NavPathRenderer>
{
    public LineRenderer lineRenderer;
    NavMeshPath path;

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    public void SetPath(NavMeshPath path, Vector3 target)
    {
        //渲染路径
        this.path = path;
        if (this.path == null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = path.corners.Length + 1;
            lineRenderer.SetPositions(path.corners);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target);

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + Vector3.up * 0.2f);
            }
        }
    }
}

