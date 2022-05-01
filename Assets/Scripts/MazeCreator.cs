using System;
using System.Collections.Generic;

public class MazeCreator
{
    public WallState[,] Create(int wallWidth, int wallHeight)
    {
        WallState[,] maze = new WallState[wallWidth, wallHeight];
        var initial = WallState.Right | WallState.Left | WallState.Up | WallState.Down;
        for (int width = 0; width < wallWidth; width++)
        {
            for (int height = 0; height < wallHeight; height++)
            {
                maze[width, height] = initial;
            }
        }

        return ApplyRecursiveBacktracker(maze, wallWidth, wallHeight);
    }


    private WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        var random = new Random();
        var positionStack = new Stack<Position>();
        var position = new Position
        {
            X = random.Next(0, width), Y = random.Next(0, height)
        };
        maze[position.X, position.Y] |= WallState.Visited;
        positionStack.Push(position);
        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);
            if (neighbours.Count > 0)
            {
                positionStack.Push(current);
                var randIndex = random.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall);
                maze[nPosition.X, nPosition.Y] |= WallState.Visited;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }

    private WallState GetOppositeWall(WallState wallState)
    {
        return wallState switch
        {
            WallState.Left => WallState.Right,
            WallState.Right => WallState.Left,
            WallState.Up => WallState.Down,
            WallState.Down => WallState.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(wallState), wallState, null)
        };
    }

    private List<Neighbour> GetUnvisitedNeighbours(Position position, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();
        if (position.X > 0 && !maze[position.X - 1, position.Y].HasFlag(WallState.Visited))
            AddLeftNeighbour(position, list);

        if (position.Y > 0 && !maze[position.X, position.Y - 1].HasFlag(WallState.Visited))
            AddBottomNeighbour(position, list);

        if (position.Y < height - 1 && !maze[position.X, position.Y + 1].HasFlag(WallState.Visited))
            AddTopNeighbour(position, list);

        if (position.X < width - 1 && !maze[position.X + 1, position.Y].HasFlag(WallState.Visited))
            AddRightNeighbour(position, list);

        return list;
    }

    private static void AddRightNeighbour(Position position, List<Neighbour> list)
    {
        list.Add(new Neighbour
        {
            Position = new Position()
            {
                X = position.X + 1,
                Y = position.Y
            },
            SharedWall = WallState.Right
        });
    }

    private static void AddTopNeighbour(Position position, List<Neighbour> list)
    {
        list.Add(new Neighbour
        {
            Position = new Position()
            {
                X = position.X,
                Y = position.Y + 1
            },
            SharedWall = WallState.Up
        });
    }

    private static void AddBottomNeighbour(Position position, List<Neighbour> list)
    {
        list.Add(new Neighbour
        {
            Position = new Position()
            {
                X = position.X,
                Y = position.Y - 1
            },
            SharedWall = WallState.Down
        });
    }

    private static void AddLeftNeighbour(Position position, List<Neighbour> list)
    {
        list.Add(new Neighbour
        {
            Position = new Position()
            {
                X = position.X - 1,
                Y = position.Y
            },
            SharedWall = WallState.Left
        });
    }
}