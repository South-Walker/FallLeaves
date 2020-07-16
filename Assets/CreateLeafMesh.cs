using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLeafMesh : MonoBehaviour
{
    public float dz = 0.1f;
    [Range(0, 0.25f)]
    public float dt = 0.1f;
    const float xmin = 0;
    const float xmax = Mathf.PI * Mathf.PI / 1.5f / 6f;
    public GameObject leaf;
    public Shader shader;
    private void Awake()
    {
        List<Vector3> vertexs = new List<Vector3>();
        List<int> indice = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        float[] controlpointx = new float[4];
        float[] controlpointz = new float[4];
        controlpointx[0] = 0;
        controlpointz[0] = 0;
        for (int i = 1; i < 4; i++)
        {
            controlpointx[i] = (xmax - xmin) * i / 3.0f + xmin;
            controlpointz[i] = controlpointz[i - 1] + 3.0f / i * dz;
        }
        vertexs.Add(new Vector3(0, 0, 0));
        uv.Add(new Vector2(0, 0));
        for (float t = dt; t < 1; t += dt)
        {
            float x = controlpointx[0] * (1 - t) * (1 - t) * (1 - t) +
                3 * controlpointx[1] * t * (1 - t) * (1 - t) +
                3 * controlpointx[2] * t * t * (1 - t) +
                controlpointx[3] * t * t * t;
            float z = controlpointz[0] * (1 - t) * (1 - t) * (1 - t) +
                3 * controlpointz[1] * t * (1 - t) * (1 - t) +
                3 * controlpointz[2] * t * t * (1 - t) +
                controlpointz[3] * t * t * t;
            float y = Mathf.Sin(Mathf.Sqrt(x * 1.5f * 6)) / 10f;
            vertexs.Add(new Vector3(x, y, z));
            uv.Add(new Vector2(0, t));
            vertexs.Add(new Vector3(x, -y, z));
            uv.Add(new Vector2(0, t));
            if (t == dt)
            {
                indice.Add(0);
                indice.Add(vertexs.Count - 2);
                indice.Add(vertexs.Count - 1);
            }
            else
            {
                indice.Add(vertexs.Count - 4);
                indice.Add(vertexs.Count - 2);
                indice.Add(vertexs.Count - 3);
                indice.Add(vertexs.Count - 3);
                indice.Add(vertexs.Count - 2);
                indice.Add(vertexs.Count - 1);
            }
        }
        vertexs.Add(new Vector3(controlpointx[3], 0, controlpointz[3]));
        uv.Add(new Vector2(0, 1));
        indice.Add(vertexs.Count - 1);
        indice.Add(vertexs.Count - 2);
        indice.Add(vertexs.Count - 3);
        Mesh m = new Mesh();
        m.vertices = vertexs.ToArray();
        m.uv = uv.ToArray();
        m.triangles = indice.ToArray();
        leaf.AddComponent<MeshFilter>().mesh = m;
        leaf.AddComponent<MeshRenderer>().material = new Material(shader);
    }
}
