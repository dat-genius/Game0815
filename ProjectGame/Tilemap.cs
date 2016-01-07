﻿using System;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    [XmlRoot("map")]
    public class Tilemap
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("orientation")]
        public string Orientation { get; set; }

        [XmlAttribute("renderorder")]
        public string RenderOrder { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlElement("tileset")]
        public Tileset[] Tilesets { get; set; }

        [XmlElement("layer")]
        public TileLayer[] TileLayers { get; set; }


        /// <summary>
        /// Processes the tilemap by calculating all the source and destination rectangles only 
        /// once instead of during every draw call.
        /// </summary>
        /// <param name="contentManager">The content manager to get the tilemap texture atlas from.</param>
        public void Build(ContentManager contentManager)
        {
            foreach (var tileset in Tilesets)
            {
                tileset.Texture = contentManager.Load<Texture2D>(tileset.Name);
            }

            foreach (var layer in TileLayers)
            {
                layer.DrawableTiles = new DrawableTile[Width * Height];

                var penX = 0;
                var penY = 0;
                for(var i = 0; i < layer.TileData.Length; ++i)
                {
                    var gid = layer.TileData[i];

                    Tileset tilesetToUse = null;
                    foreach (var tileset in Tilesets.TakeWhile(tileset => tileset.FirstGid <= gid).Where(tileset => tileset.FirstGid <= gid))
                    {
                        tilesetToUse = tileset;
                    }
                    if (tilesetToUse == null)
                        continue;

                    var destinationRectangle = new Rectangle(penX, penY, TileWidth, TileHeight);
                    penX += TileWidth;
                    if (penX >= Width * TileWidth)
                    {
                        penX = 0;
                        penY += TileHeight;
                    }

                    var tilesInTilesetHorizontal = tilesetToUse.Texture.Width/tilesetToUse.TileWidth;
                    var relativeGid = gid - tilesetToUse.FirstGid;
                    var sourceX = relativeGid;
                    var sourceY = 0;
                    while (sourceX >= tilesInTilesetHorizontal)
                    {
                        sourceX -= tilesInTilesetHorizontal;
                        sourceY += tilesetToUse.TileHeight + tilesetToUse.Spacing;
                    }
                    sourceX *= tilesetToUse.TileWidth + tilesetToUse.Spacing;
                    var sourceRectangle = new Rectangle(sourceX, sourceY, tilesetToUse.TileWidth, tilesetToUse.TileHeight);

                    layer.DrawableTiles[i] = new DrawableTile
                    {
                        DestinationRectangle = destinationRectangle,
                        SourceRectangle = sourceRectangle,
                        Texture = tilesetToUse.Texture
                    };
                }
            }
        }

        /// <summary>
        /// Draws all the layers in the tilemap.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw the tilemap with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var drawableTile in TileLayers.SelectMany(layer => layer.DrawableTiles))
            {
                if(++i > 20*320)
                    return;
                drawableTile.Draw(spriteBatch);
            }
        }
    }

    public class DrawableTile
    {
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Draws the tile.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw the tilemap with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle: DestinationRectangle, sourceRectangle: SourceRectangle);
        }
    }

    public class Tileset
    {
        [XmlAttribute("firstgid")]
        public int FirstGid { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlAttribute("spacing")]
        public int Spacing { get; set; }

        [XmlIgnore]
        public Texture2D Texture { get; set; }
    }

    public class TileLayer
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        private string csvString;
        [XmlElement("data")]
        public string CsvString
        {
            get { return csvString; }
            set
            {
                csvString = value;
                var valuesAsString = csvString.Replace("\n", "").Split(',');
                TileData = new int[valuesAsString.Length];
                for (var i = 0; i < valuesAsString.Length; ++i)
                {
                    TileData[i] = int.Parse(valuesAsString[i]);
                }
            }
        }

        [XmlIgnore]
        public int[] TileData { get; set; }

        [XmlIgnore]
        public DrawableTile[] DrawableTiles { get; set; }


        /// <summary>
        /// Draws the tile layer.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw the tilemap with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var drawableTile in DrawableTiles)
            {
                drawableTile.Draw(spriteBatch);
            }
        }
    }
}