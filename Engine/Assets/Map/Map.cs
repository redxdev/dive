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
    /// Handles loading TMX-format map files.
    /// </summary>
    public class Map
    {
        private GidInfo[] gidCache = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        protected Map()
        {
            this.BackgroundColor = Dive.Engine.ColorConstants.CornflowerBlue;
            this.Tilesets = new List<Tileset>();
            this.Layers = new List<Layer>();
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Orientation of a map.
        /// </summary>
        public enum MapOrientation
        {
            /// <summary>
            /// Orthogonal orientation.
            /// </summary>
            Orthogonal,

            /// <summary>
            /// Isometric orientation.
            /// </summary>
            Isometric,

            /// <summary>
            /// Staggered orientation.
            /// </summary>
            Staggered
        }

        /// <summary>
        /// Gets or sets the map file format version. Usually 1.0.
        /// </summary>
        /// <value>The map file format version.</value>
        public string Version
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the orientation of this map.
        /// </summary>
        /// <value>The orientation of the map.</value>
        public MapOrientation Orientation
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the height of the map in tiles.
        /// </summary>
        /// <value>The height of the map in tiles.</value>
        public int Height
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the width of the map in tiles.
        /// </summary>
        /// <value>The width of the map in tiles.</value>
        public int Width
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the height of map tiles.
        /// </summary>
        /// <value>
        /// The height of map tiles.
        /// </value>
        public int TileHeight
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the width of map tiles.
        /// </summary>
        /// <value>
        /// The width of map tiles.
        /// </value>
        public int TileWidth
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public Color BackgroundColor
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the tilesets.
        /// </summary>
        /// <value>
        /// The tilesets.
        /// </value>
        public List<Tileset> Tilesets
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        public List<Layer> Layers
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
        /// Loads the the specified map.
        /// </summary>
        /// <param name="manager">The asset manager.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>The loaded map.</returns>
        /// <exception cref="Dive.Assets.Map.MapLoadException">
        /// Invalid map format
        /// or
        /// Isometric maps are currently not supported
        /// or
        /// Staggered maps are currently not supported.
        /// </exception>
        public static Map Load(AssetManager manager, string filename)
        {
            Map map = new Map();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.DocumentType:
                            if (reader.Name != "map")
                            {
                                throw new MapLoadException("Invalid map format");
                            }

                            break;

                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "map":
                                    map.Version = reader.GetAttribute("version");
                                    map.Width = int.Parse(reader.GetAttribute("width"));
                                    map.Height = int.Parse(reader.GetAttribute("height"));
                                    map.TileWidth = int.Parse(reader.GetAttribute("tilewidth"));
                                    map.TileHeight = int.Parse(reader.GetAttribute("tileheight"));

                                    string backgroundColor = reader.GetAttribute("backgroundcolor");
                                    if (backgroundColor != null)
                                    {
                                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(reader.GetAttribute("backgroundcolor"));
                                        map.BackgroundColor = new Color(color.R, color.G, color.B);
                                    }
                                    else
                                    {
                                        map.BackgroundColor = Dive.Engine.ColorConstants.CornflowerBlue;
                                    }

                                    switch (reader.GetAttribute("orientation"))
                                    {
                                        case "orthogonal":
                                            map.Orientation = MapOrientation.Orthogonal;
                                            break;

                                        case "isometric":
                                            throw new MapLoadException("Isometric maps are currently not supported");

                                        case "staggered":
                                            throw new MapLoadException("Staggered maps are currently not supported");
                                    }

                                    break;

                                case "tileset":
                                    using (var st = reader.ReadSubtree())
                                    {
                                        st.Read();
                                        Tileset tileset = Tileset.Load(manager, st);
                                        map.Tilesets.Add(tileset);
                                    }

                                    break;

                                case "layer":
                                    using (var st = reader.ReadSubtree())
                                    {
                                        st.Read();
                                        TileLayer layer = TileLayer.Load(manager, st);
                                        map.Layers.Add(layer);
                                    }

                                    break;

                                case "objectgroup":
                                    using (var st = reader.ReadSubtree())
                                    {
                                        st.Read();
                                        ObjectGroup objGroup = ObjectGroup.Load(manager, st);
                                        map.Layers.Add(objGroup);
                                    }

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
                                                            map.Properties.Add(st.GetAttribute("name"), st.GetAttribute("value"));
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
            }

            map.BuildGidInfoCache();

            return map;
        }

        /// <summary>
        /// Gets gid information.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The gid information.</returns>
        public GidInfo GetGidInfo(int index)
        {
            index -= 1;
            if (index < 0)
            {
                return null;
            }

            return this.gidCache[index];
        }

        /// <summary>
        /// Builds the gid information cache.
        /// </summary>
        protected void BuildGidInfoCache()
        {
            IntRect rect = new IntRect();
            List<GidInfo> cache = new List<GidInfo>();
            int i = 1;

        next:
            for (int t = 0; t < this.Tilesets.Count; t++)
            {
                Tileset tileset = this.Tilesets[t];
                if (tileset.MapTileToRect(i, ref rect))
                {
                    cache.Add(new GidInfo()
                    {
                        Texture = tileset.Texture,
                        TexturePath = tileset.TexturePath,
                        Rectangle = rect
                    });
                    i++;
                    goto next;
                }
            }

            this.gidCache = cache.ToArray();
        }

        /// <summary>
        /// Global id information class.
        /// </summary>
        public class GidInfo
        {
            /// <summary>
            /// Gets or sets the texture.
            /// </summary>
            /// <value>
            /// The texture.
            /// </value>
            public Texture Texture
            {
                get;
                set;
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
                set;
            }

            /// <summary>
            /// Gets or sets the rectangle.
            /// </summary>
            /// <value>
            /// The rectangle.
            /// </value>
            public IntRect Rectangle
            {
                get;
                set;
            }
        }
    }
}
