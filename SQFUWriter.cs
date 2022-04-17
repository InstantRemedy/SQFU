using MySystem;
using System.IO;

namespace SQFUSystem
{
    class SQFUWriter //NOT USE for gaming, can use for debugging, or check
    {
        private string _sqfuF;
        private SQFUReader _sQFUReader;
        private string _path;

        public SQFUWriter(string path)
        {
            if(_CheckExtension(path))
            {
                try
                {
                    _sQFUReader = new SQFUReader(path);
                    StreamReader sr = new StreamReader(path);
                    _sqfuF = sr.ReadToEnd();
                    this._path = path;
                    sr.Close();

                }
                catch
                {
                    throw new FileLoadException();
                }
            }
            else
            {
                throw new FileLoadException();
            }
        }
        public SQFUWriter(string path, SQFUReader reader)
        {
            if(_CheckExtension(path))
            {
                try
                {
                    _sQFUReader = reader;
                    StreamReader sr = new StreamReader(path);
                    _sqfuF = sr.ReadToEnd();
                    this._path = path;
                    sr.Close();

                }
                catch
                {
                    throw new FileLoadException();
                }
            }
            else
            {
                throw new FileLoadException();
            }
        }

        private bool _CheckExtension(string path)
        {
            string extStr = "";
            for(int i = 5; i > 0; i--)
            {
                extStr += path[path.Length - i];
            }
            return extStr == ".sqfu";
        }

        public void ChangeStatus(StateQuest state, bool status)
        {

            string statsState = _GetStats(state);

            string newStatsState = statsState.Replace($"\"{_sQFUReader.GetStatus(state).ToString().ToLower()}\"", $"\"{status.ToString().ToLower()}\"");

            _sqfuF  = _sqfuF.Replace(statsState, newStatsState);
            File.WriteAllText(_path, _sqfuF);
        }

        private string _GetStats(StateQuest state)
        {
            Pair<string, string> checkState = null;
            switch(state)
            {
                case StateQuest.NotActive:
                    checkState = SQFU.NoActiveComm;
                    break;
                case StateQuest.Active:
                    checkState = SQFU.ActiveComm;
                    break;
                case StateQuest.Completed:
                    checkState = SQFU.CompletedComm;
                    break;
                case StateQuest.AfterCompleted:
                    checkState = SQFU.AfterCompletedComm;
                    break;
            }
            return _GetStringBetween(_sqfuF, checkState);
        }

        private string _GetStringBetween(string str, Pair <string, string> pair)
        {
            int indexStart = str.IndexOf(pair.First) + pair.First.Length;
            int indexEnd = str.IndexOf(pair.Second);

            return str.Substring(indexStart, indexEnd - indexStart);
        }
    }
}