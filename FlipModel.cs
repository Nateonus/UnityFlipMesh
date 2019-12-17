/*
 * 
 * Created by Nathaniel Ford (Nateonus Apps)
 * https://twitter.com/nateonus
 * 16/12/2019
 * Licenced CC0.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NateonusApps
{
    public static class FlipModel
    {

        /// <summary>
        /// Multiply all a model's vertices & normals by mul, preserving all mesh data.
        /// </summary>
        /// <param name="mesh"></param>
        public static void MultiplyMeshCoordinates(Vector3 mul, Mesh mesh)
        {
            /* Retrieve a cloned version of 
             * the vertices and normals. */
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            /* Loop through every vertex, and multiply
             * the appropriate values by 'mul'. */
            for (int v = 0; v < vertices.Length; v++)
            {
                vertices[v] = mult(vertices[v], mul);
                normals[v] = mult(normals[v], mul);
            }

            /* Reassign the mesh vertices and normals.
             * Doing it this way means we change the mesh
             * directly, rather than creating a clone. */
            mesh.vertices = vertices;
            mesh.normals = normals;

            /* Changed vertices, so recalculate bounds.
             * Changed normals, so recalculate tangents. */
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }


        /// <summary>
        /// Reverses a model's tris. Use this if the model appears 'inside-out'.
        /// </summary>
        /// <param name="mesh"></param>
        public static void FlipTris(Mesh mesh)
        {
            /* Access the triangles to flip them */
            int[] tris;
            int t, temp;
            /* Need to iterate through each submesh
             * to preserve model submeshes. */
            for (int sub = 0; sub < mesh.subMeshCount; sub++)
            {
                tris = mesh.GetTriangles(sub);
                for (t = 0; t < tris.Length; t += 3)
                {
                    temp = tris[t + 1];
                    tris[t + 1] = tris[t + 2];
                    tris[t + 2] = temp;
                }
                mesh.SetTriangles(tris, sub);
            }
        }

        /// <summary>
        /// Flips a model's X coordinate, preserving all mesh data.
        /// </summary>
        /// <param name="mesh"></param>
        public static void FlipModelX(Mesh mesh)
        {
            /* Multiply all mesh X coordinates by -1 */
            MultiplyMeshCoordinates(new Vector3(-1, 1, 1), mesh);
            FlipTris(mesh);
        }

        /// <summary>
        /// Flips a model's Y coordinate, preserving all mesh data.
        /// </summary>
        /// <param name="mesh"></param>
        public static void FlipModelY(Mesh mesh)
        {
            MultiplyMeshCoordinates(new Vector3(1, -1, 1), mesh);
            FlipTris(mesh);
        }

        /// <summary>
        /// Flips a model's Z coordinate, preserving all mesh data.
        /// </summary>
        /// <param name="mesh"></param>
        public static void FlipModelZ(Mesh mesh)
        {
            MultiplyMeshCoordinates(new Vector3(1, 1, -1), mesh);
            FlipTris(mesh);
        }

        /// <summary>
        /// Returns (x1 * x2, y1 * y2, z1 * z2) of two vectors.
        /// </summary>
        private static Vector3 mult(Vector3 x, Vector3 y)
        {
            return new Vector3(x.x * y.x, x.y * y.y, x.z * y.z);
        }

    }
}