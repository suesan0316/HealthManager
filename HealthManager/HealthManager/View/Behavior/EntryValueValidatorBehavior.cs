using System;
using Xamarin.Forms;

namespace HealthManager.View.Behavior
{
    public class EntryValueValidatorBehavior : Behavior<Entry>
    {
        public const int TypeString = 0;
        public const int TypeBool = 1;
        public const int TypeByte = 2;
        public const int TypeSbyte = 3;
        public const int TypeChar = 4;
        public const int TypeDecimal = 5;
        public const int TypeDouble = 6;
        public const int TypeFloat = 7;
        public const int TypeInt = 8;
        public const int TypeUint = 9;
        public const int TypeLong = 10;
        public const int TypeUlong = 11;
        public const int TypeObject = 12;
        public const int TypeShort = 13;
        public const int TypeUshort = 14;


        public int MaxLength { get; set; }
        public double MaxValue   { get; set; }
        public bool CheckMaxLength { get; set; }
        public bool CheckMaxValue { get; set; }

        public int ValueType { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var newValue = e.NewTextValue;
            var oldValue = e.OldTextValue;

            if (newValue.Length != 0)
            {
                if (CheckMaxLength)
                {
                    if (newValue.Length > MaxLength)
                    {
                        entry.Text = oldValue;
                    }
                }

                if (CheckMaxValue)
                {
                    if (!double.TryParse(newValue, out _) ||  double.Parse(newValue) > MaxValue)
                    {
                        entry.Text = oldValue;
                    }
                }
            }
        }
    }
}
