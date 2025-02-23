namespace Kelsey.UGUI
{
    public class Popup<TData1> : PopupBase
    {
        protected TData1 Data { get; private set; }

        public virtual void SetPopupData(TData1 data)
        {
            Data = data;
        }
    }

    public class Popup<TData1, TData2> : PopupBase
    {
        protected TData1 Data1 { get; private set; }
        protected TData2 Data2 { get; private set; }


        public virtual void SetPopupData(TData1 data1, TData2 data2)
        {
            Data1 = data1;
            Data2 = data2;
        }
    }
    
    // do it with 3 params
    public class Popup<TData1, TData2, TData3> : PopupBase
    {
        protected TData1 Data1 { get; private set; }
        protected TData2 Data2 { get; private set; }
        protected TData3 Data3 { get; private set; }

        public virtual void SetPopupData(TData1 data1, TData2 data2, TData3 data3)
        {
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
        }
    }
    
    // do it with 4 params
    public class Popup<TData1, TData2, TData3, TData4> : PopupBase
    {
        protected TData1 Data1 { get; private set; }
        protected TData2 Data2 { get; private set; }
        protected TData3 Data3 { get; private set; }
        protected TData4 Data4 { get; private set; }

        public virtual void SetPopupData(TData1 data1, TData2 data2, TData3 data3, TData4 data4)
        {
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            Data4 = data4;
        }
    }
}