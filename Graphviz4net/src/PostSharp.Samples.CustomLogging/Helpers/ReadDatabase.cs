using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDB;
using System.Diagnostics;
using System.IO;

namespace PostSharp.Samples.CustomLogging.Helpers
{



    public sealed class ReadDatabase
    {
        private static readonly ReadDatabase instance = new ReadDatabase();
        static ReadDatabase() { } // Make sure it's truly lazy


        public static ReadDatabase Instance
        {
            get { return instance; }
        }


        readonly LiteDatabase _db;
        readonly LiteCollection<LogItem> _logItems;
        //Storage _db;
        //MyRootClass _root;
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private ReadDatabase()
        {
            _db = new LiteDatabase(@"C:\Database\LiteDB\Graphviz4Net\Read\LogItem_001.db");
            _logItems = _db.GetCollection<LogItem>("LogItem");
        }

        void TestLinq()
        {
            var db = new LiteDatabase(@"C:\Database\LiteDB\Graphviz4Net\Read\LogItem_001.db");
            var logItems = db.GetCollection<LogItem>("LogItem");
            var all = logItems.FindAll();
            var ct = all.Count();
        }
        //private void Perst()
        //{
        //    _db = StorageFactory.Instance.CreateStorage();
        //    _db.Open(@"c:\Database\Perst\Robot.dbs");
        //    _root = (MyRootClass)_db.Root; // get storage root
        //    if (_root == null)
        //    {
        //        // Root is not yet defined: stotage is not initialized
        //        _root = new MyRootClass(_db); // create root object
        //        _db.Root = _root; // register root object
        //    }
        //}

        public void Add(LogItem item)
        {
            item.Milliseconds = _stopwatch.ElapsedMilliseconds;
            _logItems.Insert(item);
            //_root.intKeyIndex.Put(item);
            //_db.Commit();
        }

        public void Query()
        {
            int count1 = _logItems.Count();
            //int count = _root.intKeyIndex.GroupBy(x => x.Name).Count();
            //var count = _logItems.Count();

            var uniqueItems = _logItems.FindAll().Select(e => new { Name = e.Name, ClassName = e.ClassName, ParentMethod = e.ParentMethod, ParentClass = e.ParentClass }).Distinct();
            using (StreamWriter writer = new StreamWriter("items4.txt"))
            {
                foreach (var item in uniqueItems)
                {
                    writer.WriteLine(item.ClassName + ", " + item.Name + ", " + item.ParentClass + ", " + item.ParentMethod);
                }
            }

        }

        public IEnumerable<LogItem> FindAll()
        {
            return _logItems.FindAll();
        }

        public void GetAllMethods()
        {
            var uniqueItems = _logItems.FindAll().Select(e => new { Name = e.Name, ClassName = e.ClassName }).Distinct();
            var uniqueItems2 = _logItems.FindAll().Select(e => new { Name = e.ParentMethod, ClassName = e.ParentClass }).Distinct();

            var union1 = uniqueItems.Concat(uniqueItems2).Distinct();

            using (StreamWriter writer = new StreamWriter("items7.txt"))
            {
                foreach (var item in union1)
                {
                    writer.WriteLine(item.ClassName + ", " + item.Name);
                }
            }
        }
    }
}
