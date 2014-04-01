namespace Dive.Assets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    /// <summary>
    /// Manages loading and unloading assets such as images and configuration files. Assets are identified by a 'key', which is usually
    /// the file path to the asset. File path keys should always use '/' as the path separator for consistency's sake.
    /// </summary>
    public class AssetManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssetManager));

        private Dictionary<Type, IAssetLoader> assetLoaders = new Dictionary<Type, IAssetLoader>();

        private Dictionary<Type, Dictionary<string, WeakReference>> assets = new Dictionary<Type, Dictionary<string, WeakReference>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetManager" /> class.
        /// </summary>
        /// <param name="autoAddAssetLoaders">If true, automatically register all asset loaders that have the <see cref="Dive.Util.Assets.Attributes.AssetLoader"/> attribute.</param>
        public AssetManager(bool autoAddAssetLoaders = true)
        {
            Log.Debug("Initialized AssetManager");

            if (autoAddAssetLoaders)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!typeof(IAssetLoader).IsAssignableFrom(type))
                        {
                            continue;
                        }

                        foreach (Attributes.AssetLoader attribute in type.GetCustomAttributes<Attributes.AssetLoader>(false))
                        {
                            this.RegisterAssetLoader(attribute.AssetType, (IAssetLoader)Activator.CreateInstance(type));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the dictionary of <see cref="IAssetLoader"/>s.
        /// </summary>
        /// <value>The dictionary of <see cref="IAssetLoader"/>s.</value>
        public Dictionary<Type, IAssetLoader> AssetLoaders
        {
            get
            {
                return this.assetLoaders;
            }
        }

        /// <summary>
        /// Gets the dictionary of assets.
        /// </summary>
        /// <value>The dictionary of assets.</value>
        public Dictionary<Type, Dictionary<string, WeakReference>> Assets
        {
            get
            {
                return this.assets;
            }
        }

        /// <summary>
        /// Register an asset loader with this asset manager.
        /// </summary>
        /// <typeparam name="T">The type for this loader to handle.</typeparam>
        /// <param name="loader">The <see cref="IAssetLoader"/> instance.</param>
        public void RegisterAssetLoader<T>(IAssetLoader loader)
        {
            this.RegisterAssetLoader(typeof(T), loader);
        }

        /// <summary>
        /// Register an asset loader with this asset manager.
        /// </summary>
        /// <param name="t">The type for this loader to handle.</param>
        /// <param name="loader">The <see cref="IAssetLoader"/> instance.</param>
        public void RegisterAssetLoader(Type t, IAssetLoader loader)
        {
            this.AssetLoaders.Add(t, loader);
            Log.Debug("Registered asset loader \"" + loader.GetType().FullName + "\" for type \"" + t.FullName + "\"");
        }

        /// <summary>
        /// Get if an asset is loaded into memory.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="key">The asset key.</param>
        /// <returns>True if the asset is loaded and in memory, false if not.</returns>
        public bool IsLoaded<T>(string key)
        {
            return this.IsLoaded(typeof(T), key);
        }

        /// <summary>
        /// Get if an asset is loaded into memory.
        /// </summary>
        /// <param name="t">The asset type.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>True if the asset is loaded and in memory, false if not.</returns>
        public bool IsLoaded(Type t, string key)
        {
            Dictionary<string, WeakReference> typedAssets = null;
            if (this.Assets.TryGetValue(t, out typedAssets))
            {
                WeakReference assetRef = null;
                if (typedAssets.TryGetValue(key, out assetRef) && assetRef.IsAlive)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets if an asset loader has been registered for a type.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>True if an asset loader has been registered for the specified type, false if not.</returns>
        public bool HasLoader<T>()
        {
            return this.HasLoader(typeof(T));
        }

        /// <summary>
        /// Gets if an asset loader has been registered for a type.
        /// </summary>
        /// <param name="t">The asset type.</param>
        /// <returns>True if an asset loader has been registered for the specified type, false if not.</returns>
        public bool HasLoader(Type t)
        {
            return this.AssetLoaders.ContainsKey(t);
        }

        /// <summary>
        /// Load an asset. If it has already been loaded and is still in memory, this method will simply
        /// return the existing asset.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="key">The asset key.</param>
        /// <returns>The asset.</returns>
        public T Load<T>(string key)
        {
            return (T)this.Load(typeof(T), key);
        }

        /// <summary>
        /// Load an asset. If it has already been loaded and is still in memory, this method will simply
        /// return the existing asset.
        /// </summary>
        /// <param name="t">The type of the asset.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>The asset.</returns>
        public object Load(Type t, string key)
        {
            Dictionary<string, WeakReference> typedAssets = null;
            if (this.Assets.TryGetValue(t, out typedAssets))
            {
                WeakReference assetRef = null;
                if (typedAssets.TryGetValue(key, out assetRef) && assetRef.IsAlive)
                {
                    Log.Debug("Already loaded asset \"" + key + "\" of type \"" + t.FullName + "\", returning reference");
                    return assetRef.Target;
                }
                else
                {
                    return this.Reload(t, key);
                }
            }
            else
            {
                this.Assets.Add(t, new Dictionary<string, WeakReference>());
                return this.Reload(t, key);
            }
        }

        /// <summary>
        /// Load an asset. This will not replace instances of the asset that are being used elsewhere.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="key">The asset key.</param>
        /// <returns>The asset.</returns>
        public T Reload<T>(string key)
        {
            return (T)this.Reload(typeof(T), key);
        }

        /// <summary>
        /// Load an asset. This will not replace instances of the asset that are being used elsewhere.
        /// </summary>
        /// <param name="t">The type of the asset.</param>
        /// <param name="key">The asset key.</param>
        /// <returns>The asset.</returns>
        public object Reload(Type t, string key)
        {
            Log.Debug("Loading asset \"" + key + "\" of type \"" + t.FullName + "\"");

            if (!this.HasLoader(t))
            {
                Log.Error("No asset loader registered");
                throw new AssetLoaderException("No asset loader registered for type \"" + t.FullName + "\"");
            }

            try
            {
                Dictionary<string, WeakReference> typedAssets = null;
                if (!this.Assets.TryGetValue(t, out typedAssets))
                {
                    typedAssets = new Dictionary<string, WeakReference>();
                    this.Assets.Add(t, typedAssets);
                }

                if (typedAssets.ContainsKey(key))
                {
                    typedAssets.Remove(key);
                }

                IAssetLoader loader = this.AssetLoaders[t];
                object asset = loader.Load(this, key);
                if (asset == null)
                {
                    throw new AssetLoadException("Loader returned null asset");
                }

                WeakReference assetRef = new WeakReference(asset);
                typedAssets.Add(key, assetRef);
                return asset;
            }
            catch (Exception e)
            {
                Log.Error("Unable to load asset \"" + key + "\" with type \"" + t.FullName + "\"");
                throw new AssetLoadException("Unable to load asset \"" + key + "\" with type \"" + t.FullName + "\"", e);
            }
        }

        /// <summary>
        /// Register an asset that has already been loaded.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="key">The asset key.</param>
        /// <param name="asset">The asset being registered.</param>
        /// <returns>The asset registered.</returns>
        public T RegisterAsset<T>(string key, T asset)
        {
            return (T)this.RegisterAsset(typeof(T), key, asset);
        }

        /// <summary>
        /// Register an asset that has already been loaded.
        /// </summary>
        /// <param name="t">The asset type.</param>
        /// <param name="key">The asset key.</param>
        /// <param name="asset">The asset being registered.</param>
        /// <returns>The asset registered.</returns>
        public object RegisterAsset(Type t, string key, object asset)
        {
            Log.Debug("Registering asset \"" + key + "\" of type \"" + t.FullName + "\"");

            try
            {
                Dictionary<string, WeakReference> typedAssets = null;
                if (!this.Assets.TryGetValue(t, out typedAssets))
                {
                    typedAssets = new Dictionary<string, WeakReference>();
                    this.Assets.Add(t, typedAssets);
                }

                WeakReference assetRef = new WeakReference(asset);
                typedAssets.Add(key, assetRef);
                return asset;
            }
            catch (Exception e)
            {
                Log.Error("Unable to register asset \"" + key + "\" with type \"" + t.FullName + "\"");
                throw new AssetLoadException("Unable to register asset \"" + key + "\" with type \"" + t.FullName + "\"", e);
            }
        }

        /// <summary>
        /// Clean the asset dictionaries of assets that have been removed from memory. This is a fairly expensive operation,
        /// and should rarely be called.
        /// </summary>
        public void Clean()
        {
            int cleanCount = 0;

            foreach (KeyValuePair<Type, Dictionary<string, WeakReference>> typedAssets in this.Assets)
            {
                List<string> clean = new List<string>();
                foreach (KeyValuePair<string, WeakReference> asset in typedAssets.Value)
                {
                    if (!asset.Value.IsAlive)
                    {
                        clean.Add(asset.Key);
                        cleanCount++;
                    }
                }

                foreach (string key in clean)
                {
                    Log.Debug("Removed " + key + " of type " + typedAssets.Key.FullName);
                    typedAssets.Value.Remove(key);
                }
            }

            if (cleanCount > 0)
            {
                Log.Debug("Cleaned " + cleanCount + " dead assets");
            }
        }

        /// <summary>
        /// Get the number of assets currently loaded. This includes assets that have been deleted but not
        /// cleaned up yet.
        /// </summary>
        /// <returns>The number of assets currently loaded.</returns>
        public int GetAssetCount()
        {
            int count = 0;
            foreach (Dictionary<string, WeakReference> typedAssets in this.Assets.Values)
            {
                count += typedAssets.Count;
            }

            return count;
        }
    }
}
