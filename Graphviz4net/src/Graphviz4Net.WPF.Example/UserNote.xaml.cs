using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graphviz4Net.WPF.Example {
    /// <summary>
    /// Interaction logic for UserNote.xaml
    /// </summary>
    public partial class UserNote : UserControl, INotifyPropertyChanged, IVertice
    {
        private readonly Graph<IVertice> graph;

        public UserNote() {
            InitializeComponent();
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

        

        public UserNote(Graph<IVertice> graph)
        {
            this.graph = graph;
            this.Avatar = "./Avatars/avatarAnon.gif";
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
            private UserNote note;

            public RemoveCommandImpl(UserNote note)
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
