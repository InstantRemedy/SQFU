using MySystem;
using System;
using System.Collections.Generic;

namespace SQFUSystem
{
    public class SQFUInOut //Main class that work wit SQFUWrite and SQFUReader
    {
        private SQFUReader _reader;
        private SQFUWriter _writer;

        public StateQuest CurrentState {get; private set;}
        public List<Pair<WhoTalk, string>> CurrentDialog {get; private set;}
        public List<Pair<string, int>> CurrentThings {get; private set;}


        public SQFUInOut(string path)
        {   
            _reader = new SQFUReader(path);
            _writer = new SQFUWriter(path, _reader);
            CurrentDialog = new List<Pair<WhoTalk, string>>();
            CurrentThings = new List<Pair<string, int>>();
            _CheckCurrentState();
        }

        private void _CheckCurrentState()
        {
            foreach(StateQuest state in Enum.GetValues(typeof(StateQuest)))
            {
                if(!_reader.GetStatus(state))
                {
                    CurrentState = state;

                    if(CurrentDialog.Count > 0) CurrentDialog.Clear();
                    CurrentDialog = _reader.GetMessages(state);
                    if(CurrentThings.Count > 0) CurrentThings.Clear();
                    CurrentThings = _reader.GetThings(state);
                    break;
                }
            }
        }

        public void ExecuteState()
        {
            if(CurrentState != StateQuest.AfterCompleted)
            {
                _writer.ChangeStatus(CurrentState, true);
                _reader.Update();
                _CheckCurrentState();
            }
        }
    }
}