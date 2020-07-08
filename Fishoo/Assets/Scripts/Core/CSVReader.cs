using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// CSVの読み込みを行う
/// </summary>
public class CSVReader
{
    static public bool Exists(string directory, string filename) { 
        var path = Application.dataPath + "/" + directory+"/"+filename;
        return File.Exists(path);
    }
    static public bool CreateFile(string directory, string filename)
    {
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {
                return true;
            }
        }
        return true;
    }

    /// <summary>
    /// ファイルが存在していない場合、終了する
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static public List<List<string>> Read(string directory, string filename)
    {
        var path = Application.dataPath + "/" + directory+"/"+filename;
        if (!File.Exists(path)) return null;
        string[] line = File.ReadAllLines(path);
        var res = new List<List<string>>();
        foreach (var l in line)
        {
            var command = l.Replace(" ", "").Replace("　", "").Trim();
            var dat = command.Split(',');
            res.Add(new List<string>(dat));
        }
        return res;
    }

    /// <summary>
    /// ファイルが存在しない場合、ディレクトリ・ファイルを作成し、書き込む
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="filename"></param>
    /// <param name="data"></param>
    /// <param name="createFile"></param>
    static public void Write(string directory, string filename, List<List<string>> data, bool createFile = true)
    {
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {

            }
        }
        FileInfo fi = new FileInfo(path);
        using (StreamWriter sw = fi.CreateText())
        {
            foreach (var obj in data)
            {
                string buf = "";
                for (int i = 0; i < obj.Count - 1; i++) buf += obj[i] + ",";
                buf += obj[obj.Count - 1];
                sw.WriteLine(buf);
                sw.Flush();
            }
        }
    }

    static public void Clear(string directory, string filename)
    {
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) return;
        if (!File.Exists(path)) return;
        FileInfo fi = new FileInfo(path);
        using (StreamWriter sw = fi.CreateText())
        {
            sw.WriteLine("");
            sw.Flush();
        }
    }
}
