using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tovar
{
    public interface ILoadManager
    {
        string ReadLine();
        T Read<T>(IReadableObjectLoader<T> loader) where T : IReadableObject;
    }

    public interface IReadableObject
    { }

    public interface IReadableObjectLoader<T> where T : IReadableObject
    {
        T Load(ILoadManager man);
    }

    class LoadManager : ILoadManager
    {
        public event EventHandler<IReadableObject> ObjectDidLoad;

        FileInfo file;
        StreamReader input;
        public LoadManager(string filename)
        {
            file = new FileInfo(filename);
            input = null;
            this.ObjectDidLoad += PrintObjectToConsole;
        }

        private void PrintObjectToConsole(object sender, IReadableObject e)
        {
            Console.WriteLine($"Загружен объект: {e.ToString()}");
        }

        public T Read<T>(IReadableObjectLoader<T> loader) where T : IReadableObject
        {
            var result = loader.Load(this);

            if (ObjectDidLoad != null)
                ObjectDidLoad.Invoke(this, result);

            return result;
        }

        public void BeginRead()
        {
            if (input != null)
                throw new IOException("Load Error");

            input = file.OpenText();
        }
        public bool IsLoading
        {
            get { return input != null && !input.EndOfStream; }
        }
        public string ReadLine()
        {
            if (input == null)
                throw new IOException("Load Error");

            string line = input.ReadLine();
            return line;
        }

        public void EndRead()
        {
            if (input == null)
                throw new IOException("Load Error");

            input.Close();
        } 
    }
}
