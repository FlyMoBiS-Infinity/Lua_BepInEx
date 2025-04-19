using BepInEx.Logging;

using NLua;
using NLua.Exceptions;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


using File_Reader;
using Loader_Logger;
namespace Loader_Lua;


static public class LoadScripts {
	static public List<Lua> Luas {get; private set;} = new List<Lua>();

	static public void print (params object[] messages) {
		string result = "";
		foreach (object message in messages) {
			result = $"{result}{message.ToString()}\t";
		}
		Dairy.Log(Dairy.SLOG, LogLevel.Message, result);
	}

	/// <param name="args"> Arguments For Lua Function </param>
	/// <returns> Results Into object[] Of Lua Function. Null On Error </returns>
	static public object[] TryCallFunction (LuaFunction lfun, params object[] args) {
		try {
			object[] results = lfun.Call(args);
			//string result = null;
			//foreach (var rs in results) {
			//	result = $"{result}{rs.ToString()}\t";
			//}
			//if (result != null) {Dairy.Log(Dairy.SLOG, LogLevel.Info, $"Trying Call Script Function Result: {result}");};
			return results;
		}catch (LuaScriptException err) {
			Dairy.Log(Dairy.SLOG, LogLevel.Error, $"Script Runtime: \tIs .NET Exception: {err.IsNetException}\n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch (LuaException err) {
			Dairy.Log(Dairy.SLOG, LogLevel.Error, $"Script: \tHResult: {err.HResult}\n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch (System.Exception err) {
			Dairy.Log(Dairy.LLOG, LogLevel.Error, $"System: \n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch { Dairy.Log(Dairy.LLOG, LogLevel.Error, $"Unknown Error on Running/Loading Script: Trying Call Script Function"); }

		return null;
	}
	static public void TryCallFunctions (ref LuaFunction[] lfuns) {
		foreach (LuaFunction lfun in lfuns) {
			TryCallFunction(lfun);
		}
	}

	/// <summary>
	/// Try Load And Return A LuaFunction
	/// </summary>
	/// <param name="path"> Path To Lua File </param>
	/// <returns> LuaFunction. Null On Error </returns>
	static public LuaFunction TryLoadScript (string path) {
		string fileName = Path.GetFileName(path);
		LuaFunction lfun = null;
		string script = Reader.TryLua(path);
		if (script == null) {return lfun;}
		try {
			Lua lua = new Lua();
			Luas.Add(lua);
			lua.LoadCLRPackage();
			lua["print"] = print;
			lua["PATH"] = path;
			lfun = lua.LoadString(script, fileName);
		} catch (LuaScriptException err) {
			Dairy.Log(Dairy.SLOG, LogLevel.Error, $"Load Script Runtime: {path} - {fileName}\tIs .NET Exception: {err.IsNetException}\n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch (LuaException err) {
			Dairy.Log(Dairy.SLOG, LogLevel.Error, $"Load Script: {path} - {fileName}\tHResult: {err.HResult}\n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch (System.Exception err) {
			Dairy.Log(Dairy.LLOG, LogLevel.Error, $"On Load Script System: {path} - {fileName}\n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		} catch { Dairy.Log(Dairy.LLOG, LogLevel.Error, $"Unknown Error on Running/Loading Script: {path} - {fileName}"); }

		return lfun;
	}
	
	/// <summary>
	/// Load And Run LuaScripts In Directory. Paralleled
	/// </summary>
	/// <param name="DirPath"> Path To Directory </param>
	static async public void DoLuaFiles (string DirPath) {
		try {
			await Task.Run(() => {
				string[] PathsOfFiles = Directory.GetFiles(DirPath, "*.lua", SearchOption.TopDirectoryOnly);
				foreach (string filePath in PathsOfFiles) {
					LuaFunction lfun = TryLoadScript(filePath);
					if (lfun == null) {continue;};
					TryCallFunction(lfun);
				}
			});
		} catch {Dairy.Log(Dairy.LLOG, LogLevel.Fatal, "On Loading Scripts:");}
	}
	static public void DoLuaScripts (string LuaScriptsPath) {
		Dairy.Log(Dairy.LLOG, LogLevel.Debug, "Start Loading Lua scripts");
		try {
			string[] folders = Directory.GetDirectories(LuaScriptsPath);
			DoLuaFiles(LuaScriptsPath);
			foreach (string folder in folders) {
				DoLuaFiles(folder);
			}
		} catch (System.Exception err) {
			Dairy.Log(Dairy.LLOG, LogLevel.Fatal, $"System: \n\t\t\tMessage: {err.Message}\n\t\t\tStack Trace: {err.StackTrace}");
		}
	}
	
	/// <summary>
	/// Close Runing Lua
	/// </summary>
	/// <param name="path"> Path To Lua File </param>
	static public void CloseLua(string path) {
		for (int i = 0; i < Luas.Count; i += 1) {
			if ((string)Luas[i]["PATH"] == path) {
				Luas[i].Close();
				Luas.RemoveAt(i);
				break;
			}
		}
	}
	
	/// <summary>
	/// CloseLua, TryLoadScript, TryCallFunction
	/// </summary>
	/// <param name="path"> Path To Lua File </param>
	static public void ReloadLuaFile(string path) {
		CloseLua(path);
		LuaFunction lfun = TryLoadScript(path);
		TryCallFunction(lfun);
	}
	static public void ReloadLuaFiles(string DirPath) {
		string[] PathsOfFiles = Directory.GetFiles(DirPath, "*.lua", SearchOption.TopDirectoryOnly);
		foreach (string filePath in PathsOfFiles) {
			ReloadLuaFile(filePath);
		}
	}

}