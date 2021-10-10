using UnityEngine;

public static class MathExtension
{
	public static int FindLineCircleIntersections(Vector2 circleCenter, float radius, Vector2 vector, out Vector2 intersection1, out Vector2 intersection2)
	{
		return FindLineCircleIntersections(circleCenter, radius, new Vector2(0,0), vector, out intersection1, out intersection2);
	}

	public static int FindLineCircleIntersections(float circleX, float circleY, float radius,Vector2 point1, Vector2 point2,out Vector2 intersection1, out Vector2 intersection2)
	{
		return FindLineCircleIntersections(circleX, circleY, radius, point1, point2, out intersection1, out intersection2);
	}

	public static int FindLineCircleIntersections(float circleX, float circleY, float radius, Vector2 vector, out Vector2 intersection1, out Vector2 intersection2)
	{
		return FindLineCircleIntersections(circleX, circleY, radius, new Vector2(0, 0), vector, out intersection1, out intersection2);
	}

	public static int FindLineCircleIntersections(Vector2 circleCenter, float radius, Vector2 point1, Vector2 point2, out Vector2 intersection1, out Vector2 intersection2)
	{
		float dx, dy, A, B, C, det, t;

		dx = point2.x - point1.x;
		dy = point2.y - point1.y;

		A = dx * dx + dy * dy;
		B = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
		C = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) +
			(point1.y - circleCenter.y) * (point1.y - circleCenter.y) -
			radius * radius;

		det = B * B - 4 * A * C;
		if ((A <= 0.0000001) || (det < 0))
		{
			// No real solutions.
			intersection1 = new Vector2(float.NaN, float.NaN);
			intersection2 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		else if (det == 0)
		{
			// One solution.
			t = -B / (2 * A);
			intersection1 =
				new Vector2(point1.x + t * dx, point1.y + t * dy);
			intersection2 = new Vector2(float.NaN, float.NaN);
			return 1;
		}
		else
		{
			// Two solutions.
			t = (float)((-B + Mathf.Sqrt(det)) / (2 * A));
			intersection1 =
				new Vector2(point1.x + t * dx, point1.y + t * dy);
			t = (float)((-B - Mathf.Sqrt(det)) / (2 * A));
			intersection2 =
				new Vector2(point1.x + t * dx, point1.y + t * dy);
			return 2;
		}
	}
}
