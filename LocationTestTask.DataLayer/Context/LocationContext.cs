﻿using System.Data.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using LocationTestTask.DataLayer.DataEntities;
#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18444
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LocationTestTask.DataLayer.Context
{
    public class DebugWriter : TextWriter
    {
        private const int DefaultBufferSize = 256;
        private System.Text.StringBuilder _buffer;

        public DebugWriter()
        {
            BufferSize = 256;
            _buffer = new System.Text.StringBuilder(BufferSize);
        }

        public int BufferSize
        {
            get;
            private set;
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        #region StreamWriter Overrides
        public override void Write(char value)
        {
            _buffer.Append(value);
            if (_buffer.Length >= BufferSize)
                Flush();
        }

        public override void WriteLine(string value)
        {
            Flush();

            using (var reader = new StringReader(value))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                    System.Diagnostics.Debug.WriteLine(line);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Flush();
        }

        public override void Flush()
        {
            if (_buffer.Length > 0)
            {
                System.Diagnostics.Debug.WriteLine(_buffer);
                _buffer.Clear();
            }
        }
        #endregion
    }


    public partial class LocationContext : System.Data.Linq.DataContext, ILocationContext
    {

        public bool CreateIfNotExists()
        {
            bool created = false;
            if (!this.DatabaseExists())
            {
                string[] names = this.GetType().Assembly.GetManifestResourceNames();
                string name = names.Where(n => n.EndsWith(FileName)).FirstOrDefault();
                if (name != null)
                {
                    using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                    {
                        if (resourceStream != null)
                        {
                            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                            {
                                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
                                {
                                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                                    {
                                        long length = resourceStream.Length;
                                        byte[] buffer = new byte[32];
                                        int readCount = 0;
                                        using (BinaryReader reader = new BinaryReader(resourceStream))
                                        {
                                            // read file in chunks in order to reduce memory consumption and increase performance
                                            while (readCount < length)
                                            {
                                                int actual = reader.Read(buffer, 0, buffer.Length);
                                                readCount += actual;
                                                writer.Write(buffer, 0, actual);
                                            }
                                        }
                                    }
                                }
                            }
                            created = true;
                        }
                        else
                        {
                            this.CreateDatabase();
                            created = true;
                        }
                    }
                }
                else
                {
                    this.CreateDatabase();
                    created = true;
                }
            }
            return created;
        }

        public bool LogDebug
        {
            set
            {
                if (value)
                {
                    this.Log = new DebugWriter();
                }
            }
        }

        public static string ConnectionString = "Data Source=isostore:/WPLocation.sdf";

        public static string ConnectionStringReadOnly = "Data Source=appdata:/WPLocation.sdf;File Mode=Read Only;";

        public static string FileName = "WPLocation.sdf";

        public LocationContext(string connectionString)
            : base(connectionString)
        {
            OnCreated();
        }

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertLocation(Location instance);
        partial void UpdateLocation(Location instance);
        partial void DeleteLocation(Location instance);
        partial void InsertMapPosition(MapPosition instance);
        partial void UpdateMapPosition(MapPosition instance);
        partial void DeleteMapPosition(MapPosition instance);
        #endregion

        public System.Data.Linq.Table<Location> Locations
        {
            get
            {
                return this.GetTable<Location>();
            }
        }

        public System.Data.Linq.Table<MapPosition> MapPositions
        {
            get
            {
                return this.GetTable<MapPosition>();
            }
        }


       
    }
}
#pragma warning restore 1591
