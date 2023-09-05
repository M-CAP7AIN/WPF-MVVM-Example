using System.Collections.Generic;
using MVVM_Example.Model;

namespace MVVM_Example.ViewModel
{
    public interface IFileService
    {
        List<Flower> Open(string fileName);
        void Save(string fileName, List<Flower> phoneList);
    }
}