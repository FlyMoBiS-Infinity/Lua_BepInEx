using System.IO;
using System.Text;
using System.Collections.Generic;


namespace File_Reader;


static public class Reader {
	static public string TryLua(string path) {
		try {
			path = Path.GetFullPath(path);
			return File.ReadAllText(path);
		} catch {}
		return null;
	}
	static public byte[] TryDLL(string path) {
		try {
			path = Path.GetFullPath(path);
			return File.ReadAllBytes(path);
		} catch {}
		return null;
	}
}