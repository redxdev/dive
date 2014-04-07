namespace Dive.Engine.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using Dive.Assets.Map;
    using Dive.Engine.Components;
    using Dive.Engine.Components.Graphics;
    using Dive.Engine.Components.Map;
    using Dive.Entity;
    using log4net;
    using SFML.Graphics;
    using SFML.Window;

    /// <summary>
    /// Extensions for loading maps.
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// The map draw layer start.
        /// </summary>
        public const int MapLayerStart = 10;

        private static readonly ILog Log = LogManager.GetLogger(typeof(MapExtensions));

        /// <summary>
        /// Imports the map into world space.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="map">The map to import.</param>
        /// <returns>The list of imported entities.</returns>
        /// <exception cref="Dive.Assets.Map.MapLoadException">Unknown layer type  + layer.GetType().FullName.</exception>
        public static Entity[] ImportMap(this GameEngine engine, Map map)
        {
            Log.Debug("Map.Version = " + map.Version);
            Log.Debug("Map.Orientation = " + map.Orientation);
            Log.Debug("Map.Height = " + map.Height);
            Log.Debug("Map.Width = " + map.Width);
            Log.Debug("Map.TileHeight = " + map.TileHeight);
            Log.Debug("Map.TileWidth = " + map.TileWidth);
            Log.Debug("Map.BackgroundColor = " + map.BackgroundColor);

            foreach (Tileset tileset in map.Tilesets)
            {
                Log.Debug("Tileset: " + tileset.Name);
            }

            List<Entity> entities = new List<Entity>();

            int layerId = MapLayerStart;
            foreach (Layer layer in map.Layers)
            {
                Log.Debug("Layer: " + layer.Name);

                if (layer is TileLayer)
                {
                    if (!layer.Visible)
                    {
                        continue;
                    }

                    TileLayer tileLayer = layer as TileLayer;

                    List<Sprite> sprites = new List<Sprite>();

                    foreach (Tile tile in tileLayer.Tiles)
                    {
                        Map.GidInfo info = map.GetGidInfo(tile.Id);

                        if (info == null)
                        {
                            continue;
                        }

                        Sprite sprite = new Sprite();
                        sprite.Position = new Vector2f(tile.X * map.TileWidth, tile.Y * map.TileHeight);
                        sprite.Texture = info.Texture;
                        sprite.TextureRect = info.Rectangle;
                        ////sprite.Origin = new Vector2f(map.TileWidth / 2f, map.TileHeight / 2f);
                        sprite.Color = new Color(255, 255, 255, (byte)Math.Floor(tileLayer.Opacity == 1.0 ? 255 : tileLayer.Opacity * 256.0));

                        float rotation = 0f;
                        Vector2f scale = new Vector2f(1f, 1f);
                        if (tile.HorizontalFlip)
                        {
                            scale.X = -1f;
                        }

                        if (tile.VerticalFlip)
                        {
                            scale.Y = -1f;
                        }

                        if (tile.DiagonalFlip)
                        {
                            if (tile.HorizontalFlip &&
                                tile.VerticalFlip)
                            {
                                rotation = 90f;
                                scale.Y = -1f;
                            }
                            else if (tile.HorizontalFlip)
                            {
                                rotation = -90f;
                                scale.Y = -1f;
                            }
                            else if (tile.VerticalFlip)
                            {
                                rotation = 90f;
                                scale.X = -1f;
                            }
                            else
                            {
                                rotation = -90f;
                                scale.X = -1f;
                            }
                        }

                        sprite.Scale = scale;
                        sprite.Rotation = rotation;

                        sprites.Add(sprite);
                    }

                    Entity ent = engine.EntityManager.CreateEntity();
                    ent.AddComponent<LayerComponent>(new LayerComponent()
                        {
                            Tiles = sprites,
                            DrawLayer = layerId
                        });

                    engine.EntityManager.BuildFromProperties(layer.Properties, ent);
                    ent.FinalizeEntity();

                    layerId++;

                    entities.Add(ent);
                }
                else if (layer is ObjectGroup)
                {
                    ObjectGroup group = layer as ObjectGroup;

                    Log.Debug("Layer: " + group.Name);

                    foreach (MapObject obj in group.Objects)
                    {
                        Entity ent = null;

                        if (obj.Type != null && obj.Type != string.Empty)
                        {
                            if (!string.IsNullOrWhiteSpace(obj.Name))
                            {
                                ent = engine.EntityManager.CreateEntityFromTemplateWithName(obj.Type, obj.Name);
                            }
                            else
                            {
                                ent = engine.EntityManager.CreateEntityFromTemplate(obj.Type);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(obj.Name))
                            {
                                ent = engine.EntityManager.CreateEntity(obj.Name);
                            }
                            else
                            {
                                ent = engine.EntityManager.CreateEntity();
                            }
                        }

                        TransformComponent transform = null;
                        if (ent.HasComponent<TransformComponent>())
                        {
                            transform = ent.GetComponent<TransformComponent>();
                        }
                        else
                        {
                            transform = ent.AddComponent<TransformComponent>(new TransformComponent());
                        }

                        transform.SetPosition(obj.X, obj.Y);
                        transform.Rotation = obj.Rotation;

                        Map.GidInfo info = map.GetGidInfo(obj.Id);

                        if (info != null && obj.Visible && group.Visible)
                        {
                            transform.AddY(-1 * info.Rectangle.Height);
                            SpriteComponent sprite = null;
                            if (ent.HasComponent<SpriteComponent>())
                            {
                                sprite = ent.GetComponent<SpriteComponent>();
                            }
                            else
                            {
                                sprite = ent.AddComponent<SpriteComponent>(new SpriteComponent());
                            }

                            sprite.Drawable.Texture = info.Texture;
                            sprite.Drawable.TextureRect = info.Rectangle;
                            sprite.Drawable.Color = new Color(255, 255, 255, (byte)Math.Floor(group.Opacity == 1.0 ? 255 : group.Opacity * 256.0));

                            float rotation = 0f;
                            Vector2f scale = new Vector2f(1f, 1f);
                            if (obj.HorizontalFlip)
                            {
                                scale.X = -1f;
                            }

                            if (obj.VerticalFlip)
                            {
                                scale.Y = -1f;
                            }

                            if (obj.DiagonalFlip)
                            {
                                if (obj.HorizontalFlip &&
                                    obj.VerticalFlip)
                                {
                                    rotation = 90f;
                                    scale.Y = -1f;
                                }
                                else if (obj.HorizontalFlip)
                                {
                                    rotation = -90f;
                                    scale.Y = -1f;
                                }
                                else if (obj.VerticalFlip)
                                {
                                    rotation = 90f;
                                    scale.X = -1f;
                                }
                                else
                                {
                                    rotation = -90f;
                                    scale.X = -1f;
                                }
                            }

                            sprite.Drawable.Scale = scale;
                            transform.Rotation += rotation;

                            sprite.DrawLayer = layerId;
                        }

                        if (!obj.Properties.ContainsKey("draw-layer"))
                        {
                            obj.Properties["draw-layer"] = layerId.ToString();
                        }

                        engine.EntityManager.BuildFromProperties(obj.Properties, ent);

                        ent.FinalizeEntity();

                        layerId++;

                        entities.Add(ent);
                    }
                }
                else
                {
                    throw new MapLoadException("Unknown layer type " + layer.GetType().FullName);
                }
            }

            engine.ClearColor = map.BackgroundColor;

            return entities.ToArray();
        }
    }
}
