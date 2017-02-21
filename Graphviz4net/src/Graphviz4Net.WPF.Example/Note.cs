using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Graphviz4Net.WPF.Example
{
    public class Note : INotifyPropertyChanged, IVertice
    {
        private readonly Graph<IVertice> graph;

        public Note(Graph<IVertice> graph)
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
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
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

        public string Email
        {
            get
            {
                return this.VerticeName.ToLower().Replace(' ', '.') + "@gmail.com";
            }
        }

        public ICommand RemoveCommand
        {
            get { return new RemoveCommandImpl(this); }
        }

        private class RemoveCommandImpl : ICommand
        {
            private Note note;

            public RemoveCommandImpl(Note note)
            {
                this.note = note;
            }

            public void Execute(object parameter)
            {
                this.note.graph.RemoveVertexWithEdges(this.note);
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
