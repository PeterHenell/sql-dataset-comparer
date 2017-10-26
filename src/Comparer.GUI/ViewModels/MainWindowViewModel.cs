using Comparer.GUI.Logging;
using DataSetTools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Comparer.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<ProjectViewModel> _openProjects;
        public ObservableCollection<ProjectViewModel> OpenProjects
        {
            get { return _openProjects; }
            set
            {
                if (_openProjects != value)
                {
                    _openProjects = value;
                    RaisePropertyChanged("OpenProjects");
                }
            }
        }

        

        private ProjectViewModel _activeProject;
        public ProjectViewModel ActiveProject
        {
            get { return _activeProject; }
            set
            {
                if (_activeProject != value)
                {
                    _activeProject = value;
                    RaisePropertyChanged("ActiveProject");
                }
            }
        }

        public RelayCommand<ProjectViewModel> SelectProjectCommand { private set; get; }

        public MainWindowViewModel()
        {
            _openProjects = new ObservableCollection<ProjectViewModel>();

            LoadCurrentProject();

            SelectProjectCommand = new RelayCommand<ProjectViewModel>((project) => 
            {
                ActiveProject = project;
            });
        }

        private void LoadCurrentProject()
        {
            var proj = new ProjectViewModel();

            // Load saved state if exists
            if (System.IO.File.Exists(ProjectViewModel.SaveFilePath))
            {
                try
                {
                    using (var stream = System.IO.File.OpenRead(ProjectViewModel.SaveFilePath))
                    {
                        var serializer = new XmlSerializer(typeof(QueryConfiguration));
                        var loaded = serializer.Deserialize(stream) as QueryConfiguration;

                        proj.QueryConfiguration = loaded;
                        OpenProjects.Add(proj);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    // in case of failed deserialization, we kill the file.
                    //System.IO.File.Delete(MainWindowViewModel.SaveFilePath);
                }
            }
            ActiveProject = proj;
        }

        internal void SaveState()
        {
            ActiveProject.SaveState();
        }
    }
}
