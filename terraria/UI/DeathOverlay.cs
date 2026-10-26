using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraria.UI;

public static class DeathOverlay
{
    // A small pixel font keeps the overlay independent of installed system fonts.
    private static readonly Dictionary<char, string[]> Glyphs = new()
    {
        ['Y'] = ["10001", "10001", "01010", "00100", "00100", "00100", "00100"],
        ['O'] = ["01110", "10001", "10001", "10001", "10001", "10001", "01110"],
        ['U'] = ["10001", "10001", "10001", "10001", "10001", "10001", "01110"],
        ['D'] = ["11110", "10001", "10001", "10001", "10001", "10001", "11110"],
        ['I'] = ["11111", "00100", "00100", "00100", "00100", "00100", "11111"],
        ['E'] = ["11111", "10000", "10000", "11110", "10000", "10000", "11111"],
        ['P'] = ["11110", "10001", "10001", "11110", "10000", "10000", "10000"],
        ['R'] = ["11110", "10001", "10001", "11110", "10100", "10010", "10001"],
        ['S'] = ["01111", "10000", "10000", "01110", "00001", "00001", "11110"],
        ['T'] = ["11111", "00100", "00100", "00100", "00100", "00100", "00100"],
        ['A'] = ["01110", "10001", "10001", "11111", "10001", "10001", "10001"]
    };

    private const string Title = "YOU DIED";
    private const string Prompt = "PRESS R TO RESTART";
    private static readonly Rectangle[] TitleGeometry = BuildTextGeometry(Title);
    private static readonly Rectangle[] PromptGeometry = BuildTextGeometry(Prompt);

    public static void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        System.ArgumentNullException.ThrowIfNull(spriteBatch);
        System.ArgumentNullException.ThrowIfNull(pixel);
        Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
        spriteBatch.Draw(pixel, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * 0.7f);

        int titleScale = System.Math.Max(1, System.Math.Min(8, viewport.Width / 60));
        int promptScale = System.Math.Max(1, System.Math.Min(3, viewport.Width / 165));
        DrawCenteredText(spriteBatch, pixel, TitleGeometry, Title.Length, viewport.Width,
            viewport.Height / 2 - 7 * titleScale, titleScale, new Color(255, 80, 80));
        DrawCenteredText(spriteBatch, pixel, PromptGeometry, Prompt.Length, viewport.Width,
            viewport.Height / 2 + 24, promptScale, Color.White);
    }

    private static Rectangle[] BuildTextGeometry(string text)
    {
        List<Rectangle> runs = new();
        for (int index = 0; index < text.Length; index++)
        {
            if (!Glyphs.TryGetValue(text[index], out string[] rows)) continue;
            for (int row = 0; row < rows.Length; row++)
            {
                int column = 0;
                while (column < rows[row].Length)
                {
                    if (rows[row][column] != '1') { column++; continue; }
                    int start = column;
                    while (column < rows[row].Length && rows[row][column] == '1') column++;
                    runs.Add(new Rectangle(index * 6 + start, row, column - start, 1));
                }
            }
        }
        return runs.ToArray();
    }

    private static void DrawCenteredText(SpriteBatch spriteBatch, Texture2D pixel,
        Rectangle[] geometry, int characterCount, int viewportWidth, int y, int scale, Color color)
    {
        int x = (viewportWidth - (characterCount * 6 - 1) * scale) / 2;
        foreach (Rectangle run in geometry)
        {
            spriteBatch.Draw(pixel, new Rectangle(x + run.X * scale, y + run.Y * scale,
                run.Width * scale, scale), color);
        }
    }
}
