using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Graphviz4Net.Graphs;
using Graphviz4Net.WPF.Example;

namespace Graphviz4Net.WPF.Example
{
    public class LogItemVetice : INotifyPropertyChanged, IVertice
    {
        private readonly Graph<IVertice> graph;
        private static volatile int _verticeId;

        public LogItemVetice(Graph<IVertice> graph)
        {
            _verticeId++;
            this.graph = graph;
            this.Avatar = "./Avatars/avatarAnon.gif";
        }

        
        public string VerticeName => _verticeId.ToString();


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
                if (this.PropertyChanged != null)
                {
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
            private LogItemVetice logItemVetice;

            public AddCommandImpl(LogItemVetice logItemVetice)
            {
                this.logItemVetice = logItemVetice;
            }

            public void Execute(object parameter)
            {
                var graph = this.logItemVetice.graph;
                //var g = Guid.NewGuid();
                //string name = g.ToString().Substring(0, 10);
                var p = new LogItemVetice(graph);
                graph.AddVertexNoRedraw(p);
                graph.AddEdge(new Edge<IVertice>(this.logItemVetice, p, new Arrow(), new Arrow()));
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
            private LogItemVetice logItemVetice;

            public RemoveCommandImpl(LogItemVetice logItemVetice)
            {
                this.logItemVetice = logItemVetice;
            }

            public void Execute(object parameter)
            {
                this.logItemVetice.graph.RemoveVertexWithEdges(this.logItemVetice);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
