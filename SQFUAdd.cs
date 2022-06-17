using MySystem;

namespace SQFUSystem
{
    public enum StateQuest
    {
        NotActive,
        Active,
        Completed,
        AfterCompleted
    }

    public enum CommandQuest
    {
        Status,
        Msg,
        Things
    }

    public enum WhoTalk
    {
        Player,
        NPC
    }

    public class SQFU 
    {
        public static Pair<string, string> NoActiveComm = new Pair<string, string>("<NotActive>", "<NotActive/>");
        public static Pair<string, string> ActiveComm = new Pair<string, string>("<Active>", "<Active/>");
        public static Pair<string, string> CompletedComm = new Pair<string, string>("<Completed>", "<Completed/>");
        public static Pair<string, string> AfterCompletedComm = new Pair<string, string>("<AfterCompleted>", "<AfterCompleted/>");

        public static  Pair<string, string> StatusComm = new Pair<string, string>("<Status>", "<Status/>");
        public static  Pair<string, string> MsgComm = new Pair<string, string>("<Msg>", "<Msg/>");
        public static  Pair<string, string> ThingsComm = new Pair<string, string>("<Things>", "<Things/>");

        public static  Pair<string, string> PlayerTalk = new Pair<string, string>("<PLAYER>", "<PLAYER/>");
        public static  Pair<string, string> NPCTalk = new Pair<string, string>("<NPC>", "<NPC/>");
    }
}