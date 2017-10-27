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
using System.Windows.Forms;
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
        public RelayCommand NewProjectCommand { private set; get; }
        public RelayCommand OpenProjectCommand { private set; get; }
        public RelayCommand CloseActiveProjectCommand { private set; get; }
        public RelayCommand CloseAllCommand { private set; get; }
        public RelayCommand ExitWindowCommand { private set; get; }
        public RelayCommand SaveProjectCommand { private set; get; }
        public RelayCommand SaveAsProjectCommand { private set; get; }
        


        public MainWindowViewModel()
        {
            _openProjects = new ObservableCollection<ProjectViewModel>();


            SelectProjectCommand = new RelayCommand<ProjectViewModel>((project) =>
            {
                SelectActiveProject(project);
            });

            NewProjectCommand = new RelayCommand(() =>
            {
                var proj = new ProjectViewModel();
                proj.ProjectName = ProjectViewModel.UnNamedProjectName; // bug waiting to happen. names should be more unique because of how saving saving works at
                SelectActiveProject(proj);
                OpenProjects.Add(proj);
            });

            CloseActiveProjectCommand = new RelayCommand(() =>
            {
                if (ActiveProject != null)
                {
                    // TODO: Ask if to save first
                    ActiveProject.SaveState();
                    OpenProjects.Remove(ActiveProject);

                    SelectActiveProject(OpenProjects.LastOrDefault());
                }
            });

            OpenProjectCommand = new RelayCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var proj = ProjectViewModel.FromFile(openFileDialog.FileName);
                    if (proj != null)
                    {
                        SelectActiveProject(proj);
                        OpenProjects.Add(proj);
                    }
                }
            });

            SaveProjectCommand = new RelayCommand(() => {
                if (ActiveProject.ProjectName == ProjectViewModel.UnNamedProjectName)
                {
                    SaveAs();
                } else
                {
                    ActiveProject.SaveState();
                }
                
            });

            SaveAsProjectCommand = new RelayCommand(() =>
            {
                SaveAs();
            });

            CloseAllCommand = new RelayCommand(() => {
                foreach (var item in OpenProjects)
                {
                    item.SaveState();
                }

                ActiveProject = null;
                OpenProjects.Clear();
            });
        }

        private void SelectActiveProject(ProjectViewModel project)
        {
            if (project != null)
            {
                if (ActiveProject != null)
                {
                    ActiveProject.IsProjectSelected = false;
                }
                
                project.IsProjectSelected = true;
            }
            ActiveProject = project;
        }

        private void SaveAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ActiveProject.ProjectName = saveDialog.FileName;
                ActiveProject.SaveState();
            }
        }

        internal void SaveState()
        {
            foreach (var item in OpenProjects)
            {
                item.SaveState();
            }
        }
    }
}

