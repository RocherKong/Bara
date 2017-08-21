using Bara.Abstract.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Bara.Model
{
    [XmlRoot(Namespace = "http://bara.github.io/schemas/BaraConfig.xsd")]
    public class BaraMapConfig
    {
        [XmlElement("Settings")]
        public Settings Settings { get; set; }

        [XmlElement("Database")]
        public DataBase DataBase { get; set; }

        [XmlArray("BaraMaps")]
        [XmlArrayItem("BaraMap")]
        public List<BaraMapSource> BaraMapSources { get; set; }

        #region Extend Prop and Methods
        [XmlIgnore]
        public IList<BaraMap> BaraMaps { get; set; }
        [XmlIgnore]
        public String Path { get; set; }
        [XmlIgnore]
        public IBaraMapper BaraMapper { get; set; }

        private IDictionary<String, Statement> _mappedStatements;

        public void ClearMappedStatements()
        {
            _mappedStatements = null;
        }
        [XmlIgnore]
        public IDictionary<String, Statement> MappedStatements
        {
            get
            {
                if (_mappedStatements == null)
                {
                    lock (this)
                    {
                        if (_mappedStatements == null)
                        {
                            _mappedStatements = new Dictionary<string, Statement>();
                            foreach (var sqlmap in BaraMaps)
                            {
                                foreach (var statement in sqlmap.Statements)
                                {
                                    var statementId = $"{sqlmap.Scope}.{statement.Id}";
                                    _mappedStatements.Add(statementId, statement);
                                }
                            }
                        }
                    }
                }
                return _mappedStatements;
            }
        }

        #endregion
    }

    public class Settings
    {
        [XmlAttribute]
        public bool IsWatchConfigFile { get; set; }
    }

    public class DataBase
    {
        [XmlElement("DbProvider")]
        public DbProvider DbProvider { get; set; }
        [XmlElement("Write")]
        public WriteDataBase WriteDataBase { get; set; }
        [XmlElement("Read")]
        public List<ReadDataSource> ReadDataSources { get; set; }
       
    }

    public class DbProvider
    {
        [XmlAttribute]
        public String Name { get; set; }
        [XmlAttribute]
        public String ParameterPrefix { get; set; }
        [XmlAttribute]
        public String Type { get; set; }

        [XmlIgnore]
        public String TypeName { get { return Type.Split(',')[0]; } }
        [XmlIgnore]
        public String AssemblyName { get { return Type.Split(',')[1]; } }
        private DbProviderFactory _dbProviderFactory;

        public DbProviderFactory DbProviderFactory
        {

            get
            {
                if (_dbProviderFactory == null)
                {
                    lock (this)
                    {
                        LoadFactory();
                    }
                }
                return _dbProviderFactory;
            }
        }

        private void LoadFactory()
        {
            _dbProviderFactory = Assembly.Load(new AssemblyName { Name = AssemblyName })
                .GetType(TypeName)
                .GetRuntimeField("Instance")
                .GetValue(null) as DbProviderFactory;
        }

    }

    public class WriteDataBase
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string ConnectionString { get; set; }

    }

    public class ReadDataSource
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string ConnectionString { get; set; }
        [XmlAttribute]
        public int Weight { get; set; }
    }

    public class BaraMapSource
    {
        [XmlAttribute]
        public String Path { get; set; }
    }


}
