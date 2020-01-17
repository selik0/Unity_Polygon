using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Polygon : Graphic
{
    /// <summary>
    /// 选择节点的生成方式
    /// </summary>
    [Tooltip("选择节点的生成方式"), Space(10)]
    public bool isUseAutoVertex = true;
    /// <summary>
    /// 节点数，最少3个.GetVertexType为AutoVertex使用该字段
    /// </summary>
    [Range(3, 360), Tooltip("选中isUseAutoVertex时该属性生效"), Space(10)]
    public int autoVerts = 3;
    /// <summary>
    /// GetVertexType为CustomizeVertex使用该字段
    /// </summary>
    [Tooltip("未选中isUseAutoVertex时该属性生效"), Space(10)]
    public RectTransform[] customizeVerts = new RectTransform[3];
    /// <summary>
    /// 节点向右旋转的角度
    /// </summary>
    [Range(0, 360), Space(10)]
    public float rotation = 0;
    /// <summary>
    /// 顶点到中心点的距离
    /// </summary>
    [Range(0, 1), Space(10)]
    public float[] vertex2pivot = new float[4];

    Vector3[] vertexsPosArr = new Vector3[3];


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        //清除所有顶点缓存
        vh.Clear();
        //初始化
        Init();
        int verts = GetVerts();
        for (int i = 0; i < verts + 1; i++)
        {
            vh.AddVert(vertexsPosArr[i], Color.white, Vector2.zero);
        }
        // 方法1: n边形画n个三角形
        //int y = 0;
        //for (int i = 0; i < verts; i++)
        //{
        //    y = i + 1;
        //    if (y >= verts)
        //    {
        //        y = 0;
        //    }
        //    vh.AddTriangle(i, y, verts);
        //    Debug.Log("---" + i + " " + y + " " + (verts));
        //}
        //方法2 画n-2个三角形
        for (int i = 0; i < verts - 2; i++)
        {
            vh.AddTriangle(0, i + 1, i + 2);
        }

    }

    void Init()
    {
        int verts = GetVerts();
        // 初始化顶点到中心到距离
        vertex2pivot = InitVertex2Piovt(verts + 1);
        // 初始化顶点的位置
        vertexsPosArr = InitVertexPosArr(verts + 1);
    }
    /// <summary>
    /// 获取n边形的n个顶点
    /// </summary>
    /// <returns></returns>
    int GetVerts()
    {
        int verts;
        if (isUseAutoVertex)
        {
            verts = autoVerts;
        }
        else
        {
            if (null == customizeVerts && customizeVerts.Length < 3)
            {
                return 3;
            }
            verts = customizeVerts.Length;
        }
        return verts;
    }

    /// <summary>
    /// 初始化顶点到中心到距离
    /// </summary>
    /// <param name="verts">顶点数 n + 1 (1为中心点)</param>
    /// <returns></returns>
    float[] InitVertex2Piovt(int verts)
    {
        if (vertex2pivot.Length != verts)
        {
            vertex2pivot = new float[verts];
            for (int i = 0; i < verts; i++)
            {
                vertex2pivot[i] = 1f;
            }
        }
        return vertex2pivot;
    }
    /// <summary>
    /// 初始化顶点到的位置，在InitVertex2Piovt后调用
    /// </summary>
    /// <param name="verts">顶点数 n + 1 (1为中心点)</param>
    /// <returns></returns>
    Vector3[] InitVertexPosArr(int verts)
    {
        vertexsPosArr = new Vector3[verts];
        if (isUseAutoVertex)
        {
            for (int i = 0; i < verts; i++)
            {
                float outer = -rectTransform.pivot.x * rectTransform.rect.width * vertex2pivot[i];
                float angle = ((float)360 / (verts - 1) * i) + rotation;
                float x = outer * getCos(angle);
                float y = outer * getSin(angle);
                vertexsPosArr[i] = new Vector3(x, y, 0);
                Debug.Log("角度：" + angle + "sin:" + getSin(angle) + "cos:" + getCos(angle));
            }
            vertexsPosArr[verts - 1] = Vector3.zero;
            for (int i = 0; i < verts; i++)
            {
                Debug.Log("vertexsPosArr[" + i +"]:" + vertexsPosArr[i].ToString());
            }
        }
        else
        {
            for (int i = 0; i < verts; i++)
            {
                if (customizeVerts[i] == null)
                {
                    Debug.LogError("customizeVerts[" + i + "] is null");
                    break;
                }
                vertexsPosArr[i] = customizeVerts[i].localPosition * vertex2pivot[i];
            }
            vertexsPosArr[verts - 1] = Vector3.zero;
        }
        return vertexsPosArr;
    }

    public float getSin(float angle)
    {
        return Mathf.Sin(angle * Mathf.PI / 180);
    }

    public float getCos(float angle)
    {
        return Mathf.Cos(angle * Mathf.PI / 180);
    }
}
