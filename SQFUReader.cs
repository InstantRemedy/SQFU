using MySystem;

namespace SQFUSystem
{
    class SQFUReader //NOT USE for gaming, can use for debugging, or check
    {   
        private string _sqfuF;
        private string _path;


        public SQFUReader(string path)
        {
            if(_CheckExtension(path))
            {
                try
                {
                    StreamReader reader = new StreamReader(path);
                    _sqfuF = reader.ReadToEnd();
                    _path = path;
                    reader.Close();
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
                    checkState = SQFU.ActiveComm;
                    break;
            }
            return _GetStringBetween(_sqfuF, checkState);
        }

        private List<string> _GetStringCommand(StateQuest state, CommandQuest command)
        {
            List<string> answer = new List<string>();
            
            string stats = _GetStats(state);

            Pair<string, string> checkCommand = null;
            switch(command)
            {
                case CommandQuest.Status:
                    checkCommand = SQFU.StatusComm;
                    break;
                case CommandQuest.Msg:
                    checkCommand = SQFU.MsgComm;
                    break;
                case CommandQuest.Things:
                    checkCommand = SQFU.ThingsComm;
                    break;
            }
            
            string checkStr = _GetStringBetween(stats, checkCommand);

            bool canCount = false;
            string temp = String.Empty;
            foreach(char c in checkStr)
            {
                if(c == '"' && !canCount)
                {
                    canCount = true;
                }
                else if(c == '"' && canCount)
                {
                    answer.Add(temp);
                    temp = String.Empty;
                    canCount = false;
                }
                else if(canCount) temp += c;
            }
            return answer;
        }    
        
        public bool GetStatus(StateQuest state) => Convert.ToBoolean(_GetStringCommand(state, CommandQuest.Status)[0]);
        
        public List <Pair<WhoTalk, string>> GetMessages(StateQuest state) 
        {
            List<Pair<WhoTalk, string>> messages = new List<Pair<WhoTalk, string>>();
            string stats = _GetStats(state);
            string allMsg = _GetStringBetween(stats, SQFU.MsgComm);
            
            List<Pair<Pair<int, int>, WhoTalk>> playerIndexes = new List<Pair<Pair<int, int>, WhoTalk>>();
            List<Pair<Pair<int, int>, WhoTalk>> npcIndexes = new List<Pair<Pair<int, int>, WhoTalk>>();
            
            var NPC_MS = new List<int>();
            var NPC_ME = new List<int>();
            var Player_MS = new List<int>();
            var Player_ME = new List<int>();
            for (int i = allMsg.IndexOf("<NPC>"); i > -1; i = allMsg.IndexOf("<NPC>", i + 1))
            {
                    NPC_MS.Add(i);
            }
            for (int i = allMsg.IndexOf("<NPC/>"); i > -1; i = allMsg.IndexOf("<NPC/>", i + 1))
            {
                    NPC_ME.Add(i);
            }
            for (int i = allMsg.IndexOf("<PLAYER>"); i > -1; i = allMsg.IndexOf("<PLAYER>", i + 1))
            {
                    Player_MS.Add(i);
            }
            for (int i = allMsg.IndexOf("<PLAYER/>"); i > -1; i = allMsg.IndexOf("<PLAYER/>", i + 1))
            {
                    Player_ME.Add(i);
            }
            for(int i = 0; i < Player_MS.Count; i++)
            {
                playerIndexes.Add(new Pair<Pair<int, int>, WhoTalk>(new Pair<int, int>(Player_MS[i], Player_ME[i]), WhoTalk.Player));
            }
            for(int i = 0; i < NPC_MS.Count; i++)
            {
                npcIndexes.Add(new Pair<Pair<int, int>, WhoTalk>(new Pair<int, int>(NPC_MS[i], NPC_ME[i]), WhoTalk.NPC));
            }
            List<Pair<Pair<int, int>, WhoTalk>> lstAllMsg = new List<Pair<Pair<int, int>, WhoTalk>>();

            foreach(var npc in npcIndexes)
            {
                lstAllMsg.Add(npc);

            }
            foreach(var Pl in playerIndexes) 
            {
                lstAllMsg.Add(Pl);
            }


            List<Pair<Pair<int, int>, WhoTalk>> QuickSort(List<Pair<Pair<int, int>, WhoTalk>> lst, int minIndex, int maxIndex)
            {
                if(minIndex >= maxIndex)
                {
                    return lst;
                }
                var pivotIndex = Partition(lst, minIndex, maxIndex);
                QuickSort(lst, minIndex, pivotIndex - 1);
                QuickSort(lst, pivotIndex + 1, maxIndex);

                return lst;
            }

            int Partition(List<Pair<Pair<int, int>, WhoTalk>> lst, int minIndex, int maxIndex)
            {
                var pivot = minIndex - 1;
                for (var i = minIndex; i < maxIndex; i++)
                {
                    if (lst[i].First.First < lst[maxIndex].First.First)
                    {
                        pivot++;
                        Swap(lst, pivot, i);
                    }
                }
                pivot++;
                Swap(lst, pivot, maxIndex);
                return pivot;
            }

            void Swap(List<Pair<Pair<int, int>, WhoTalk>> lst, int indexOne, int indexTwo)
            {
                var t = lst[indexOne];
                lstAllMsg[indexOne] = lst[indexTwo];
                lst[indexTwo] = t;
            }
            
            foreach(var p in QuickSort(lstAllMsg, 0, lstAllMsg.Count - 1))
            {
                string msg = allMsg.Substring(p.First.First, p.First.Second - p.First.First + 10);
                if(p.Second == WhoTalk.Player)
                {
                    msg = _GetStringBetween(msg, SQFU.PlayerTalk);
                }
                else if(p.Second == WhoTalk.NPC)
                {
                    msg = _GetStringBetween(msg, SQFU.NPCTalk);
                }
                bool canCount = false;
                string temp = String.Empty;
                foreach(char c in msg)
                {
                    if(c == '"' && !canCount)
                    {
                        canCount = true;
                    }
                    else if(c == '"' && canCount)
                    {
                        messages.Add(new Pair<WhoTalk, string>(p.Second, temp));
                        temp = String.Empty;
                        canCount = false;
                    }
                    else if(canCount) temp += c;
                }
            }

            return messages;
        }
       

        public List<Pair<string, int>> GetThings(StateQuest state)
        {
            List<Pair<string, int>> thingsList = new  List<Pair<string, int>>();
            List<string> strThings = _GetStringCommand(state, CommandQuest.Things);

            foreach(string strThing in strThings)
            {
                string[] arrThing = strThing.Split(':');
                thingsList.Add(new Pair<string, int>(arrThing[0], Convert.ToInt32(arrThing[1])));
            }
            return thingsList;
        }

        public void Update()
        {
            StreamReader reader = new StreamReader(_path);
            _sqfuF = reader.ReadToEnd();
            reader.Close();
        }

        private string _GetStringBetween(string str, Pair <string, string> pair)
        {
            int indexStart = str.IndexOf(pair.First) + pair.First.Length;
            int indexEnd = str.IndexOf(pair.Second);

            return str.Substring(indexStart, indexEnd - indexStart);
        }
    }
}
