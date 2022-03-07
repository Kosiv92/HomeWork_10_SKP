using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TelegramLibrary
{    
    public class Repository
    {
        //string which keeps path to directory of repository
        readonly string pathToRepository;

        DirectoryInfo _repoDirectory;

        /// <summary>
        /// Condition for access to repository
        /// </summary>
        public string PathToRepository
        {
            get { return pathToRepository; }
        }

        /// <summary>
        /// Constructor of object
        /// </summary>
        public Repository()
        {
            pathToRepository = Environment.CurrentDirectory + "\\repo\\";

            CreateRepoDirectory();
        }

        /// <summary>
        /// Create directory of repository if it isn't exist
        /// </summary>
        public void CreateRepoDirectory()
        {
            if (!CheckRepoDirectory())
            {
                Directory.CreateDirectory(pathToRepository);
            }

            _repoDirectory = new DirectoryInfo(pathToRepository);
        }

        /// <summary>
        /// Check that directory of repository exist 
        /// </summary>
        /// <returns>Результат проверки</returns>
        bool CheckRepoDirectory()
        {
            DirectoryInfo path = new DirectoryInfo(pathToRepository);
            if (path.Exists) return true;
            else return false;
        }

        /// <summary>
        /// Get list of filenames in repository
        /// </summary>
        /// <returns>Список файлов хранящихся в репозитории</returns>
        public FileInfo[] GetFilesNameList()
        {
            //FileInfo[] Files = repoDirectory.GetFiles("*.pdf");
            FileInfo[] files = _repoDirectory.GetFiles();
            string str = "";
            foreach (FileInfo file in files)
            {
                str = str + ", " + file.Name;
            }
            return files;
        }

        /// <summary>
        /// Get list of files in repository
        /// </summary>
        /// <returns></returns>
        public string GetAllFileNames()
        {
            FileInfo[] files = _repoDirectory.GetFiles();
            StringBuilder fileList = new StringBuilder();
            foreach (var file in files)
            {
                fileList.Append($"- {file.Name}\n");
            }

            return fileList.ToString();

        }

    }
}
