using Godot;

public partial class AttackZone : Polygon2D
{
	[Export] public int Segments = 8;
	[Export] public float AttackAngle = 45.0f;
	[Export] public float AttackRange = 150.0f;

	public override void _Ready()
	{
		Modulate = new Color(1, 1, 1, 0.5f);
		UpdateConeShape();
	}

	public void UpdateConeShape()
	{
		float halfAngle = Mathf.DegToRad(AttackAngle / 2);
		var points = new Vector2[Segments + 2];

		points[0] = Vector2.Zero;

		for(int i = 0; i <= Segments; i++)
		{
			float angle = Mathf.Lerp(-halfAngle, halfAngle, (float)i / Segments);
			points[i+1] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * AttackRange;
		}
		Polygon = points;
	}
	public void RotateTo(Vector2 direction)
	{
		Rotation = direction.Angle();
	}
	public Vector2 GetForwardDirection()
	{
		return Vector2.Right.Rotated(Rotation);
	}
	public void ShowCooldown()
	{
		Modulate = new Color(0.9f,0.3f,0.3f, 0.4f);
	}
		public void ShowReady()
	{
		Modulate = new Color(0.3f,0.9f,0.5f, 0.4f);
	}
}
