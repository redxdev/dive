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
    /// Handles tmx layers.
    /// </summary>
    public class TileLayer : Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileLayer"/> class.
        /// </summary>
        protected TileLayer()
        {
            this.Tiles = new List<Tile>();
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public uint Width
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public uint Height
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        public List<Tile> Tiles
        {
            get;
            protected set;
        }

        /// <summary>
        /// Loads a tile layer.
        /// </summary>
        /// <param name="manager">The asset manager.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>The loaded tile layer.</returns>
        /// <exception cref="Dive.Assets.Map.MapLoadException">Base64 and compression is not currently supported.</exception>
        internal static TileLayer Load(AssetManager manager, XmlReader reader)
        {
            TileLayer layer = new TileLayer();

            layer.Name = reader.GetAttribute("name");
            layer.Width = uint.Parse(reader.GetAttribute("width"));
            layer.Height = uint.Parse(reader.GetAttribute("height"));

            string visible = reader.GetAttribute("visible");
            layer.Visible = visible == null ? true : (int.Parse(visible) > 0 ? true : false);

            string opacity = reader.GetAttribute("opacity");
            layer.Opacity = opacity == null ? 1f : float.Parse(opacity);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "data":
                                {
                                    if (reader.GetAttribute("encoding") != null)
                                    {
                                        throw new MapLoadException("base64 and compression is not currently supported");
                                    }

                                    int x = 0;
                                    int y = 0;

                                    using (var st = reader.ReadSubtree())
                                    {
                                        while (!st.EOF)
                                        {
                                            switch (st.NodeType)
                                            {
                                                case XmlNodeType.Element:
                                                    if (st.Name == "tile")
                                                    {
                                                        uint gid = uint.Parse(reader.GetAttribute("gid"));
                                                        bool horizontalFlip = (gid & Layer.HorizontalFlipFlag) != 0;
                                                        bool verticalFlip = (gid & Layer.VerticalFlipFlag) != 0;
                                                        bool diagonalFlip = (gid & Layer.DiagonalFlipFlag) != 0;
                                                        gid &= ~(Layer.HorizontalFlipFlag
                                                            | Layer.VerticalFlipFlag
                                                            | Layer.DiagonalFlipFlag);
                                                        Tile tile = new Tile((int)gid, horizontalFlip, verticalFlip, diagonalFlip, x, y);
                                                        layer.Tiles.Add(tile);

                                                        if (x >= layer.Width - 1)
                                                        {
                                                            x = 0;
                                                            y++;
                                                        }
                                                        else
                                                        {
                                                            x++;
                                                        }
                                                    }

                                                    break;
                                            }

                                            st.Read();
                                        }
                                    }

                                    break;
                                }

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
                                                        layer.Properties.Add(st.GetAttribute("name"), st.GetAttribute("value"));
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

            return layer;
        }
    }
}
