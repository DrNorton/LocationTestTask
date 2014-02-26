﻿using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18444
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------
namespace LocationTestTask.DataLayer
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

    public partial class LocationContext : System.Data.Linq.DataContext
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
        partial void InsertLocations(Locations instance);
        partial void UpdateLocations(Locations instance);
        partial void DeleteLocations(Locations instance);
        partial void InsertMapPositions(MapPositions instance);
        partial void UpdateMapPositions(MapPositions instance);
        partial void DeleteMapPositions(MapPositions instance);
        #endregion

        public System.Data.Linq.Table<Locations> Locations
        {
            get
            {
                return this.GetTable<Locations>();
            }
        }

        public System.Data.Linq.Table<MapPositions> MapPositions
        {
            get
            {
                return this.GetTable<MapPositions>();
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class Locations : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private long _Id;

        private long _MapPositionId;

        private System.DateTime _Datetime;

        private EntityRef<MapPositions> _MapPositions;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        partial void OnMapPositionIdChanging(long value);
        partial void OnMapPositionIdChanged();
        partial void OnDatetimeChanging(System.DateTime value);
        partial void OnDatetimeChanged();
        #endregion

        public Locations()
        {
            this._MapPositions = default(EntityRef<MapPositions>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "BigInt NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MapPositionId", DbType = "BigInt NOT NULL")]
        public long MapPositionId
        {
            get
            {
                return this._MapPositionId;
            }
            set
            {
                if ((this._MapPositionId != value))
                {
                    if (this._MapPositions.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnMapPositionIdChanging(value);
                    this.SendPropertyChanging();
                    this._MapPositionId = value;
                    this.SendPropertyChanged("MapPositionId");
                    this.OnMapPositionIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Datetime", DbType = "DateTime NOT NULL")]
        public System.DateTime Datetime
        {
            get
            {
                return this._Datetime;
            }
            set
            {
                if ((this._Datetime != value))
                {
                    this.OnDatetimeChanging(value);
                    this.SendPropertyChanging();
                    this._Datetime = value;
                    this.SendPropertyChanged("Datetime");
                    this.OnDatetimeChanged();
                }
            }
        }

        [global::System.Runtime.Serialization.IgnoreDataMember]
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "fk_Locations_MapPosition", Storage = "_MapPositions", ThisKey = "MapPositionId", OtherKey = "Id", IsForeignKey = true)]
        public MapPositions MapPositions
        {
            get
            {
                return this._MapPositions.Entity;
            }
            set
            {
                MapPositions previousValue = this._MapPositions.Entity;
                if (((previousValue != value)
                            || (this._MapPositions.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._MapPositions.Entity = null;
                        previousValue.Locations.Remove(this);
                    }
                    this._MapPositions.Entity = value;
                    if ((value != null))
                    {
                        value.Locations.Add(this);
                        this._MapPositionId = value.Id;
                    }
                    else
                    {
                        this._MapPositionId = default(long);
                    }
                    this.SendPropertyChanged("MapPositions");
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class MapPositions : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private long _Id;

        private double _Latitude;

        private double _Longitude;

        private EntitySet<Locations> _Locations;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(long value);
        partial void OnIdChanged();
        partial void OnLatitudeChanging(double value);
        partial void OnLatitudeChanged();
        partial void OnLongitudeChanging(double value);
        partial void OnLongitudeChanged();
        #endregion

        public MapPositions()
        {
            this._Locations = new EntitySet<Locations>(new Action<Locations>(this.attach_Locations), new Action<Locations>(this.detach_Locations));
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "BigInt NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Latitude", DbType = "Float NOT NULL")]
        public double Latitude
        {
            get
            {
                return this._Latitude;
            }
            set
            {
                if ((this._Latitude != value))
                {
                    this.OnLatitudeChanging(value);
                    this.SendPropertyChanging();
                    this._Latitude = value;
                    this.SendPropertyChanged("Latitude");
                    this.OnLatitudeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Longitude", DbType = "Float NOT NULL")]
        public double Longitude
        {
            get
            {
                return this._Longitude;
            }
            set
            {
                if ((this._Longitude != value))
                {
                    this.OnLongitudeChanging(value);
                    this.SendPropertyChanging();
                    this._Longitude = value;
                    this.SendPropertyChanged("Longitude");
                    this.OnLongitudeChanged();
                }
            }
        }

        [global::System.Runtime.Serialization.IgnoreDataMember]
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "fk_Locations_MapPosition", Storage = "_Locations", ThisKey = "Id", OtherKey = "MapPositionId", DeleteRule = "NO ACTION")]
        public EntitySet<Locations> Locations
        {
            get
            {
                return this._Locations;
            }
            set
            {
                this._Locations.Assign(value);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void attach_Locations(Locations entity)
        {
            this.SendPropertyChanging();
            entity.MapPositions = this;
        }

        private void detach_Locations(Locations entity)
        {
            this.SendPropertyChanging();
            entity.MapPositions = null;
        }
    }
}

