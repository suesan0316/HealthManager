using Xamarin.Forms;

namespace HealthManager.Common.Extention
{
    public class ReciveRequestStackLayout : StackLayout
    {
        public static readonly BindableProperty RecivedRequestProperty =
            BindableProperty.Create(nameof(RecivedRequest), typeof(IEditChildrenRequest), typeof(ReciveRequestStackLayout), null
               , 
                propertyChanged: (b, o, n) =>
                {
                    var self = (ReciveRequestStackLayout) b;
                    var request = (IEditChildrenRequest) n;
                    if (request is ChildrenAddRequest)
                    {
                        self.Children.Add(((ChildrenAddRequest)request).Target);
                    }

                    if (request is ChildrenRemoveRequest)
                    {
                        self.Children.Remove(((ChildrenRemoveRequest)request).Target);
                    }

                    if (request is ChildrenRemoveRequest)
                    {
                        self.Children.Clear();
                    }

                });

        public IEditChildrenRequest RecivedRequest
        {
            get => (IEditChildrenRequest)GetValue(RecivedRequestProperty);
            set => SetValue(RecivedRequestProperty, value);
        }
    }

    public interface IEditChildrenRequest { }

    public class ChildrenAddRequest : IEditChildrenRequest
    {
        public Xamarin.Forms.View Target;
    }
    public class ChildrenRemoveRequest : IEditChildrenRequest
    {
        public Xamarin.Forms.View Target;
    }
    public class ChildrenClearRequest : IEditChildrenRequest { }

    public class RequestSender
    {
        public static ChildrenAddRequest SendChildrenAddRequest(Xamarin.Forms.View target)
        {
            return new ChildrenAddRequest(){Target = target};
        }
        public static ChildrenRemoveRequest SendChildrenRemoveRequest(Xamarin.Forms.View target)
        {
            return new ChildrenRemoveRequest() { Target = target };
        }
        public static ChildrenClearRequest SendChildrenClearRequest()
        {
            return new ChildrenClearRequest();
        }
    }
}
