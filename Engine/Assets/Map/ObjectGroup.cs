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
    public class ObjectGroup : Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectGroup"/> class.
        /// </summary>
        protected ObjectGroup()
        {
            this.Objects = new List<MapObject>();
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        /// <value>
        /// The objects.
        /// </value>
        public List<MapObject> Objects
        {
            get;
            protected set;
        }

        /// <summary>
        /// Loads an object group.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>The loaded object group.</returns>
        internal static ObjectGroup Load(AssetManager manager, XmlReader reader)
        {
            ObjectGroup objGroup = new ObjectGroup();

            objGroup.Name = reader.GetAttribute("name");

            string visible = reader.GetAttribute("visible");
            objGroup.Visible = visible == null ? true : (int.Parse(visible) > 0 ? true : false);

            string opacity = reader.GetAttribute("opacity");
            objGroup.Opacity = opacity == null ? 1f : float.Parse(opacity);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "object":
                                {
                                    using (var st = reader.ReadSubtree())
                                    {
                                        while (!st.EOF)
                                        {
                                            switch (st.NodeType)
                                            {
                                                case XmlNodeType.Element:
                                                    if (st.Name == "object")
                                                    {
                                                        string gidStr = reader.GetAttribute("gid");
                                                        uint gid = 0;
                                                        bool horizontalFlip = false;
                                                        bool verticalFlip = false;
                                                        bool diagonalFlip = false;
                                                        if (gidStr != null)
                                                        {
                                                            gid = uint.Parse(reader.GetAttribute("gid"));
                                                            horizontalFlip = (gid & Layer.HorizontalFlipFlag) != 0;
                                                            verticalFlip = (gid & Layer.VerticalFlipFlag) != 0;
                                                            diagonalFlip = (gid & Layer.DiagonalFlipFlag) != 0;
                                                            gid &= ~(Layer.HorizontalFlipFlag
                                                                | Layer.VerticalFlipFlag
                                                                | Layer.DiagonalFlipFlag);
                                                        }

                                                        string name = reader.GetAttribute("name");
                                                        name = name == null ? string.Empty : name;

                                                        string type = reader.GetAttribute("type");
                                                        type = type == null ? string.Empty : type;

                                                        string rotationString = reader.GetAttribute("rotation");
                                                        float rotation = 0f;
                                                        if (rotationString != null)
                                                        {
                                                            rotation = float.Parse(rotationString);
                                                        }

                                                        string visibleString = reader.GetAttribute("visible");
                                                        bool visibleObj = visibleString == null ? true : (int.Parse(visibleString) > 0 ? true : false);

                                                        MapObject obj = new MapObject(
                                                            (int)gid,
                                                            horizontalFlip,
                                                            verticalFlip,
                                                            diagonalFlip,
                                                            int.Parse(reader.GetAttribute("x")),
                                                            int.Parse(reader.GetAttribute("y")),
                                                            name,
                                                            type,
                                                            rotation,
                                                            visibleObj);

                                                        string width = reader.GetAttribute("width");
                                                        if (width != null)
                                                        {
                                                            obj.Properties.Add("width", width);
                                                        }

                                                        string height = reader.GetAttribute("height");
                                                        if (height != null)
                                                        {
                                                            obj.Properties.Add("height", height);
                                                        }

                                                        using (var props = reader.ReadSubtree())
                                                        {
                                                            props.Read();

                                                            while (!props.EOF)
                                                            {
                                                                switch (props.NodeType)
                                                                {
                                                                    case XmlNodeType.Element:
                                                                        switch (props.Name)
                                                                        {
                                                                            case "properties":
                                                                                {
                                                                                    using (var st2 = reader.ReadSubtree())
                                                                                    {
                                                                                        while (!st2.EOF)
                                                                                        {
                                                                                            switch (st2.NodeType)
                                                                                            {
                                                                                                case XmlNodeType.Element:
                                                                                                    if (st2.Name == "property")
                                                                                                    {
                                                                                                        obj.Properties.Add(st2.GetAttribute("name"), st2.GetAttribute("value"));
                                                                                                    }

                                                                                                    break;
                                                                                            }

                                                                                            st2.Read();
                                                                                        }
                                                                                    }

                                                                                    break;
                                                                                }

                                                                            case "polygon":
                                                                                {
                                                                                    obj.Properties.Add("polygon", props.GetAttribute("points"));
                                                                                    break;
                                                                                }

                                                                            case "polyline":
                                                                                {
                                                                                    obj.Properties.Add("polyline", props.GetAttribute("points"));
                                                                                    break;
                                                                                }

                                                                            case "ellipse":
                                                                                {
                                                                                    obj.X += float.Parse(width) / 2;
                                                                                    obj.Y += float.Parse(height) / 2;
                                                                                    break;
                                                                                }
                                                                        }

                                                                        break;
                                                                }

                                                                props.Read();
                                                            }
                                                        }

                                                        objGroup.Objects.Add(obj);
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
                                                        objGroup.Properties.Add(st.GetAttribute("name"), st.GetAttribute("value"));
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

            return objGroup;
        }
    }
}
