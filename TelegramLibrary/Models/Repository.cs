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
        /// <summary>
        /// Путь к репозиторию (хранилищу файлов)
        /// </summary>
        readonly string pathToRepository;

        DirectoryInfo _repoDirectory;

        /// <summary>
        /// Свойства доступа к репозиторию
        /// </summary>
        public string PathToRepository
        {
            get { return pathToRepository; }
        }

        
        public Repository()
        {
            pathToRepository = Environment.CurrentDirectory + "\\repo\\";

            CreateRepoDirectory();
        }

        
        public void CreateRepoDirectory()
        {
            if (!CheckRepoDirectory())
            {
                Directory.CreateDirectory(pathToRepository);
            }

            _repoDirectory = new DirectoryInfo(pathToRepository);
        }
                
        bool CheckRepoDirectory()
        {
            DirectoryInfo path = new DirectoryInfo(pathToRepository);
            if (path.Exists) return true;
            else return false;
        }

        /// <summary>
        /// Получить список файлов находящихся в репозитории
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
        /// Получить список названий файлов хранящихся в репозитории
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
