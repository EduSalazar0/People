using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly PersonRepository _personRepository;
        private Person _selectedPerson;
        private string _statusMessage;
        public ObservableCollection<Person> _peopleList { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand GetAllPeopleCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }
        

        public PersonViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EduardoSalazar.db3");
            _personRepository = new PersonRepository(dbPath);

            PeopleList = new ObservableCollection<Person>();
            AddPersonCommand = new Command(async () => await AddPerson());
            GetAllPeopleCommand = new Command(async () => await GetAllPeople());
            DeletePersonCommand = new Command<Person>(async (person) => await Eliminar(person));

        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if(_selectedPerson != value)
                {
                    _selectedPerson = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewPersonName { get; set; }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<Models.Person> PeopleList
        {
            get => _peopleList;
            set
            {
                if(_peopleList != value)
                {
                    _peopleList = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task AddPerson()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewPersonName))
                {
                    StatusMessage = "El nombre no puede estar vacio";
                    return;
                }

                await _personRepository.AddNewPerson(NewPersonName);

                StatusMessage = $"Persona: '{NewPersonName}' añadida correctamente.";
            
                //Limpiar el campo de entrada
                
                OnPropertyChanged(nameof(NewPersonName));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la persona: {ex.Message}";
            }
        }

        private async Task GetAllPeople()
        {
            try
            {
                var people = await _personRepository.GetAllPeople();
                PeopleList.Clear();

                foreach (var person in people)
                {
                    PeopleList.Add(person);
                    
                }
                StatusMessage = $"Se cargaron {PeopleList.Count} personas";
                
            } catch (Exception ex)
            {
                StatusMessage = $"Error al cargar las personas: {ex.Message}";
            }
        }

        private async Task Eliminar(Models.Person personaEliminar)
        {
            try
            {
                if (personaEliminar == null)
                {
                    throw new Exception("Persona no válida.");
                }

                await _personRepository.DeleteAsync(personaEliminar.Name);
                PeopleList.Remove(personaEliminar);
                StatusMessage = $"Se eliminó a {personaEliminar.Name}.";

                await Shell.Current.DisplayAlert("Aviso!", $"Eduardo Salazar acaba de eliminar a {personaEliminar.Name}", "Aceptar");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar a la persona: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
