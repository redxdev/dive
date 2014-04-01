namespace Dive.Assets.Map
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using SFML.Graphics;
    using SFML.Window;

    /// <summary>
    /// Handles tmx tilesets.
    /// </summary>
    public class Tileset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tileset"/> class.
        /// </summary>
        protected Tileset()
        {
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the name of the tileset.
        /// </summary>
        /// <value>
        /// The name of the tileset.
        /// </value>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the first gid.
        /// </summary>
        /// <value>
        /// The first gid.
        /// </value>
        public int FirstGID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the width of the tile.
        /// </summary>
        /// <value>
        /// The width of the tile.
        /// </value>
        public int TileWidth
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the height of the tile.
        /// </summary>
        /// <value>
        /// The height of the tile.
        /// </value>
        public int TileHeight
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, string> Properties
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture Texture
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the texture path.
        /// </summary>
        /// <value>
        /// The texture path.
        /// </value>
        public string TexturePath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>
        /// The spacing.
        /// </value>
        public int Spacing
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public int Margin
        {
            get;
            set;
        }

        /// <summary>
        /// Maps the tile to a rectangle.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="rect">The rectangle.</param>
        /// <returns>True if the tile has been successfully mapped, false if not.</returns>
        public bool MapTileToRect(int index, ref IntRect rect)
        {
            index -= this.FirstGID;

            if (index < 0)
            {
                return false;
            }

            int rowSize = (int)this.Texture.Size.X / (this.TileWidth + this.Spacing);
            int row = index / rowSize;
            int numRows = (int)this.Texture.Size.Y / (this.TileHeight + this.Spacing);

            if (row >= numRows)
            {
                return false;
            }

            int col = index % rowSize;

            rect.Left = (col * this.TileWidth) + (col * this.Spacing) + this.Margin;
            rect.Top = (row * this.TileHeight) + (row * this.Spacing) + this.Margin;
            rect.Width = this.TileWidth;
            rect.Height = this.TileHeight;
            return true;
        }

        /// <summary>
        /// Loads a tileset.
        /// </summary>
        /// <param name="manager">The asset manager.</param>
        /// <param name="mainReader">The main reader.</param>
        /// <returns>
        /// The tileset.
        /// </returns>
        /// <exception cref="Dive.Assets.Map.MapLoadException">Invalid tileset format for  + source.</exception>
        internal static Tileset Load(AssetManager manager, XmlReader mainReader)
        {
            Tileset tileset = new Tileset();

            XmlReader reader = mainReader;
            string source = reader.GetAttribute("source");
            if (source != null)
            {
                reader = XmlReader.Create(Path.Combine("content/maps/", source));
                bool readerOk = false;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "tileset")
                    {
                        readerOk = true;
                        break;
                    }
                }

                if (!readerOk)
                {
                    throw new MapLoadException("Invalid tileset format for " + source);
                }
            }

            tileset.Name = reader.GetAttribute("name");
            tileset.FirstGID = int.Parse(mainReader.GetAttribute("firstgid"));
            tileset.TileWidth = int.Parse(reader.GetAttribute("tilewidth"));
            tileset.TileHeight = int.Parse(reader.GetAttribute("tileheight"));

            string spacing = reader.GetAttribute("spacing");
            if (reader.GetAttribute("spacing") != null)
            {
                tileset.Spacing = int.Parse(spacing);
            }

            string margin = reader.GetAttribute("margin");
            if (margin != null)
            {
                tileset.Margin = int.Parse(margin);
            }

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "image":
                                string path = Path.Combine("content/maps/", reader.GetAttribute("source"));
                                Image image = manager.Load<Image>(path);
                                string trans = reader.GetAttribute("trans");
                                if (trans != null)
                                {
                                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#" + trans);
                                    image.CreateMaskFromColor(new Color(color.R, color.G, color.B));
                                }

                                tileset.Texture = new Texture(image);
                                tileset.TexturePath = path;
                                break;

                            case "properties":
                                {
                                    using (var st = reader.ReadSubtree())
                                    {
                                        while (!st.EOF)
                                        {
                                            switch (st.NodeType)
                                            {
                                                case XmlNodeType.Element:
                                                    if (st.Name == "property")
                                                    {
                                                        tileset.Properties.Add(st.GetAttribute("name"), st.GetAttribute("value"));
                                                    }

                                                    break;
                                            }

                                            st.Read();
                                        }
                                    }

                                    break;
                                }
                        }

                        break;
                }
            }

            return tileset;
        }
    }
}
