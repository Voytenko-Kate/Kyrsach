using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tovar
{
    public class Logger
    {
        protected TextWriter output;

        public Logger(TextWriter output)
        {
            this.output = output;
        }
        
        public virtual void WriteLine(string message)
        {
            output.WriteLine(message);
        }

        public void Close()
        {
            output.Close();
        }
    }

    public class LoadLogger
    {
        LoadManager loadManager;
        Logger logger;

        public LoadLogger(LoadManager loadManager, Logger logger)
        {
            this.loadManager = loadManager;
            this.logger = logger;
            loadManager.DidStartLoad += LoadStarted;
            loadManager.DidEndLoad += LoadFinished;
            loadManager.ObjectDidLoad += ObjectLoaded;
        }

        private void ObjectLoaded(object sender, IReadableObject e)
        {
            logger.WriteLine($"{DateTime.Now.ToString()} Object loaded: {e.ToString()}");
        }

        private void LoadStarted(object sender, FileInfo e)
        {
            logger.WriteLine($"{DateTime.Now.ToString()} Loading started from: {e.FullName}");
        }

        private void LoadFinished(object sender, FileInfo e)
        {
            logger.WriteLine($"{DateTime.Now.ToString()} Loading from {e.FullName} finished.");
            logger.Close();
        }
    }
}
