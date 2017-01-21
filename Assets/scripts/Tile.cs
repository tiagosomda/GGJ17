public class Tile
{
    public bool left;
    public bool right;
    public bool top;
    public bool bottom;

    public static string CleanName(string name)
    {
        name = name.Replace(" ", string.Empty);

        var index = name.IndexOf('(');

        if (index > 0)
        {
            name = name.Substring(0, index);
        }

        return name;
    }
    public static Tile FromName(string name)
    {
        name = CleanName(name);

        var tile = new Tile();

        var walls = name.Split('_');

        foreach (var wall in walls)
        {
            if (wall == "top")
            {
                tile.top = true;
            }

            if (wall == "bottom")
            {
                tile.bottom = true;
            }

            if (wall == "left")
            {
                tile.left = true;
            }

            if (wall == "right")
            {
                tile.right = true;
            }
        }

        return tile;
    }
}
