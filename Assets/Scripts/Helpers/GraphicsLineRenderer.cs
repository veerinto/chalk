/* File: GraphicsLineRenderer.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;


namespace Interlude {
	[RequireComponent (typeof(MeshRenderer))]
	[RequireComponent (typeof(MeshFilter))]
	public class GraphicsLineRenderer : MonoBehaviour {

		public Material lineMaterial;

		public Mesh lineMesh;

		private Vector3 positionStart;

		private float lineSize = 0.1f;

		private bool isFirstQuad = true;

		void Start () {
			lineMesh = GetComponent<MeshFilter> ().mesh;
			GetComponent<MeshRenderer> ().material = lineMaterial;
		}

		public void AddPoint(Vector3 positionPoint) {
			if (positionStart != Vector3.zero) {
				AddLine (lineMesh, MakeQuad (positionStart, positionPoint, lineSize, isFirstQuad));
				isFirstQuad = false;
			}

			positionStart = positionPoint;
		}

		public void SetWidth(float width) {
			lineSize = width;
		}
		
		Vector3[] MakeQuad(Vector3 _positionStart, Vector3 _positionEnd, float width, bool all) {
			width = width / 2;

			Vector3[] vertices;
			if (all)
				vertices = new Vector3[4];
			else
				vertices = new Vector3[2];

			Vector3 normal = Vector3.Cross (_positionStart, _positionEnd);
			Vector3 side = Vector3.Cross (normal, _positionEnd - _positionStart);
			side.Normalize ();

			if (all) {
				vertices [0] = transform.InverseTransformPoint (_positionStart + side * width);
				vertices [1] = transform.InverseTransformPoint (_positionStart + side * -width);
				vertices [2] = transform.InverseTransformPoint (_positionEnd + side * width);
				vertices [3] = transform.InverseTransformPoint (_positionEnd + side * -width);
			} else {
				vertices [0] = transform.InverseTransformPoint (_positionEnd + side * width);
				vertices [1] = transform.InverseTransformPoint (_positionEnd + side * -width);
			}

			return vertices;
		}

		void AddLine(Mesh _mesh, Vector3[] _verticesQuad) {
			int lenVertices = _mesh.vertices.Length;

			Vector3[] verticesLine = _mesh.vertices;
			verticesLine = resizeVertices (verticesLine, 2 * _verticesQuad.Length);

			for (int i = 0; i < 2 * _verticesQuad.Length; i += 2) {
				verticesLine [lenVertices + i] = _verticesQuad [i / 2];
				verticesLine [lenVertices + i + 1] = _verticesQuad [i / 2];
			}

			Vector2[] uvs = _mesh.uv;
			uvs = resizeUVs (uvs, 2 * _verticesQuad.Length);

			if (_verticesQuad.Length == 4) {
				uvs [lenVertices + 0] = Vector2.zero;
				uvs [lenVertices + 1] = Vector2.zero;
				uvs [lenVertices + 2] = Vector2.right;
				uvs [lenVertices + 3] = Vector2.right;
				uvs [lenVertices + 4] = Vector2.up;
				uvs [lenVertices + 5] = Vector2.up;
				uvs [lenVertices + 6] = Vector2.one;
				uvs [lenVertices + 7] = Vector2.one;
			} else {
				if (lenVertices % 8 == 0) {
					uvs [lenVertices + 0] = Vector2.zero;
					uvs [lenVertices + 1] = Vector2.zero;
					uvs [lenVertices + 2] = Vector2.right;
					uvs [lenVertices + 3] = Vector2.right;
				} else {
					uvs [lenVertices + 0] = Vector2.up;
					uvs [lenVertices + 1] = Vector2.up;
					uvs [lenVertices + 2] = Vector2.one;
					uvs [lenVertices + 3] = Vector2.one;
				}
			}

			int lenTriangles = _mesh.triangles.Length;

			int[] triangles = _mesh.triangles;
			triangles = resizeTriangles (triangles, 12);

			if (_verticesQuad.Length == 2) {
				lenVertices -= 4;
			}

			triangles [lenTriangles + 0] = lenVertices;
			triangles [lenTriangles + 1] = lenVertices + 2;
			triangles [lenTriangles + 2] = lenVertices + 4;

			triangles [lenTriangles + 3] = lenVertices + 2;
			triangles [lenTriangles + 4] = lenVertices + 6;
			triangles [lenTriangles + 5] = lenVertices + 4;

			triangles [lenTriangles + 6] = lenVertices + 5;
			triangles [lenTriangles + 7] = lenVertices + 3;
			triangles [lenTriangles + 8] = lenVertices + 1;

			triangles [lenTriangles + 9] = lenVertices + 5;
			triangles [lenTriangles + 10] = lenVertices + 7;
			triangles [lenTriangles + 11] = lenVertices + 3;

			_mesh.vertices = verticesLine;
			_mesh.uv = uvs;
			_mesh.triangles = triangles;
			_mesh.RecalculateBounds ();
			_mesh.RecalculateNormals ();
		}

		Vector3[] resizeVertices(Vector3[] _verticesOld, int n) {
			Vector3[] _verticesNew = new Vector3[_verticesOld.Length + n];
			for(int i = 0; i < _verticesOld.Length; i++) {
				_verticesNew [i] = _verticesOld [i];
			}
			return _verticesNew;
		}

		Vector2[] resizeUVs(Vector2[] _uvsOld, int n) {
			Vector2[] _uvsNew = new Vector2[_uvsOld.Length + n];
			for (int i = 0; i < _uvsOld.Length; i++) {
				_uvsNew [i] = _uvsOld [i];
			}
			return _uvsNew;
		}

		int[] resizeTriangles(int[] _verticesOld, int n) {
			int[] _verticesNew = new int[_verticesOld.Length + n];
			for(int i = 0; i < _verticesOld.Length; i++) {
				_verticesNew [i] = _verticesOld [i];
			}
			return _verticesNew;
		}

	}
}
