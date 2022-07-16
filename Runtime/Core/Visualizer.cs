using UnityEngine;
using Sirenix.OdinInspector;
using SimpleMan.Utilities;

namespace SimpleMan.Zones
{
	[ExecuteInEditMode]
    [AddComponentMenu(Editor.EditorConstants.MENU_PATH + "Visualizer")]
    public sealed class Visualizer : MonoBehaviour
    {
		//------FIELDS
		[BoxGroup("Shape")]
		[SerializeField]
		private bool _drawShape = true;

		[BoxGroup("Shape")]
		[EnableIf(nameof(_drawShape))]
		[SerializeField]
		private bool _wireShape = false;

		[BoxGroup("Shape")]
		[EnableIf(nameof(_drawShape))]
		[ColorUsage(false)]
		[SerializeField]
		private Color _shapeColor = Color.green.WithAlpha(0.4f);

		[BoxGroup("Shape")]
		[SerializeField, PropertyRange(0.1f, 0.7f)]
		private float _shapeAlpha = 0.4f;

		[FoldoutGroup("Label")]
		[SerializeField]
		private bool _drawLabel = true;

		[FoldoutGroup("Label")]
		[EnableIf(nameof(_drawLabel))]
		[SerializeField, TextArea(2, 10)]
		private string _textOverwrite;

		[FoldoutGroup("Label")]
		[EnableIf(nameof(_drawLabel))]
		[PropertyRange(0, 15)]
		[SerializeField]
		private int _maxCharactersInLine = 8;

		[FoldoutGroup("Label")]
		[SerializeField]
		[EnableIf(nameof(_drawLabel))]
		[Range(1, 64)]
		private int _labelSize = 8;

		[FoldoutGroup("Label")]
		[EnableIf(nameof(_drawLabel))]
		[SerializeField]
		private bool _constantLabelSize;

		[FoldoutGroup("Label")]
		[EnableIf(nameof(_drawLabel))]
		[SerializeField]
		private bool _labelSizeDependsOnScale = true;

		[FoldoutGroup("Label")]
		[SerializeField]
		[ColorUsage(false)]
		[EnableIf(nameof(_drawLabel))]
		private Color _labelColor = Color.white;

		[FoldoutGroup("Label")]
		[PropertyRange(0, 15)]
		[SerializeField]
		private float _labelOffset;
		private Transform _selfTransform;
		private Collider _colliderTarget;





		//------PROPERTIES
		public Transform SelfTransform
        {
			get
            {
				if(_selfTransform == null)
					_selfTransform = transform;

				return _selfTransform;
            }
        }
		public bool DrawShape
		{
			get => _drawShape;
			set => _drawShape = value;
		}
		public bool WireShape
		{
			get => _wireShape;
			set => _wireShape = value;
		}
		public bool DrawLabel
		{
			get => _drawLabel;
			set => _drawLabel = value;
		}
		public int LabelSize
		{
			get => _labelSize;
			set => _labelSize = Mathf.Clamp(value, 12, 72);
		}
		public float LabelOffset
        {
			get => _labelOffset;
			set => _labelOffset = value;
        }
		public bool LabelSizeDependsOnScale
        {
			get => _labelSizeDependsOnScale;
			set => _labelSizeDependsOnScale = value;
        }
		public Color ShapeColor
		{
			get => _shapeColor;
            set
            {
				_shapeColor = value.WithAlpha(
					Mathf.Clamp(value.a, 0.1f, 0.7f));
            }
		}
		public Color LabelColor
		{
			get => _labelColor;
			set => _labelColor = value.MaxAlpha();
		}
		public Collider ColliderTarget
        {
            get
            {
				if(_colliderTarget == null)
					_colliderTarget = GetComponent<Collider>();

				return _colliderTarget;
            }
        }




        //------EVENTS




        //------METHODS
        private void OnValidate()
        {
            _shapeColor = _shapeColor.WithAlpha(_shapeAlpha);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
			void DrawShape()
            {
				if (!_drawShape)
					return;

                if (!ColliderTarget)
                {
					return;
                }
				
				Mesh GetMesh(out Vector3 positionOffset, out Vector3 size)
                {
					Mesh mesh = null;
					positionOffset = Vector3.zero;
					size = Vector3.one;

					if (ColliderTarget is SphereCollider)
					{
						SphereCollider collider = ColliderTarget as SphereCollider;
						mesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");


						float radius = Mathf.Max(
									   SelfTransform.lossyScale.x,
									   SelfTransform.lossyScale.y,
									   SelfTransform.lossyScale.z) * collider.radius;

						size = Vector3.one * radius * 2;
						positionOffset = Vector3.Scale(collider.center, SelfTransform.lossyScale);
					}
					else if (ColliderTarget is BoxCollider)
					{
						BoxCollider collider = ColliderTarget as BoxCollider;
						mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");


						size = new Vector3(
									SelfTransform.lossyScale.x * collider.size.x,
									SelfTransform.lossyScale.y * collider.size.y,
									SelfTransform.lossyScale.z * collider.size.z);

						positionOffset = Vector3.Scale(collider.center, SelfTransform.lossyScale);
					}


					return mesh;
				}
				
				void DrawMesh()
                {
					Color defaultColor = Gizmos.color;
					Gizmos.color = _shapeColor;

					Mesh mesh = GetMesh(out Vector3 positionOffset, out Vector3 size);
					if (_wireShape)
					{
						Gizmos.DrawWireMesh(
							mesh,
							SelfTransform.position + positionOffset,
							SelfTransform.rotation, size);
					}
					else
					{
						Gizmos.DrawMesh(
							mesh,
							SelfTransform.position + positionOffset,
							SelfTransform.rotation, size);
					}

					Gizmos.color = defaultColor;
				}
				DrawMesh();
				
            }
			DrawShape();

			void DrawLabel()
            {
				if (!_drawLabel)
					return;

				int GetFontSize()
                {
					if (_constantLabelSize)
						return _labelSize;

					float GetDistanceToEditorCamera()
                    {
						if (UnityEditor.SceneView.currentDrawingSceneView == null)
                        {
							if (Camera.main == null)
                            {
								return 0;
                            }

                            else
                            {
								return Vector3.Distance(
								SelfTransform.position,
								Camera.main.transform.position);
							}
						}
                        else
                        {
							return Vector3.Distance(
						   SelfTransform.position,
						   UnityEditor.SceneView.currentDrawingSceneView.camera.transform.position);
						}	
					}
					
					float GetScaleFactor()
                    {
						if (_labelSizeDependsOnScale)
						{
							return Mathf.Min(
								SelfTransform.lossyScale.x,
								SelfTransform.lossyScale.y,
								SelfTransform.lossyScale.z);
						}
                        else
                        {
							return 1;
                        }
					}

					float ratio = _labelSize / GetDistanceToEditorCamera() * _labelSize;
					return (int)Mathf.Round(ratio * GetScaleFactor());
                }

				GUIStyle GetLabelStyle()
                {
					GUIStyle style = new GUIStyle();
					style.alignment = TextAnchor.MiddleCenter;
					style.normal.textColor = _labelColor;
					style.fontSize = GetFontSize();

					return style;
				}
				
				string GetText()
                {
					void SplitByLength(ref string value)
                    {
                        string[] words = value.Split(' ');
						value = string.Empty;


						int symbolsInLine = 0;
						for (int i = 0; i < words.Length; i++)
						{
							symbolsInLine += words[i].Length;
							value += words[i] + ' ';

							if (symbolsInLine >= _maxCharactersInLine)
							{
								value += '\n';
								symbolsInLine = 0;
							}
						}
                    }

					string labelText = 
						string.IsNullOrWhiteSpace(_textOverwrite) ?
						gameObject.GetNameWithoutPrefix().ToSplitPascalCase() : 
						_textOverwrite;

					SplitByLength(ref labelText);
					return labelText;
                }

                UnityEditor.Handles.Label(
                       SelfTransform.position + Vector3.up * _labelOffset,
                       GetText(),
                       GetLabelStyle());
            }
			DrawLabel();
#endif
        }
    }
}