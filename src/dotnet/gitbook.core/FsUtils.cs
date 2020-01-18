using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace gitbook.core
{
    public static class FsUtils
    {
        public static bool CreateDir(this FsPath path)
        {
            if (!FsPath.IsEmptyPath(path))
            {
                Directory.CreateDirectory(path.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CopyDirectory(this FsPath sourceDirectory, FsPath TargetDir)
        {
            if (!Directory.Exists(TargetDir.ToString()))
            {
                Directory.CreateDirectory(TargetDir.ToString());
            }

            foreach (string newPath in Directory.GetFiles(sourceDirectory.ToString(), "*.*",
                SearchOption.AllDirectories))
            {
                var targetfile = newPath.Replace(sourceDirectory.ToString(), TargetDir.ToString());
                File.Copy(newPath, targetfile, true);
            }
            return true;
        }

        public static bool Copy(this FsPath source, FsPath target, bool overwrite)
        {
            if (!source.IsExisting)
            {
                return false;
            }
            var dir = Path.GetDirectoryName(target.ToString());
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.Copy(source.ToString(), target.ToString(), overwrite);
            return true;
        }

        public static bool CreateBackup(this FsPath source)
        {
            if (!source.IsExisting)
            {
                return false;
            }
            string targetname = $"{source}_backup";
            if (File.Exists(targetname))
            {
                bool exists = true;
                int counter = 1;
                do
                {
                    targetname = $"{source}_backup{counter}";
                    ++counter;
                    exists = File.Exists(targetname);
                }
                while (exists);
            }
            File.Copy(source.ToString(), targetname);
            return true;
        }

        public static bool WriteFile(this FsPath target, params string[] contents)
        {
            FileInfo fileInfo = new FileInfo(target.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            using (var writer = File.CreateText(target.ToString()))
            {
                foreach (var content in contents)
                {
                    writer.Write(content);
                }
            }

            return true;
        }

        public static string ReadFile(this FsPath path)
        {
            using (var reader = File.OpenText(path.ToString()))
            {
                return reader.ReadToEnd();
            }
        }

        public static void ProtectDirectory(this FsPath directory)
        {
            var outp = directory.Combine("index.html");
            StringBuilder sb = new StringBuilder(4096);
            for (int i=0; i<256; i++)
            {
                sb.Append("                ");
            }
            outp.WriteFile(sb.ToString());
        }

        public static IEnumerable<FsPath> GetAllFiles(this FsPath directory, string mask = "*.*")
        {
            // 当前目录的文件;
            foreach (var file in Directory.GetFiles(directory.ToString(), mask, SearchOption.TopDirectoryOnly))
            {
                if (!GitignoreParser.Default.IsMatch(file))
                {
                    yield return new FsPath(file);
                }
            }
            // 子目录的文件;
            foreach (var dir in Directory.GetDirectories(directory.ToString())) 
            {
                if (!GitignoreParser.Default.IsMatch(dir))
                {
                    foreach (var file in Directory.GetFiles(dir, mask, SearchOption.AllDirectories))
                    {
                        if (!GitignoreParser.Default.IsMatch(file))
                        {
                            yield return new FsPath(file);
                        }
                    }
                }
            }
        }

        public static bool SerializeXml<T>(this FsPath path, T obj,IList<(string prefix, string namespac)>? nslist = null) where T : class, new()
        {
            FileInfo fileInfo = new FileInfo(path.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            XmlSerializerNamespaces? xnames = null;
            if (nslist != null)
            {
                xnames = new XmlSerializerNamespaces();
                foreach (var ns in nslist)
                {
                    xnames.Add(ns.prefix, ns.namespac);
                }
            }

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (var writer = File.Create(path.ToString()))
            {
                if (xnames == null)
                    xs.Serialize(writer, obj);
                else
                    xs.Serialize(writer, obj, xnames);
            }

            return true;
        }

        public static bool SerializeJson<T>(this FsPath path, T obj,bool indent = true) where T : class, new()
        {
            FileInfo fileInfo = new FileInfo(path.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            string serialized = JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions
            {
                WriteIndented = indent
            });

            using (var writer = File.CreateText(path.ToString()))
            {
                writer.Write(serialized);
            }

            return true;
        }

        public static T DeserializeJson<T>(this FsPath path) where T: class, new()
        {
            using (var reader = File.OpenText(path.ToString()))
            {
                string text = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(text);
            }
        }

        public static FsPath GetAbsolutePathRelativeTo(this FsPath path, FsPath file)
        {
            if (path.ToString().StartsWith("../"))
            {
                path = new FsPath(path.ToString().Substring(3));
            }

            string filespec = path.ToString();
            string folder = file.ToString();
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            var pathUri = new Uri(new Uri(folder), filespec);
            return new FsPath(pathUri.ToString().Replace("file:///", "").Replace("/", "\\"));
        }

        public static FsPath GetRelativePathRelativeTo(this FsPath path, FsPath file)
        {
            string filespec = path.ToString();
            string folder = file.ToString();

            if (file.Extension == null)
                folder = Path.GetDirectoryName(file.ToString());

            Uri pathUri = new Uri(filespec);

            if (folder?.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
            {
                folder += Path.DirectorySeparatorChar;
            }

            Uri folderUri = new Uri(folder);

            var relatvie = folderUri.MakeRelativeUri(pathUri).ToString();

            var ret = Uri.UnescapeDataString(relatvie.Replace('/', Path.DirectorySeparatorChar));
            return new FsPath(ret);
        }

        public static FsPath GetDirectory(this FsPath path)
        {
            return new FsPath(Path.GetDirectoryName(path.ToString()));
        }



        /**
         * 获得targetPath相对于sourcePath的相对路径
         * @param sourcePath	: 原文件路径
         * @param targetPath	: 目标文件路径
         * @return
         */
        public static string GetRelativePath(this FsPath path, string targetPath)
        {
            string sourcePath = path.ToString().Replace("\\","/");
            targetPath = targetPath.Replace("\\", "/");
            StringBuilder pathSB = new StringBuilder();

            if (targetPath.IndexOf(sourcePath) == 0)
            {
                pathSB.Append(targetPath.Replace(sourcePath, ""));
            }
            else
            {
                String[] sourcePathArray = sourcePath.Split("/");
                String[] targetPathArray = targetPath.Split("/");
                if (targetPathArray.Length >= sourcePathArray.Length)
                {
                    for (int i = 0; i < targetPathArray.Length; i++)
                    {
                        if (sourcePathArray.Length > i && targetPathArray[i].Equals(sourcePathArray[i]))
                        {
                            continue;
                        }
                        else
                        {
                            for (int j = i; j < sourcePathArray.Length; j++)
                            {
                                pathSB.Append("../");
                            }
                            for (; i < targetPathArray.Length; i++)
                            {
                                pathSB.Append(targetPathArray[i] + "/");
                            }
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < sourcePathArray.Length; i++)
                    {
                        if (targetPathArray.Length > i && targetPathArray[i].Equals(sourcePathArray[i]))
                        {
                            continue;
                        }
                        else
                        {
                            for (int j = i; j < sourcePathArray.Length; j++)
                            {
                                pathSB.Append("../");
                            }
                            break;
                        }
                    }
                }

            }

            return pathSB.ToString().Trim('/');
        }

    }
}