using System;

using Godot;

using Redwood.Singletons;

namespace Redwood.World.Utils;

/// <summary>
/// Utils for chunk queueing and generation.
/// Reference - https://www.redblobgames.com/grids/circle-drawing
/// </summary>
public class ChunkCircleUtils
{
	/// <summary>
	/// Determines whether the given chunk coordinates falls within the cone of view at the angle the player is moving in.
	/// </summary>
	/// <param name="chunkCoordsToCheck">Requested chunk to validate</param>
	/// <param name="playerChunkCoords">Chunk the player is currently in</param>
	/// <returns><c>true</c> if the chunk's coordinates are within the cone of view, <c>false</c> otherwise.</returns>
	public static bool IsChunkInCone(Vector2I chunkCoordsToCheck, Vector2I playerChunkCoords)
	{
		int widthOfCone = 120; // Degrees

		var playerVelocity = PlayerManager.Instance.PlayerNode.Velocity.Normalized().Ceil();

		double searchAngle = playerVelocity.Angle() * (180 / Math.PI); // Convert from radians to degrees

		var angle = 180 / Math.PI * Math.Atan2(chunkCoordsToCheck.Y - playerChunkCoords.Y, chunkCoordsToCheck.X - playerChunkCoords.X);
		return DegreesApart(searchAngle, angle) <= widthOfCone / 2;
	}

	/// <summary>
	/// Determines whether the given chunk coordinates falls within the greater circle around the player.
	/// </summary>
	/// <param name="chunkCoordsToCheck">Requested chunk to validate</param>
	/// <param name="playerChunkCoords">Chunk teh player is currently in</param>
	/// <param name="radius">Radius of the circle</param>
	/// <returns><c>true</c> if the chunk's coordinates are within the circle, <c>false</c> otherwise.</returns>
	public static bool IsChunkInsideCircle(Vector2I chunkCoordsToCheck, Vector2I playerChunkCoords, int radius)
	{
		var distSqrd = playerChunkCoords.DistanceSquaredTo(chunkCoordsToCheck);
		var isInsideCircle = distSqrd <= radius * radius;

		return isInsideCircle;
	}

	private static double DegreesApart(double startDeg, double endDeg)
	{
		return Math.Min(DegreesLeft(startDeg, endDeg),
						DegreesRight(startDeg, endDeg));
	}

	private static double DegreesLeft(double startDeg, double endDeg)
	{
		return Mod(endDeg - startDeg, 360);
	}

	private static double DegreesRight(double startDeg, double endDeg)
	{
		return Mod(startDeg - endDeg, 360);
	}

	private static double Mod(double value, double modulo)
	{
		return ((value % modulo) + modulo) % modulo;
	}
}