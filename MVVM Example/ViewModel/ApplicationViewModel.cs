using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using MVVM_Example.Model;
using MVVM_Example.ViewModel.Commands;

namespace MVVM_Example.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {

        private Flower selectedPhone;

        private int selectedPhoneIndex;

        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        
        public ObservableCollection<Flower> Phones { get; set; }

        public Flower SelectedPhone
        {
            get { return selectedPhone; }
            set
            {
                selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public int SelectedPhoneIndex
        {
            get { return selectedPhoneIndex; }
            set
            {
                selectedPhoneIndex = value; 
            }
        }
        
        // Command to save file
        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                       (_saveCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.SaveFileDialog() == true)
                               {
                                   _fileService.Save(_dialogService.FilePath,
                                       Phones.Select(phone => new Flower
                                               {Description = phone.Description, Price = phone.Price, Title = phone.Title})
                                           .ToList());
                                   _dialogService.ShowMessage("File saved");
                               }
                           }
                           catch (Exception ex)
                           {
                               _dialogService.ShowMessage(ex.Message);
                           }
                       }));
            }
        }
        
        //Command to open file
        private RelayCommand _openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand ??
                       (_openCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.OpenFileDialog() == true)
                               {
                                   var phones = _fileService.Open(_dialogService.FilePath);
                                   Phones.Clear();
                                   foreach (var phone in phones)
                                   {
                                       Phones.Add(phone);
                                   }

                                   _dialogService.ShowMessage("File opened");
                               }
                           }
                           catch (Exception ex)
                           {
                               _dialogService.ShowMessage(ex.Message);
                           }
                       }));
            }
        }

        // Command to add new object
        private RelayCommand _addCommand;

        public RelayCommand AddCommand
        {
            get { return _addCommand ??
                         (_addCommand = new RelayCommand(obj =>
                         {
                            Flower phone = new Flower();
                            Phones.Insert(0, phone);
                            SelectedPhone = phone;
                            
                         })); }
        }
        
        // Command to delete the object
        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                       (_removeCommand = new RelayCommand(obj =>
                           {
                               Flower phone = obj as Flower;
                               var selectedIndex = selectedPhoneIndex;
                               if (phone != null)
                               {
                                   Phones.Remove(phone);
                               }

                               UpdatePhonesListSelection(Phones.Count, selectedIndex);
                           },
                           (obj) => Phones.Count > 0)); //Button will be disabled in "canExecute = false"
            }
        }

        private void UpdatePhonesListSelection(int phonesCount, int selectedIndex)
        {
            if (phonesCount == 0)
            {
                SelectedPhone = null;
            }
            else if (phonesCount <= selectedIndex)
            {
                SelectedPhone = Phones[phonesCount - 1];
            }
            else
            {
                SelectedPhone = Phones[selectedIndex];
            }
        }

        //Command to create Phone record copy 
        private RelayCommand _copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand ??
                       (_copyCommand = new RelayCommand(obj =>
                       {
                           Flower phone = obj as Flower;
                           if (phone != null)
                           {
                               Flower phoneCopy = new Flower
                               {
                                   Description = phone.Description,
                                   Price = phone.Price,
                                   Title = phone.Title

                               };
                               Phones.Insert(0, phoneCopy);
                           }
                       }));
            }
        }

        public ApplicationViewModel(IDialogService dialogService, IFileService fileService)
        {
            this._dialogService = dialogService;
            this._fileService = fileService;
            
            //Default data
            Phones = new ObservableCollection<Flower>
            {
                new Flower {Title = "Lily",  Description = "Lilium is a genus of herbaceous", Price = 100000, Image = @"/Images/img1.jpg"},
                new Flower {Title = "Iris",  Description = "Iris is a flowering plant genus", Price = 110000, Image = @"/Images/img2.jpg"},
                new Flower {Title = "Tulip", Description = "Tulips are a genus of spring", Price = 70000, Image = @"/Images/img1.jpg"}
            };

            //ImagePath = @"pack://application:,,,/Images/img1.jpg";
            //ImageSource = new BitmapImage(new Uri(ImagePath));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}