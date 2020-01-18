using System;
using System.IO;
using System.Runtime.InteropServices;

namespace gitbook.core
{
    public sealed class FsPath : IEquatable<FsPath>
    {
        private readonly string _path;

        private FsPath()
        {
            _path = string.Empty;
        }

        public FsPath(params string[] pathParts)
        {
            var combined = Path.Combine(pathParts);

            if (combined.Contains("\\"))
            {
                _path = combined.Replace("\\", "/");
            }
            else
            {
                _path = combined;
            }
        }

        public FsPath Combine(string part)
        {
            return new FsPath(_path, part);
        }

        public bool IsExisting
        {
            get
            {
                return Directory.Exists(_path) || File.Exists(_path);
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(_path);
            }
        }

        public string Filename
        {
            get { return Path.GetFileName(_path); }
        }

        public override string ToString()
        {
            return _path;
        }

        public static FsPath Empty
        {
            get { return new FsPath(); }
        }

        public static bool IsEmptyPath(FsPath path)
        {
            return string.IsNullOrEmpty(path._path);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FsPath);
        }

        public bool Equals(FsPath? other)
        {
            return
                _path == other?._path;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_path);
        }

        public static bool operator ==(FsPath? left, FsPath? right)
        {
            return left?._path == right?._path;
        }

        public static bool operator !=(FsPath? left, FsPath? right)
        {
            return !(left == right);
        }
    }
}
