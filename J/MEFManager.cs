using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System
{
    public abstract class MEFManager
    {
        private readonly AggregateCatalog aggregateCatalog = new AggregateCatalog();

        private readonly CompositionContainer container;

        private List<string> Export = new List<string>();

        protected MEFManager()
        {
            this.container = new CompositionContainer(this.aggregateCatalog, true, null);
        }

        public void ComposeParts(params object[] attributedParts)
        {
            this.container.ComposeParts(attributedParts);
        }

        public IEnumerable<T> GetExports<T>(Assembly assembly)
        {
            if (!Export.Contains(assembly.FullName))
            {
                Export.Add(assembly.FullName);
                this.aggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            try
            {
                return this.container.GetExportedValues<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new List<T>();
        }

        private bool IsLoaded = false;
        public IEnumerable<T> GetExports<T>(params object[] attributedParts)
        {
            //var rts = new List<T>();
            if (!IsLoaded)
            {
                IsLoaded = true;
                LoadPlugins();
            }

            //IEnumerable<Export> rps = new List<Export>();
            if (this.container.Catalog.Parts.Count() > 0 && ExportFiles?.Count() > 0)
            {
                //var a = this.container.TryGetExports(new ImportDefinition((di) => di.ContractName != "", "", ImportCardinality.ZeroOrMore, false, false), new AtomicComposition(), out rps);
                try
                {
                    return this.container.GetExportedValues<T>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // 导入失败不进行处理
                }
            }

            return new List<T>();
        }

        public virtual string SearchPattern { get; protected set; } = "*.*";

        private FileInfo[] ExportFiles = null;

        protected void LoadPlugins()
        {
            ExportFiles = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles(SearchPattern);
            if (ExportFiles.Count() > 0)
            {
                foreach (var item in ExportFiles)
                {
                    try
                    {
                        var name = item.Name.ToUpper();
                        var path = item.FullName.ToUpper();
                        if (path.EndsWith(".EXE") || path.EndsWith(".DLL"))
                        {
                            if (!Export.Contains(path))
                            {
                                var assembly = Assembly.LoadFile(item.FullName);
                                Export.Add(path);
                                this.aggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //LogX.Error(ex, "item is :{0}", item.FullName);
                    }
                }
            }
        }
    }
}