
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PostSharp.Samples.CustomLogging.Helpers;

namespace Graphviz4Net.WPF.Example
{
    using Graphs;
    using System.ComponentModel;
    using System.Windows.Media;

    public class Person : INotifyPropertyChanged, IVertice
    {
        private readonly Graph<IVertice> graph;

        public Person(Graph<IVertice> graph)
        {
            this.graph = graph;
            this.Avatar = "./Avatars/avatarAnon.gif";
        }

    	private string name;
    	public string VerticeName
        {
    		get { return this.name; }
    		set
    		{
    			this.name = value;
    			if (this.PropertyChanged != null) {
    				this.PropertyChanged(this, new PropertyChangedEventArgs("VerticeName"));
    			}
    		}
    	}

        SolidColorBrush mySolidColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

        private Brush itemColor;
        public Brush ItemColor
        {
            get { return itemColor; }
            set
            {
                this.itemColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ItemColor"));
                }
            }
        }

        private string className;
        public string ClassName
        {
            get { return this.className; }
            set
            {
                this.className = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ClassName"));
                }
            }
        }

        private string methodName;
        public string MethodName
        {
            get { return this.methodName; }
            set
            {
                this.methodName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("MethodName"));
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return this.comment; }
            set
            {
                this.comment = value;
                if (this.PropertyChanged != null) {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Comment"));
                }
            }
        }

        public string Avatar { get; set; }


        public ICommand AddCommand
        {
            get { return new AddCommandImpl(this); }
        }

        private class AddCommandImpl : ICommand
        {
            private Person person;

            public AddCommandImpl(Person person)
            {
                this.person = person;
            }

            public void Execute(object parameter)
            {
                var graph = this.person.graph;
                var g = Guid.NewGuid();
                string name = g.ToString().Substring(0, 10);
                var p = new Person(graph) { VerticeName = name };
                graph.AddVertexNoRedraw(p);   
                graph.AddEdge(new Edge<IVertice>(this.person, p, new Arrow(), new Arrow()));
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

        public ICommand RemoveCommand
        {
            get { return new RemoveCommandImpl(this); }
        }

        private class RemoveCommandImpl : ICommand
        {
            private Person person;

            public RemoveCommandImpl(Person person)
            {
                this.person = person;
            }

            public void Execute(object parameter)
            {
                this.person.graph.RemoveVertexWithEdges(this.person);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

    	public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class VertexItem
    {
        public string VerticeName { get; set; }
        public Person p;
    }

    public class SubGraphItem
    {
        public string ClassName { get; set; }
        public SubGraph<IVertice> _subGraph;
        public Color _color;
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            var graph = new Graph<IVertice>();


            var allItems = Database.Instance.FindAll();

            var uniqueItems = allItems.Select(e => new { Name = e.Name, ClassName = e.ClassName }).Distinct();
            var uniqueItems2 = allItems.Select(e => new { Name = e.ParentMethod, ClassName = e.ParentClass }).Distinct();

            var union1 = uniqueItems.Concat(uniqueItems2).Distinct();

            var listClassNames = union1.Select(x => x.ClassName).Distinct();

            List<SubGraphItem> subGraphItems = new List<SubGraphItem>();


            Random rnd = new Random();
            foreach (var className in listClassNames)
            {
                var subGraph = new SubGraph<IVertice> { Label = className };
                //graph.AddSubGraph(subGraph);
                var color = Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
                subGraphItems.Add(new SubGraphItem { ClassName = className, _subGraph = subGraph, _color = color });
            }

            List<VertexItem> vItems = new List<VertexItem>();

            foreach(var item in union1)
            {
                var verticeName = item.ClassName + ":" + item.Name;
                var color = subGraphItems.First(x => x.ClassName.Equals(item.ClassName))._color;
                SolidColorBrush solid = new SolidColorBrush(color);
                var el = new Person(graph) { VerticeName = verticeName, ClassName = item.ClassName, MethodName = item.Name, ItemColor = solid };
                var vi = new VertexItem { VerticeName = verticeName, p = el };
                vItems.Add(vi);
                
                var sub = subGraphItems.First(x => x.ClassName.Equals(item.ClassName));
                //sub._subGraph.AddVertex(el);
                graph.AddVertex(el);
            }

            foreach (var item in union1)
            {
                try
                { 
                    var verticeName = item.ClassName + ":" + item.Name;
                    var src = vItems.First(x => x.VerticeName.Equals(verticeName));
                    var dest = allItems.First(x => x.ClassName.Equals(item.ClassName) && x.Name.Equals(item.Name));
                    var destVersticeName = dest.ParentClass + ":" + dest.ParentMethod;
                    var dest2 = vItems.First(x => x.VerticeName.Equals(destVersticeName));
                    graph.AddEdge(new Edge<IVertice>(dest2.p, src.p));
                 }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write("bug");
                }
            }

            



            //var a = new Person(graph) { VerticeName = "Jonh", Comment = "Test", Avatar = "./Avatars/avatar1.jpg" };
            //var b = new Person(graph) { VerticeName = "Michael", Avatar = "./Avatars/avatar2.gif" };
            //var c = new Person(graph) { VerticeName = "Kenny" };
            //var d = new Person(graph) { VerticeName = "Lisa" };
            //var e = new Person(graph) { VerticeName = "Lucy", Avatar = "./Avatars/avatar3.jpg" };
            //var f = new Person(graph) { VerticeName = "Ted Mosby" };
            //var g = new Person(graph) { VerticeName = "Glen" };
            //var h = new Person(graph) { VerticeName = "Alice", Avatar = "./Avatars/avatar1.jpg" };

            //var un = new Person(graph) { VerticeName = "AAAAAAAAAAAA"};

            //graph.AddVertex(a);
            //graph.AddVertex(b);
            //graph.AddVertex(c);
            //graph.AddVertex(d);
            //graph.AddVertex(e);
            //graph.AddVertex(f);
            //graph.AddVertex(un);

            //var subGraph = new SubGraph<IVertice> { Label = "Work" };
            //graph.AddSubGraph(subGraph);
            //subGraph.AddVertex(g);
            //subGraph.AddVertex(h);
            //graph.AddEdge(new Edge<IVertice>(g, h));
            //graph.AddEdge(new Edge<IVertice>(a, g));

            //var subGraph5 = new SubGraph<IVertice> { Label = "Testtest" };
            //graph.AddSubGraph(subGraph5);
            //var loner5 = new Person(graph) { VerticeName = "Loner5", Avatar = "./Avatars/avatar1.jpg" };
            //subGraph5.AddVertex(loner5);


            //var subGraph2 = new SubGraph<IVertice> {Label = "School"};
            //graph.AddSubGraph(subGraph2);
            //var loner = new Person(graph) { VerticeName = "Loner", Avatar = "./Avatars/avatar1.jpg" };
            //subGraph2.AddVertex(loner);
            //var loner2 = new Person(graph) { VerticeName = "Loneddddddddr2", Avatar = "./Avatars/avatar1.jpg" };
            //subGraph2.AddVertex(loner2);
            //graph.AddEdge(new Edge<SubGraph<IVertice>>(subGraph, subGraph2) { Label = "Link between groups" } );

            //graph.AddEdge(new Edge<IVertice>(c, d) { Label = "In love", DestinationArrowLabel = "boyfriend", SourceArrowLabel = "girlfriend" });

            //graph.AddEdge(new Edge<IVertice>(c, g, new Arrow(), new Arrow()));
            //graph.AddEdge(new Edge<IVertice>(c, a, new Arrow()) { Label = "Boss" });
            //graph.AddEdge(new Edge<IVertice>(d, h, new DiamondArrow(), new DiamondArrow()));
            //graph.AddEdge(new Edge<IVertice>(f, h, new DiamondArrow(), new DiamondArrow()));
            //graph.AddEdge(new Edge<IVertice>(f, loner, new DiamondArrow(), new DiamondArrow()));
            //graph.AddEdge(new Edge<IVertice>(f, b, new DiamondArrow(), new DiamondArrow()));
            //graph.AddEdge(new Edge<IVertice>(e, g, new Arrow(), new Arrow()) { Label = "Siblings" });

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

            var p = new Person(this.Graph) { VerticeName = this.NewPersonName };
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

			this.GetPerson(this.UpdatePersonName).VerticeName = this.UpdatePersonNewName;
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


        //private void TestCode()
        //{
        //    var graph = new Graph<IVertice>();
        //    var a = new Person(graph) { VerticeName = "Jonh", Comment = "Test", Avatar = "./Avatars/avatar1.jpg" };
        //    var b = new Person(graph) { VerticeName = "Michael", Avatar = "./Avatars/avatar2.gif" };
        //    var c = new Person(graph) { VerticeName = "Kenny" };
        //    var d = new Person(graph) { VerticeName = "Lisa" };
        //    var e = new Person(graph) { VerticeName = "Lucy", Avatar = "./Avatars/avatar3.jpg" };
        //    var f = new Person(graph) { VerticeName = "Ted Mosby" };
        //    var g = new Person(graph) { VerticeName = "Glen" };
        //    var h = new Person(graph) { VerticeName = "Alice", Avatar = "./Avatars/avatar1.jpg" };

        //    var un = new Person(graph) { VerticeName = "AAAAAAAAAAAA" };

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
        //    var loner5 = new Person(graph) { VerticeName = "Loner5", Avatar = "./Avatars/avatar1.jpg" };
        //    subGraph5.AddVertex(loner5);


        //    var subGraph2 = new SubGraph<IVertice> { Label = "School" };
        //    graph.AddSubGraph(subGraph2);
        //    var loner = new Person(graph) { VerticeName = "Loner", Avatar = "./Avatars/avatar1.jpg" };
        //    subGraph2.AddVertex(loner);
        //    var loner2 = new Person(graph) { VerticeName = "Loneddddddddr2", Avatar = "./Avatars/avatar1.jpg" };
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
