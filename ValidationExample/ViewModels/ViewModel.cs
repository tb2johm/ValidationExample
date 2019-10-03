using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MMVVM.ViewModelBase;
using MMVVM.Commands;
using Version = ValidationExample.Models.Version;
using ValidationExample.Extentions;

namespace ValidationExample.ViewModels
{
    public class ViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        private Version _version;
        private string _invalidVersion;
        public string Version
        {
            get { return _version?.ToString() ?? _invalidVersion; }
            set
            {
                if (ValidationExample.Models.Version.TryParse(value, out ValidationExample.Models.Version version))
                {
                    _invalidVersion = null;
                    _version = version;
                    _errors[nameof(Version)]?.Clear();
                }
                else
                {
                    _version = null;
                    _invalidVersion = value;
                    _errors.GetOrCreate(nameof(Version)).Add( $"Couldn't convert {_invalidVersion} to valid Version" );
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Version)));
                }
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand SubmitCommand { get; private set; }


        public ViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                _version = new Version(4, 2);
            }

            SubmitCommand = new RelayCommand(() => Version = null, () => !HasErrors);
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errors.Values.FirstOrDefault(err => err.Count > 0) == null ? false : true;
        public IEnumerable GetErrors(string propertyName)
        {
            _errors.TryGetValue(propertyName, out List<string> errors);
            if (errors != null) return errors;

            return null;
        }
    }

}
