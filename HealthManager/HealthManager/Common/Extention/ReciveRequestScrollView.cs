using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HealthManager.Common.Extention
{
    public class ReciveRequestScrollView : ScrollView
    {
        public static readonly BindableProperty RecivedRequestProperty = BindableProperty.Create(nameof(RecivedRequest),
            typeof(ScrollRequest), typeof(VisualElement), null, BindingMode.OneWay, null,
            OnRecivedScrollRequest, null,
            null, null);

        public ScrollRequest RecivedRequest
        {
            get => (ScrollRequest) GetValue(RecivedRequestProperty);
            set => SetValue(RecivedRequestProperty, value);
        }

        private static void OnRecivedScrollRequest(BindableObject bindable, object oldvalue, object newvalue)
        {
            var self = (ReciveRequestScrollView) bindable;
            var request = (ScrollRequest) newvalue;
            switch (request.RequestType)
            {
                case ScrollRequestType.RequestTypeToTop:
                    self.ScrollToAsync(self, ScrollToPosition.Start, request.Animation);
                    break;
                case ScrollRequestType.RequestTypeToBottom:
                    self.ScrollToAsync(SearchLast(self.Children,self.Orientation, new SearchLastStruct()).ReturnElement, ScrollToPosition.End, request.Animation);
                    break;
                default:
                    break;
            }
        }

        private static SearchLastStruct SearchLast(IReadOnlyList<Element> target, ScrollOrientation orientation,
            SearchLastStruct current)
        {
            foreach (var element in target)
                if (element is Layout)
                {
                    SearchLast(((Layout) element).Children, orientation, current);
                }
                else
                {
                    if (element is Xamarin.Forms.View)
                        if (orientation == ScrollOrientation.Horizontal)
                        {
                            var tmp = ((Xamarin.Forms.View) element).X;
                            if (current.MaxX < tmp)
                            {
                                current.MaxX = tmp;
                                current.ReturnElement = element;
                            }
                        }
                        else
                        {
                            var tmp = ((Xamarin.Forms.View) element).Y;
                            if (current.MaxY < tmp)
                            {
                                current.MaxY = tmp;
                                current.ReturnElement = element;
                            }
                        }
                }
            return current;
        }

        public class SearchLastStruct
        {
            public double MaxX { get; set; }
            public double MaxY { get; set; }
            public Element ReturnElement { get; set; }
        }
    }

    public class ScrollRequest
    {
        private ScrollRequest()
        {
        }

        public int RequestType { get; set; }

        public bool Animation { get; set; }

        public static ScrollRequest SendScrollRequest(int requestType, bool animation = false)
        {
            GC.Collect();
            return new ScrollRequest {RequestType = requestType,Animation = animation};
        }
    }

    public static class ScrollRequestType
    {
        public const int RequestTypeToTop = 0;
        public const int RequestTypeToBottom = 1;
        public const int RequestTypeNone = 2;
    }
}