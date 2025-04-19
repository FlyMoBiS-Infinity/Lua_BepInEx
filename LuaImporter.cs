using NLua;

using System;
using System.IO;
using System.Reflection;


using File_Reader;
using Loader_Logger;
namespace Lua_Importer;


static public class LuaImporter {
	static public void Import(Lua lua, string path) {
		Assembly dll = Assembly.Load(Reader.TryDLL(path));
		Module[] modules = dll.GetLoadedModules();
		foreach (Module module in modules) {
			Type[] types = module.GetTypes();
			foreach (Type type in types) {
				if (type.IsAbstract == true) {continue;}
				if (type.IsInterface == true) {continue;}
				if (type.IsClass == false) {continue;}
				lua[type.Name] = type;
			}
		}
	}
}