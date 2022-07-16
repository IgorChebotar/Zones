using UnityEngine;


namespace SimpleMan.Zones.Editor
{
    [System.Serializable]
	public class VisualData
    {
		//------FIELDS
		[SerializeField]
		private bool _drawShape = true;

		[SerializeField]
		private bool _wireShape = false;

		[SerializeField]
		private bool _drawLabel = true;

		[SerializeField]
		[Range(12, 72)]
		private int _fontSize = 14;

		[SerializeField]
		private Color _shapeColor = Color.green;

		[SerializeField]
		private Color _labelColor = Color.white;

        





        //------PROPERTIES
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
        public int FontSize 
		{ 
			get => _fontSize; 
			set => _fontSize = Mathf.Clamp(value, 12, 72); 
		}
        public Color ShapeColor 
		{ 
			get => _shapeColor; 
			set => _shapeColor = value;
		}
        public Color LabelColor 
		{ 
			get => _labelColor; 
			set => _labelColor = value; 
		}




		//------CONSTRUCTOR
		public VisualData()
		{
			
		}

		public VisualData(bool drawShape, bool wireShape, bool drawLabel, int fontSize, Color shapeColor, Color labelColor)
		{
			DrawShape = drawShape;
			WireShape = wireShape;
			DrawLabel = drawLabel;
			FontSize = fontSize;
			ShapeColor = shapeColor;
			LabelColor = labelColor;
		}
	}
}