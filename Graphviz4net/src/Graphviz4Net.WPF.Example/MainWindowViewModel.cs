
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PostSharp.Samples.CustomLogging.Helpers;
using NamedPipeWrapper;

namespace Graphviz4Net.WPF.Example
{
    using Graphs;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    

    public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class VertexItem
    {
        public string VerticeName { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public LogItemVetice p;
    }

    public class SubGraphItem
    {
        public string ClassName { get; set; }
        public SubGraph<IVertice> _subGraph;
        public Color _color;
    }

    public struct ClassRelationship
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ParentClass { get; set; }
        public string ParentMethod { get; set; }

        public ClassRelationship(LogItem logItem)
        {
            ClassName = logItem.ClassName;
            MethodName = logItem.Name;
            ParentClass = logItem.ParentClass;
            ParentMethod = logItem.ParentMethod;
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        MsgServer _msgServer;

        public MainWindowViewModel()
        {
            this.Graph = new Graph<IVertice>();
            this.Graph.Changed += GraphChanged;

            //TestSubGraph(Graph);

            //_msgServer = new MsgServer("Graphviz");
            //_msgServer.MsgReceived += OnReceiveMsg;
            //_msgServer.WaitAndProcessEvents();

            //NewWay();

            OldWay();
            this.NewPersonName = "Enter new name";
            this.UpdatePersonNewName = "Enter new name";
        }

        public class AnonymousClass
        {
            public string ClassName { get; set; }
            public string Name { get; set; }
            public List<LogItem> Items { get; set; }

            public override string ToString()
            {
                return $"{{ ClassName = {ClassName}, Name = {Name}, Items = {Items} }}";
            }

            public override bool Equals(object value)
            {
                var type = value as AnonymousClass;
                return (type != null) && EqualityComparer<string>.Default.Equals(type.ClassName, ClassName) && EqualityComparer<string>.Default.Equals(type.Name, Name) && EqualityComparer<List<LogItem>>.Default.Equals(type.Items, Items);
            }

            public override int GetHashCode()
            {
                int num = 0x7a2f0b42;
                num = (-1521134295 * num) + EqualityComparer<string>.Default.GetHashCode(ClassName);
                num = (-1521134295 * num) + EqualityComparer<string>.Default.GetHashCode(Name);
                return (-1521134295 * num) + EqualityComparer<List<LogItem>>.Default.GetHashCode(Items);
            }
        }

        void NewWay()
        {
            var graph = new Graph<IVertice>();
            var allItems = ReadDatabase.Instance.FindAll();

            var groupedByClass = allItems.GroupBy(
                x => new { x.ClassName, x.Name }, 
                (key, group) =>
                {
                    var @class = new AnonymousClass();
                    @class.ClassName = key.ClassName;
                    @class.Name = key.Name;
                    @class.Items = @group.ToList();
                    return @class;
                });

            HashSet<ClassRelationship> classRelSet = new HashSet<ClassRelationship>();

            Random rnd = new Random();
            foreach (var item in groupedByClass)
            {
                var verticeName = item.ClassName + ":" + item.Name;
                var color = Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
                SolidColorBrush solid = new SolidColorBrush(color);
                var el = new LogItemVetice(graph) {  ClassName = item.ClassName, MethodName = item.Name, ItemColor = solid };
                graph.AddVertex(el);

                foreach (var logItem in item.Items)
                {
                    var classRel = new ClassRelationship(logItem);
                    if(!classRelSet.Contains(classRel))
                    {

                    }
                    else
                    {

                    }
                }

            }
            this.Graph = graph;
            this.Graph.Changed += GraphChanged;          
        }

        

        void OldWay()
         { 
            var graph = new Graph<IVertice>();


            var allItems = ReadDatabase.Instance.FindAll().Select(x => new { x.Name, x.ClassName, x.ParentClass, x.ParentMethod }).Distinct();
            var count = allItems.Count();
            var uniqueItems = allItems.Select(e => new { Name = e.Name, ClassName = e.ClassName }).Distinct();
            var uniqueItems2 = allItems.Select(e => new { Name = e.ParentMethod, ClassName = e.ParentClass }).Distinct();

            var union1 = uniqueItems.Concat(uniqueItems2).Distinct();

            var listClassNames = union1.Select(x => x.ClassName).Distinct();

            List<SubGraphItem> subGraphItems = new List<SubGraphItem>();


            Random rnd = new Random();
            foreach (var className in listClassNames)
            {
                var subGraph = new SubGraph<IVertice> { Label = className };
                var color = Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
                subGraphItems.Add(new SubGraphItem { ClassName = className, _subGraph = subGraph, _color = color });
            }

            List<VertexItem> vItems = new List<VertexItem>();

            foreach(var item in union1)
            {
                var verticeName = item.ClassName + ":" + item.Name;
                var color = subGraphItems.First(x => x.ClassName.Equals(item.ClassName))._color;
                SolidColorBrush solid = new SolidColorBrush(color);
                var el = new LogItemVetice(graph) { ClassName = item.ClassName, MethodName = item.Name, ItemColor = solid };
                var vi = new VertexItem { ClassName = item.ClassName, MethodName = item.Name, p = el };
                vItems.Add(vi);
                graph.AddVertex(el);
            }

            var intersect1 = (from a in vItems
                              join b in allItems
                                on new { a.ClassName, a.MethodName } equals new { b.ClassName, MethodName = b.Name }
                                into gp
                             select new { a.ClassName, a.MethodName, a.p, gp });


            foreach (var item in intersect1)
            {
                foreach (var element in item.gp)
                {
                    var parent = vItems.First(x => x.ClassName == element.ParentClass && x.MethodName == element.ParentMethod);

                    graph.AddEdge(new Edge<IVertice>(parent.p, item.p, new Arrow()));
                }
            }

            //foreach (var item in vItems)
            //{
            //    try
            //    {
            //        var children = allItems.Where(x => x.ParentClass.Equals(item.ClassName) && x.ParentMethod.Equals(item.MethodName)).Select(x => new { MethodName = x.Name, ClassName = x.ClassName } ).Distinct();

            //        var intersect = (from a in children
            //                         join b in vItems
            //                            on new { a.ClassName, a.MethodName } equals new { b.ClassName, b.MethodName }
            //                         select new { a.ClassName, a.MethodName, b.p });

            //        foreach (var element in intersect)
            //        {
            //            graph.AddEdge(new Edge<IVertice>(item.p, element.p, new Arrow()));
            //        }

                    
            //    }
            //    catch (Exception e)
            //    {
            //        System.Diagnostics.Debug.Write("bug");
            //    }
            //}

            //foreach (var item in union1)
            //{
            //    try
            //    { 
            //        var verticeName = item.ClassName + ":" + item.Name;
            //        var src = vItems.First(x => x.VerticeName.Equals(verticeName));
            //        var dest = allItems.First(x => x.ClassName.Equals(item.ClassName) && x.Name.Equals(item.Name));
            //        var destVersticeName = dest.ParentClass + ":" + dest.ParentMethod;
            //        var dest2 = vItems.First(x => x.VerticeName.Equals(destVersticeName));
            //        graph.AddEdge(new Edge<IVertice>(dest2.p, src.p, new Arrow()));
            //     }
            //    catch (Exception e)
            //    {
            //        System.Diagnostics.Debug.Write("bug");
            //    }
            //}

            this.Graph = graph;
            this.Graph.Changed += GraphChanged;
            this.NewPersonName = "Enter new name";
        	this.UpdatePersonNewName = "Enter new name";
        }

        

        public Graph<IVertice> Graph { get; private set; }

        public string NewPersonName { get; set; }

		public string UpdatePersonName { get; set; }

		public string UpdatePersonNewName { get; set; }

        public IEnumerable<string> PersonNames
        {
            get { return this.Graph.AllVertices.Select(x => x.VerticeName); }
        }

        public string NewEdgeStart { get; set; }

        public string NewEdgeEnd { get; set; }

        public string NewEdgeLabel { get; set; }

        public void CreateEdge()
        {
            if (string.IsNullOrWhiteSpace(this.NewEdgeStart) ||
                string.IsNullOrWhiteSpace(this.NewEdgeEnd))
            {
                return;
            }

            this.Graph.AddEdge(
                new Edge<IVertice>
                    (this.GetPerson(this.NewEdgeStart), 
                    this.GetPerson(this.NewEdgeEnd))
                    {
                        Label = this.NewEdgeLabel
                    });
        }

        public void CreatePerson()
        {
            if (this.PersonNames.Any(x => x == this.NewPersonName))
            {
                // such a person already exists: there should be some validation message, but 
                // it is not so important in a demo
                return;
            }

            var p = new LogItemVetice(this.Graph);
            this.Graph.AddVertex(p);
        }

        public void CreateNote()
        {
            //TODO  
            if (this.PersonNames.Any(x => x == this.NewPersonName))
            {
                // such a person already exists: there should be some validation message, but 
                // it is not so important in a demo
                return;
            }

            var p = new Note(this.Graph) { VerticeName = this.NewPersonName };
            this.Graph.AddVertex(p);
        }

        public void UpdatePerson()
		{
			if (string.IsNullOrWhiteSpace(this.UpdatePersonName)) 
			{
				return;
			}

			//this.GetPerson(this.UpdatePersonName).VerticeName = this.UpdatePersonNewName;
			this.RaisePropertyChanged("PersonNames");
			this.RaisePropertyChanged("Graph");
		}

        public event PropertyChangedEventHandler PropertyChanged;

        private void GraphChanged(object sender, GraphChangedArgs e)
        {
            this.RaisePropertyChanged("PersonNames");
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        //TODO return person?
        private IVertice GetPerson(string name)
        {
            return this.Graph.AllVertices.First(x => string.CompareOrdinal(x.VerticeName, name) == 0);
        }

        //TODO move into graph
        Dictionary<string, LogItemVetice> _dictVertex = new Dictionary<string, LogItemVetice>();

        private void OnReceiveMsg(object sender, MessageType message)
        {
            lock (this)
            {
                if (message is MsgNewVertex)
                {
                    var msg = message as MsgNewVertex;
                    if (_dictVertex.Keys.Contains(msg.Name))
                    {
                        //TODO better management
                        throw new Exception("Vertex already exists");
                    }
                    else
                    {
                        var item = new LogItemVetice(Graph) {  ClassName = msg.Name };
                        Graph.AddVertex(item);
                        _dictVertex.Add(msg.Name, item);
                        this.RaisePropertyChanged("Graph");
                    }
                    
                    
                }
                else if(message is MsgNewEdge)
                {
                    var msg = message as MsgNewEdge;
                    if (_dictVertex.Keys.Contains(msg.Source) && _dictVertex.Keys.Contains(msg.Destination))
                    {
                        var source = _dictVertex.GetValue(msg.Source);
                        var dest = _dictVertex.GetValue(msg.Destination);
                        //Graph.AddEdge(new Edge<IVertice>())
                        Graph.AddEdge(new Edge<IVertice>(source, dest, new Arrow()));
                    }
                    else
                    {
                        //TODO better
                        throw new Exception("Missing source or destination");
                    }
                }
            }          
        }

        private void TestSubGraph(Graph<IVertice> graph)
        {
            var g = new LogItemVetice(graph);
            var h = new LogItemVetice(graph);

            var subGraph = new SubGraph<IVertice> { Label = "Work" };
            graph.AddSubGraph(subGraph);
            subGraph.AddVertex(g);
            subGraph.AddVertex(h);
            

            this.Graph = graph;
            this.Graph.Changed += GraphChanged;
        }

        //private void TestCode()
        //{
        //    var graph = new Graph<IVertice>();
        //    var a = new LogItemVetice(graph) { VerticeName = "Jonh", Comment = "Test", Avatar = "./Avatars/avatar1.jpg" };
        //    var b = new LogItemVetice(graph) { VerticeName = "Michael", Avatar = "./Avatars/avatar2.gif" };
        //    var c = new LogItemVetice(graph) { VerticeName = "Kenny" };
        //    var d = new LogItemVetice(graph) { VerticeName = "Lisa" };
        //    var e = new LogItemVetice(graph) { VerticeName = "Lucy", Avatar = "./Avatars/avatar3.jpg" };
        //    var f = new LogItemVetice(graph) { VerticeName = "Ted Mosby" };
        //    var g = new LogItemVetice(graph) { VerticeName = "Glen" };
        //    var h = new LogItemVetice(graph) { VerticeName = "Alice", Avatar = "./Avatars/avatar1.jpg" };

        //    var un = new LogItemVetice(graph) { VerticeName = "AAAAAAAAAAAA" };

        //    graph.AddVertex(a);
        //    graph.AddVertex(b);
        //    graph.AddVertex(c);
        //    graph.AddVertex(d);
        //    graph.AddVertex(e);
        //    graph.AddVertex(f);
        //    graph.AddVertex(un);

        //    var subGraph = new SubGraph<IVertice> { Label = "Work" };
        //    graph.AddSubGraph(subGraph);
        //    subGraph.AddVertex(g);
        //    subGraph.AddVertex(h);
        //    graph.AddEdge(new Edge<IVertice>(g, h));
        //    graph.AddEdge(new Edge<IVertice>(a, g));

        //    var subGraph5 = new SubGraph<IVertice> { Label = "Testtest" };
        //    graph.AddSubGraph(subGraph5);
        //    var loner5 = new LogItemVetice(graph) { VerticeName = "Loner5", Avatar = "./Avatars/avatar1.jpg" };
        //    subGraph5.AddVertex(loner5);


        //    var subGraph2 = new SubGraph<IVertice> { Label = "School" };
        //    graph.AddSubGraph(subGraph2);
        //    var loner = new LogItemVetice(graph) { VerticeName = "Loner", Avatar = "./Avatars/avatar1.jpg" };
        //    subGraph2.AddVertex(loner);
        //    var loner2 = new LogItemVetice(graph) { VerticeName = "Loneddddddddr2", Avatar = "./Avatars/avatar1.jpg" };
        //    subGraph2.AddVertex(loner2);
        //    graph.AddEdge(new Edge<SubGraph<IVertice>>(subGraph, subGraph2) { Label = "Link between groups" });

        //    graph.AddEdge(new Edge<IVertice>(c, d) { Label = "In love", DestinationArrowLabel = "boyfriend", SourceArrowLabel = "girlfriend" });

        //    graph.AddEdge(new Edge<IVertice>(c, g, new Arrow(), new Arrow()));
        //    graph.AddEdge(new Edge<IVertice>(c, a, new Arrow()) { Label = "Boss" });
        //    graph.AddEdge(new Edge<IVertice>(d, h, new DiamondArrow(), new DiamondArrow()));
        //    graph.AddEdge(new Edge<IVertice>(f, h, new DiamondArrow(), new DiamondArrow()));
        //    graph.AddEdge(new Edge<IVertice>(f, loner, new DiamondArrow(), new DiamondArrow()));
        //    graph.AddEdge(new Edge<IVertice>(f, b, new DiamondArrow(), new DiamondArrow()));
        //    graph.AddEdge(new Edge<IVertice>(e, g, new Arrow(), new Arrow()) { Label = "Siblings" });

        //    this.Graph = graph;
        //    this.Graph.Changed += GraphChanged;
        //    this.NewPersonName = "Enter new name";
        //    this.UpdatePersonNewName = "Enter new name";
        //}
    }
}
