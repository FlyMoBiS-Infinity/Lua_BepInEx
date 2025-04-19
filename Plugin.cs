using BepInEx;
using BepInEx.Logging;

using System;


using Loader_Logger;
namespace Loader_Lua;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]

public class Plugin : BaseUnityPlugin
{
    static public string LuaScriptsFolderPath = System.IO.Path.GetFullPath(BepInEx.Paths.PluginPath + "/LuaScripts");

    private void Awake ()
    {
		Dairy.Log(Dairy.MLOG, LogLevel.Debug, $"-----Valheim Lua Awake-----\n\t\tPlugin Path: {Paths.PluginPath}");

        if (System.IO.Directory.Exists(LuaScriptsFolderPath) == false) {
            try {
				System.IO.Directory.CreateDirectory(LuaScriptsFolderPath);
            } catch (System.UnauthorizedAccessException uae) {
                Dairy.Log(Dairy.MLOG, LogLevel.Error, $"Unauthorized Access - Creating Directory: {uae.Data} On Creating Directory In {LuaScriptsFolderPath}");
            } catch (System.Exception err) {
				Dairy.Log(Dairy.MLOG, LogLevel.Fatal, $"System: On Creating Directory In {LuaScriptsFolderPath}");
			}
		}

        LoadScripts.DoLuaScripts(LuaScriptsFolderPath);

		Dairy.Log(Dairy.MLOG, LogLevel.Info, "-----Valheim Lua Awake End-----");
    }

	private void Start () {
		Dairy.Log(Dairy.MLOG, LogLevel.Debug, $"-----Valheim Lua Start-----\n\t\tPlugin Path: {Paths.PluginPath}");
		Dairy.Log(Dairy.MLOG, LogLevel.Debug, $"");
		Dairy.Log(Dairy.MLOG, LogLevel.Debug, $"-----Valheim Lua End-----");
	}
}