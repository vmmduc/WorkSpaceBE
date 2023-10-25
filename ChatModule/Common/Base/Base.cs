namespace Common.Base
{
    public class BaseRequest<T>
    {
        public T Param { get; set; } = default!;
    }

    public class Base
    {
        public string? rtMessage { get; set; }
        public bool rtStatus { get; set; } = true;
        public int rtCode { get; set; }
    }

    public class BaseResult<T> : Base
    {
        public BaseResult() : base() { }
        public BaseResult(bool rtStatus, string rtMsg) : this()
        {
            this.rtStatus = rtStatus;
            rtMessage = rtMsg;
        }

        public BaseResult(T rtResult, bool rtStatus, int rtCode) : this()
        {
            this.rtResult = rtResult;
            this.rtStatus = rtStatus;
            this.rtCode = rtCode;
        }

        public BaseResult(List<T>? rtList, bool rtStatus, int rtCode) : this()
        {
            this.rtList = rtList ?? new List<T>();
            this.rtStatus = rtStatus;
            this.rtCode = rtCode;
        }

        public T? rtResult { get; set; }
        public List<T> rtList { get; set; } = new List<T>();
        public int rtTotal
        {
            get => rtList.Count;
            set { }
        }
    }
}
