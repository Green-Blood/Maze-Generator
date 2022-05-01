using UnityEngine;

public sealed class MazeDrawer : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float size = 1f;
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private Transform floorPrefab;

    private MazeCreator _mazeCreator;

    private void Awake()
    {
        _mazeCreator = new MazeCreator();
    }

    private void Start()
    {
        var maze = _mazeCreator.Create(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);
                if (cell.HasFlag(WallState.Up))
                {
                    var offset = new Vector3(0, 0, size / 2);
                    var rotation = transform.localRotation.eulerAngles;
                    InstantiateWall(position, offset, rotation);
                }

                if (cell.HasFlag(WallState.Left))
                {
                    var offset = new Vector3(-size / 2, 0, 0);
                    var rotation = new Vector3(0, 90, 0);
                    InstantiateWall(position, offset, rotation, size);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.Right))
                    {
                        var offset = new Vector3(+size / 2, 0, 0);
                        var rotation = new Vector3(0, 90, 0);
                        InstantiateWall(position, offset, rotation, size);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.Down))
                    {
                        var offset = new Vector3(0, 0, -size / 2);
                        var rotation = transform.localRotation.eulerAngles;
                        InstantiateWall(position, offset, rotation);
                    }
                }
            }
        }
    }

    private void InstantiateWall(Vector3 position, Vector3 offSet, Vector3 rotation, float scale = 1)
    {
        var wall = Instantiate(wallPrefab, transform);
        wall.position = position + offSet;
        wall.eulerAngles = rotation;
        wall.localScale = new Vector3(scale, wall.localScale.y, wall.localScale.z);
    }
}