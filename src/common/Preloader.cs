using System.Collections.Generic;
using Godot;

/// <summary>
/// Loads all the assets. Singleon.
/// </summary>
public sealed class Preloader
{
    private static Preloader instance = null;
    // Contains the mapping from resource path to the Godot resource
    private static Dictionary<string, Godot.Resource> sceneDict = new Dictionary<string, Godot.Resource>();

    private Preloader()
    {

    }

    public static Preloader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Preloader();
            }
            return instance;
        }
    }

    public T GetResource<T>(string path) where T : Godot.Resource
    {
        if (sceneDict.ContainsKey(path))
        {
            return (T)sceneDict[path];
        }
        T resource = GD.Load<T>(path);
        sceneDict.Add(path, resource);
        return resource;
    }
}