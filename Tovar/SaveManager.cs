using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tovar
{
    public interface ISaveManager
    {
        void WriteLine(string line);
        void WriteObject(IWritableObject obj);
    }

    public interface IWritableObject
    {
        void Write(ISaveManager man);
    }
    class SaveManager: ISaveManager
    {
        public event EventHandler<IWritableObject> ObjectDidSave;

        FileInfo file;

        public SaveManager(string filename)
        {
            file = new FileInfo(filename);
            ObjectDidSave += PrintObjectToConsole;
        }

        private void PrintObjectToConsole(object sender, IWritableObject e)
        {
            Console.WriteLine($"Сохранен объект: {e.ToString()}");
        }

        public void CreateFile()
        {
            if (file.Exists) file.Delete();
            FileStream fileStream = file.Create();
            fileStream.Close();
        }

        public void WriteLine(string line)
        {
            var output = file.AppendText();
            output.WriteLine(line);
            output.Close();
        }

        public void WriteObject(IWritableObject obj)
        {
            obj.Write(this);
            if (ObjectDidSave != null)
                ObjectDidSave.Invoke(this, obj);
        }
    }
}