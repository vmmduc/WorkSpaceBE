namespace Common.Constant
{
    public static class Role
    {
        public const string ADMIN = "ADMIN";
        public const string MEMBER = "MEMBER";
    }
    public static class Policy
    {
        public const string MEMBER = "MEMBER";
    }
    public static class State
    {
        public const string WaitingConfirm = "WCF";
        public const string Accept = "ACP";
        public const string Reject = "REJ";
        public const string Cancel = "CAN";
    }
    public enum FriendAction {
        ACCEPT, REJECT, CANCEL, ADD
    }
}
